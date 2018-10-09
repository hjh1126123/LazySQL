using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using LazySQL.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace LazySQL.Core.CoreFactory.MethodEncapsulation
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

        public CodeStatementCollection Create(StringBuilderBlueprint sqlStrBlueprint,
            Dictionary<PARAMETER, string> paramter,
            bool isLastOne)
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

            int value = 0;
            if (paramter.ContainsKey(PARAMETER.VALUE))
            {
                value = 1;
            }

            #endregion

            this.fieldName = fieldName;

            codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"!string.IsNullOrWhiteSpace({fieldName})",
               () =>
               {
                   CodeStatementCollection codeStatementCollectionIF = new CodeStatementCollection();
                   switch (value)
                   {
                       case 1:
                           codeStatementCollectionIF.Add(sqlStrBlueprint.Append($"@{fieldName}"));
                           normalBuild(codeStatementCollectionIF);
                           if (!isLastOne)
                               codeStatementCollectionIF.Add(stringBuilderBlueprint.Append(" , "));
                           break;

                       default:
                           if (!needSplit)
                           {
                               codeStatementCollectionIF.Add(sqlStrBlueprint.Append($"{target} {symbol} {(string.IsNullOrWhiteSpace(template) ? $"@{fieldName}" : template)}"));
                               normalBuild(codeStatementCollectionIF);
                           }
                           else
                           {
                               ListBlueprint listBlueprint = new ListBlueprint($"{fieldName}List");
                               codeStatementCollectionIF.Add(listBlueprint.Create<string>($"{fieldName}.Split(',')"));
                               codeStatementCollectionIF.Add(sqlStrBlueprint.Append($"{target} {symbol} {templates[0]}"));

                               codeStatementCollectionIF.Add(ToolManager.Instance.CircleTool.CreateCircle("int i = 0", $"i < {fieldName}List.Count", "i++", () =>
                               {
                                   CodeStatementCollection codeStatementCollectionFor = new CodeStatementCollection();
                                   CircleBuild(codeStatementCollectionFor);
                                   return codeStatementCollectionFor;
                               }));

                               codeStatementCollectionIF.Add(sqlStrBlueprint.Append(templates[1]));
                           }
                           if (!isLastOne)
                           {
                               codeStatementCollectionIF.Add(stringBuilderBlueprint.Append(" AND "));
                           }
                           break;
                   }
                   return codeStatementCollectionIF;
               }));

            return codeStatementCollection;
        }

        protected string fieldName = string.Empty;

        protected StringBuilderBlueprint stringBuilderBlueprint;

        protected abstract void normalBuild(CodeStatementCollection codeStatementCollection);

        protected abstract void CircleBuild(CodeStatementCollection codeStatementCollection);
    }
}
