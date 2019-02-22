using LazySQL.Core.Blueprint;
using LazySQL.Core.Blueprint.SystemBlueprint;
using LazySQL.Core.Tools;
using LazySQL.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace LazySQL.Extends
{
    public abstract class IParamterQuery
    {
        List<string> ConditionSplit;
        public IParamterQuery()
        {
            ConditionSplit = new List<string>
            {
                "in",
                "not in"
            };
        }

        public CodeStatementCollection Create(StringBuilderBlueprint sqlStrBlueprint
            , Dictionary<PARAMETER, string> paramter
            , CONDITION_TYPE cONDITION_TYPE)
        {
            stringBuilderBlueprint = sqlStrBlueprint;

            #region 属性赋值及其格式化处理

            CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
            bool needSplit = false;

            string name;
            if (paramter.ContainsKey(PARAMETER.NAME))
            {
                name = paramter[PARAMETER.NAME];
            }
            else
            {
                throw new Exception("不存在Name属性");
            }
            string fieldName = name.Replace(".", string.Empty);

            string target;
            if (paramter.ContainsKey(PARAMETER.TARGET))
            {
                target = paramter[PARAMETER.TARGET];
            }
            else
            {
                target = name;
            }

            string symbol;
            if (paramter.ContainsKey(PARAMETER.SYMBOL))
            {
                symbol = paramter[PARAMETER.SYMBOL];
                foreach (var c in ConditionSplit)
                {
                    if (symbol.Equals(c, StringComparison.InvariantCultureIgnoreCase))
                        needSplit = true;
                }
            }
            else
            {
                symbol = "=";
            }

            string template = string.Empty;
            List<string> templates = new List<string>();
            if (paramter.ContainsKey(PARAMETER.TEMPLATE))
            {
                template = paramter[PARAMETER.TEMPLATE];

                if (template.IndexOf("*") == -1)
                {
                    throw new Exception("在模板中，没有作为标记的'*'符号");
                }

                if (needSplit)
                {
                    templates = new List<string>(template.Split('*'));
                }
            }
            else
            {
                if (needSplit)
                    throw new Exception("你使用了in,not in这类多参数的传值没有写模板");
            }

            #endregion

            //FieldName赋值，方便给子类使用
            this.fieldName = fieldName;

            switch (cONDITION_TYPE)
            {
                case CONDITION_TYPE.SET:
                    codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"!string.IsNullOrWhiteSpace({fieldName})",
                        () =>
                        {
                            CodeStatementCollection codeStatementCollectionIF = new CodeStatementCollection();
                            codeStatementCollectionIF.Add(sqlStrBlueprint.Append($"{fieldName} = @{fieldName}ParSET,"));
                            SetTrue(codeStatementCollectionIF);                            
                            return codeStatementCollectionIF;
                        }));
                    break;

                case CONDITION_TYPE.VALUE:
                    codeStatementCollection.Add(sqlStrBlueprint.Append($"@{fieldName}VALUE"));
                    codeStatementCollection.Add(sqlStrBlueprint.Append(" , "));
                    codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"!string.IsNullOrWhiteSpace({fieldName})",
                        () =>
                        {
                            CodeStatementCollection codeStatementCollectionIF = new CodeStatementCollection();
                            ValueTrue(codeStatementCollectionIF);
                            return codeStatementCollectionIF;
                        },
                        () =>
                        {
                            CodeStatementCollection codeStatementCollectionElse = new CodeStatementCollection();
                            ValueFalse(codeStatementCollectionElse);
                            return codeStatementCollectionElse;
                        }));
                    break;

                case CONDITION_TYPE.WHERE:
                    codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"!string.IsNullOrWhiteSpace({fieldName})",
                      () =>
                      {
                          CodeStatementCollection codeStatementCollectionIF = new CodeStatementCollection();
                          codeStatementCollectionIF.Add(stringBuilderBlueprint.Append(" AND "));
                          if (!needSplit)
                          {
                              codeStatementCollectionIF.Add(sqlStrBlueprint.Append($"{target} {symbol} {(string.IsNullOrWhiteSpace(template) ? $"@{fieldName}" : template)}"));
                              ExecuteDataTableNormalBuild(codeStatementCollectionIF);
                          }
                          else
                          {
                              ListBlueprint listBlueprint = new ListBlueprint($"{fieldName}List");
                              codeStatementCollectionIF.Add(listBlueprint.Create<string>($"{fieldName}.Split(',')"));
                              codeStatementCollectionIF.Add(sqlStrBlueprint.Append($"{target} {symbol} {templates[0]}"));

                              codeStatementCollectionIF.Add(ToolManager.Instance.CircleTool.CreateCircle("int i = 0", $"i < {fieldName}List.Count", "i++", () =>
                              {
                                  CodeStatementCollection codeStatementCollectionFor = new CodeStatementCollection();
                                  ExecuteDataTableCircleBuild(codeStatementCollectionFor);
                                  return codeStatementCollectionFor;
                              }));

                              codeStatementCollectionIF.Add(sqlStrBlueprint.Append(templates[1]));
                          }
                          return codeStatementCollectionIF;
                      }));
                    break;
            }

            return codeStatementCollection;
        }

        protected string fieldName = string.Empty;

        protected StringBuilderBlueprint stringBuilderBlueprint;

        protected abstract void SetTrue(CodeStatementCollection codeStatementCollection);

        protected abstract void ValueTrue(CodeStatementCollection codeStatementCollection);

        protected abstract void ValueFalse(CodeStatementCollection codeStatementCollection);

        protected abstract void ExecuteDataTableNormalBuild(CodeStatementCollection codeStatementCollection);

        protected abstract void ExecuteDataTableCircleBuild(CodeStatementCollection codeStatementCollection);

        protected void SimpleExecuteDataTableCircleBuild<T>(T par, ListBlueprint listBlueprint, CodeStatementCollection codeStatementCollection) where T : IBlueprint, IParmsBlueprint
        {
            codeStatementCollection.Add(stringBuilderBlueprint.Append($"@{fieldName}"));
            codeStatementCollection.Add(stringBuilderBlueprint.AppendField("i"));
            codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"i != ({fieldName}List.Count - 1)", () =>
            {
                CodeStatementCollection codeStatementCollectionTmpIF = new CodeStatementCollection();
                codeStatementCollectionTmpIF.Add(stringBuilderBlueprint.Append(","));
                return codeStatementCollectionTmpIF;
            }));
            
            codeStatementCollection.Add(par.Create($"\"@{fieldName}\" + i", $"{fieldName}List[i]"));
            codeStatementCollection.Add(listBlueprint.Add(par.Field));
        }

        protected void SimpleExecuteDataTableNormalBuild<T>(T par, ListBlueprint listBlueprint, CodeStatementCollection codeStatementCollection) where T : IBlueprint, IParmsBlueprint
        {
            codeStatementCollection.Add(par.Create($"\"@{fieldName}\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(par.Field));
        }

        protected void SimpleSetTrue<T>(T par, ListBlueprint listBlueprint, CodeStatementCollection codeStatementCollection) where T : IBlueprint, IParmsBlueprint
        {
            codeStatementCollection.Add(par.Create($"\"@{fieldName}ParSET\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(par.Field));
        }

        protected void SimpleValueTrue<T>(T par, ListBlueprint listBlueprint, CodeStatementCollection codeStatementCollection) where T : IBlueprint, IParmsBlueprint
        {           
            codeStatementCollection.Add(par.Create($"\"@{fieldName}VALUE\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(par.Field));
        }

        protected void SimpleValueFalse<T>(T par, ListBlueprint listBlueprint, CodeStatementCollection codeStatementCollection) where T : IBlueprint, IParmsBlueprint
        {
            codeStatementCollection.Add(par.Create($"\"@{fieldName}VALUE\"", "\"\""));
            codeStatementCollection.Add(listBlueprint.Add(par.Field));
        }
    }
}
