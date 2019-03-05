using LazySQL.Core.Blueprint.CodeDesignBlueprint;
using LazySQL.Core.Blueprint.SystemBlueprint;
using LazySQL.Infrastructure;
using LazySQL.System;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LazySQL.Extends
{
    public enum CONDITION_TYPE
    {
        WHERE,
        VALUE,
        SET
    }

    public abstract class ICoreFactory
    {
        /// <summary>
        /// 导出脚本文件（不编译）
        /// </summary>
        /// <param name="connection">连接字符串</param>
        /// <param name="name">名称</param>
        /// <param name="xmlNode">操作节点</param>
        /// <param name="maxConditionsCount">最大条件数</param>
        /// <param name="path">导出地址</param>
        public void OutPutCSharp(string connection, string name, XmlNode xmlNode, int maxConditionsCount, string path)
        {
            if (xmlNode == null)
                throw new Exception($"未能从{name}中获取对应的XML模板");

            CodeDomProvider provider = null;
            try
            {
                CodeSetUp(connection
                    , name
                    , xmlNode
                    , maxConditionsCount
                    , out XmlNodeList parameters
                    , out Type codeReturnType
                    , out provider
                    , out CompilerParameters compilerParameters
                    , out CodeCompileUnit code);

                using (StreamWriter sourceWriter = new StreamWriter($"{path}\\{name}.cs"))
                {
                    CodeGeneratorOptions options = new CodeGeneratorOptions
                    {
                        BracingStyle = "C"
                    };
                    provider.GenerateCodeFromCompileUnit(
                        code, sourceWriter, options);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (provider != null)
                    provider.Dispose();
            }
        }

        /// <summary>
        /// 编译代码并将其生成方法存储至内存中
        /// </summary>
        /// <param name="connection">连接字符串</param>
        /// <param name="name">名称</param>
        /// <param name="xmlNode">操作节点</param>
        /// <param name="maxConditionsCount">最大条件数</param>
        public void Production(string connection, string name, XmlNode xmlNode, int maxConditionsCount)
        {
            if (xmlNode == null)
                throw new Exception($"未能从{name}中获取对应的XML模板");

            CodeDomProvider provider = null;
            try
            {
                CodeSetUp(connection
                    , name
                    , xmlNode
                    , maxConditionsCount
                    , out XmlNodeList parameters
                    , out Type codeReturnType
                    , out provider
                    , out CompilerParameters compilerParameters
                    , out CodeCompileUnit code);

                //将方法绑定至委托
                SystemMediator.Instance.DelegateSystem.SaveMethodInfo(name,
                    CodedomHelper.GetInstance().GetCompilerResults(provider,
                    compilerParameters,
                    code)
                    .CompiledAssembly
                    .CreateInstance($"Autogeneration.Dao.SQL.{name}Class")
                    .GetType()
                    .GetMethod(name), (parameters == null ? 0 : parameters.Count), codeReturnType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (provider != null)
                    provider.Dispose();
            }
        }

        /// <summary>
        /// 条件序号以及所在位置
        /// </summary>
        private class CondiIndex
        {
            /// <summary>
            /// 位置
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// 条件序号
            /// </summary>
            public int CondiCount { get; set; }

            /// <summary>
            /// 自动构建类型
            /// </summary>
            public CONDITION_TYPE cONDITION_TYPE { get; set; }
        }

        /// <summary>
        /// 条件序号以及SQL脚本
        /// </summary>
        protected class SqlFormat
        {
            /// <summary>
            /// 条件序号
            /// </summary>
            public int CondiIndex { get; set; }

            /// <summary>
            /// SQL脚本
            /// </summary>
            public string SQLText { get; set; }

            /// <summary>
            /// 自动构建类型
            /// </summary>
            public CONDITION_TYPE oNDITION_TYPE { get; set; }
        }

        /// <summary>
        /// 代码构成脚本
        /// </summary>
        /// <param name="connection">连接字符串</param>
        /// <param name="name">名称</param>
        /// <param name="xmlNode">操作节点</param>
        /// <param name="maxConditionsCount">最大条件数</param>
        /// <param name="parameters">返回属性数组</param>
        /// <param name="codeReturnType">返回数据类型</param>
        /// <param name="provider">返回代码编译容器</param>
        /// <param name="compilerParameters">返回代码参数</param>
        /// <param name="codeCompileUnit">返回代码容器</param>
        private void CodeSetUp(string connection
            , string name
            , XmlNode xmlNode
            , int maxConditionsCount
            , out XmlNodeList parameters
            , out Type codeReturnType
            , out CodeDomProvider provider
            , out CompilerParameters compilerParameters
            , out CodeCompileUnit codeCompileUnit)
        {
            provider = CodeDomProvider.CreateProvider("CSharp");
            parameters = null;
            codeReturnType = null;
            compilerParameters = null;
            codeCompileUnit = null;

            //从XML中获取Parameters所有内容
            XmlNode parametersFarther = XmlHelper.Instance.GetNode(xmlNode, "parameters");
            if (parametersFarther != null)
            {
                parameters = parametersFarther.ChildNodes;
            }
            //将Parameters内容格式化
            Dictionary<int, List<Dictionary<PARAMETER, string>>> parametersDict = ParFormatAction(out List<CodeParameterDeclarationExpression> codeParameterDeclarationExpressions, parameters);

            //新建代码容器
            codeCompileUnit = new CodeCompileUnit();

            //创建命名空间
            CodeNamespace nameSpace = new CodeNamespace("Autogeneration.Dao.SQL");

            //创建类
            CodeTypeDeclaration codeTypeDeclarations = new CodeTypeDeclaration();
            codeTypeDeclarations.Name = $"{name.ToUpper()}_CLASS";
            codeTypeDeclarations.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            //获取所有xml子节点
            XmlNodeList xmlNodeList = XmlHelper.Instance.GetAllNode(xmlNode);
            for (var i = 0; i < xmlNodeList.Count; i++)
            {
                XmlNode sqlNode = xmlNodeList[i];
                if (sqlNode.Name.Equals("parameters"))
                    continue;

                //返回类型
                string query = XmlHelper.Instance.GetNodeAttribute(xmlNode, "query");
                if (string.IsNullOrWhiteSpace(query))
                    query = "select";

                codeReturnType = query.ConventToTypes();

                //添加系统组件
                compilerParameters = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                };
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            
                //创建方法
                CodeMemberMethod codeMemberMethod = new CodeMemberMethod
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static
                };
                codeMemberMethod.Name = $"{name.ToUpper()}_{sqlNode.Name.ToUpper()}";
                codeMemberMethod.ReturnType = new CodeTypeReference(codeReturnType);

                //为方法添加参数
                codeMemberMethod.Parameters.AddRange(codeParameterDeclarationExpressions.ToArray());

                //格式化SQL字段，分割SQL并写出对应条件键值
                List<string> referencedAssemblies = new List<string>();
                codeMemberMethod.Statements.AddRange(Build(parametersDict
                    , SqlFormatAction(sqlNode.InnerText.Replace("\r", " ").Replace("\t", " ").Replace("\n", " ").Trim(), maxConditionsCount, out bool isNotActiveConditionInsQL)
                    , connection
                    , isNotActiveConditionInsQL
                    , referencedAssemblies
                    , codeReturnType
                    , query
                    ));
                if (referencedAssemblies.Count > 0)
                {
                    compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());
                }
                //在类中添加方法
                codeTypeDeclarations.Members.Add(codeMemberMethod);
            }

            //在命名空间添加类
            nameSpace.Types.Add(codeTypeDeclarations);
            //在代码容器中添加命名空间
            codeCompileUnit.Namespaces.Add(nameSpace);
        }

        /// <summary>
        /// 将Xml获取到的SQL格式化
        /// </summary>
        /// <param name="sQL">sQL字段</param>
        /// <param name="maxConditionsCount">最大条件数量</param>
        /// <returns></returns>
        private List<SqlFormat> SqlFormatAction(
            string sQL
            , int maxConditionsCount
            , out bool isNotActiveConditionInsQL)
        {
            //最终输出产物 { [条件:0,SQL:...,IsWhere:...] ,[条件:1,SQL:...,IsWhere:...] ,...}
            List<SqlFormat> sqlList = new List<SqlFormat>();

            List<string> spliteCount = new List<string>();
            List<CondiIndex> indexList = new List<CondiIndex>();

            isNotActiveConditionInsQL = true;

            #region 添加CondiIndex

            IndexAdd("", CONDITION_TYPE.VALUE, sQL, maxConditionsCount, spliteCount, indexList, ref isNotActiveConditionInsQL);

            IndexAdd("?", CONDITION_TYPE.WHERE, sQL, maxConditionsCount, spliteCount, indexList, ref isNotActiveConditionInsQL);

            IndexAdd("!", CONDITION_TYPE.SET, sQL, maxConditionsCount, spliteCount, indexList, ref isNotActiveConditionInsQL);

            #endregion

            //让格式化后的条件按照位置，从小到大排序
            indexList.Sort((x, y) => x.Index.CompareTo(y.Index));

            //以 {0},{1},{2}... 作为分割符，分割SQL字段
            List<string> sqlSplite = new List<string>(sQL.Split(spliteCount.ToArray(), StringSplitOptions.RemoveEmptyEntries));

            //格式化逻辑字符，按正常顺序输出，并带上逻辑条件标记（{0},{1},{2}...）
            if (!isNotActiveConditionInsQL)
            {
                for (var count = 0; count < sqlSplite.Count; count++)
                {
                    SqlFormat sqlFormat = new SqlFormat
                    {
                        SQLText = sqlSplite[count]
                    };

                    //将最后一段SQL标记为-1
                    if (count < indexList.Count)
                    {
                        sqlFormat.CondiIndex = indexList[count].CondiCount;
                        sqlFormat.oNDITION_TYPE = indexList[count].cONDITION_TYPE;
                    }
                    else
                        sqlFormat.CondiIndex = -1;

                    sqlList.Add(sqlFormat);
                }
            }
            else
            {
                //无任何逻辑字符直接输出
                sqlList.Add(new SqlFormat()
                {
                    CondiIndex = -1,
                    SQLText = sqlSplite[0]
                });
            }

            return sqlList;
        }

        private void IndexAdd(
            string otherAdd
            , CONDITION_TYPE cONDITION_TYPE
            , string sQL
            , int maxConditionsCount
            , List<string> spliteCount
            , List<CondiIndex> condiIndices
            , ref bool isNotActiveConditionInsQL)
        {
            if (string.IsNullOrWhiteSpace(sQL))
                return;

            for (var sqlCount = 0; sqlCount < maxConditionsCount; sqlCount++)
            {
                int index = 0;
                spliteCount.Add($"{{{sqlCount}{otherAdd}}}");
                while (index != -1)
                {
                    index = sQL.IndexOf($"{{{sqlCount}{otherAdd}}}", index + $"{{{sqlCount}{otherAdd}}}".Length);
                    if (index == -1)
                        break;

                    condiIndices.Add(new CondiIndex
                    {
                        Index = index,
                        CondiCount = sqlCount,
                        cONDITION_TYPE = cONDITION_TYPE
                    });
                    isNotActiveConditionInsQL = false;
                }
            }
        }

        /// <summary>
        /// 将Xml获取到的条件字段格式化
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private Dictionary<int, List<Dictionary<PARAMETER, string>>> ParFormatAction(out List<CodeParameterDeclarationExpression> codeParameterDeclarationExpressions
            , XmlNodeList parameters)
        {
            Dictionary<int, List<Dictionary<PARAMETER, string>>> parametersDict = new Dictionary<int, List<Dictionary<PARAMETER, string>>>();
            codeParameterDeclarationExpressions = new List<CodeParameterDeclarationExpression>();
            if (parameters != null)
            {
                for (var i = 0; i < parameters.Count; i++)
                {
                    string index = XmlHelper.Instance.GetNodeAttribute(parameters[i], "index");
                    string parName = XmlHelper.Instance.GetNodeAttribute(parameters[i], "name");
                    bool only = XmlHelper.Instance.GetNodeAttribute(parameters[i], "only").ToBool();

                    if (!only)
                    {
                        if (!string.IsNullOrWhiteSpace(index))
                        {
                            int sequenceIndex = Convert.ToInt32(index);
                            if (parametersDict.ContainsKey(sequenceIndex))
                            {
                                parametersDict[sequenceIndex].Add(GetXmlNodeAttribute(parameters[i]));
                            }
                            else
                            {
                                parametersDict.Add(sequenceIndex, new List<Dictionary<PARAMETER, string>>());
                                parametersDict[sequenceIndex].Add(GetXmlNodeAttribute(parameters[i]));
                            }
                        }
                        else
                        {
                            if (parametersDict.ContainsKey(0))
                            {
                                parametersDict[0].Add(GetXmlNodeAttribute(parameters[i]));
                            }
                            else
                            {
                                parametersDict.Add(0, new List<Dictionary<PARAMETER, string>>());
                                parametersDict[0].Add(GetXmlNodeAttribute(parameters[i]));
                            }
                        }
                    }

                    codeParameterDeclarationExpressions.Add(new CodeParameterDeclarationExpression(typeof(string), parName.Replace("@", string.Empty).Replace(".", string.Empty)));
                }
            }
            return parametersDict;
        }

        /// <summary>
        /// 获取XML节点并添加入字典数组的简易封装
        /// </summary>
        /// <param name="xmlNode">xml节点</param>
        /// <returns></returns>
        private Dictionary<PARAMETER, string> GetXmlNodeAttribute(XmlNode xmlNode)
        {
            Dictionary<PARAMETER, string> tmpDict = new Dictionary<PARAMETER, string>();

            //获取节点name属性
            string name = XmlHelper.Instance.GetNodeAttribute(xmlNode, "name");

            //获取节点symbol属性
            string symbol = XmlHelper.Instance.GetNodeAttribute(xmlNode, "symbol");

            //获取节点target属性
            string target = XmlHelper.Instance.GetNodeAttribute(xmlNode, "target");

            //获取节点template属性
            string template = XmlHelper.Instance.GetNodeAttribute(xmlNode, "template");

            #region 节点属性添加至属性字典(parameters)

            if (!string.IsNullOrWhiteSpace(name))
            {
                tmpDict.Add(PARAMETER.NAME, name);
            }
            else
            {
                throw new Exception("没有添加NAME或name字段",new XmlException());
            }

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                tmpDict.Add(PARAMETER.SYMBOL, symbol);
            }

            if (!string.IsNullOrWhiteSpace(target))
            {
                tmpDict.Add(PARAMETER.TARGET, target);
            }

            if (!string.IsNullOrWhiteSpace(template))
            {
                tmpDict.Add(PARAMETER.TEMPLATE, template);
            }

            #endregion

            return tmpDict;
        }

        /// <summary>
        /// 组件构成
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sQLs"></param>
        /// <param name="connection"></param>
        /// <param name="isNotCondition"></param>
        /// <param name="ReferencedAssemblies"></param>
        /// <returns></returns>
        protected abstract CodeStatementCollection Build(
            Dictionary<int, List<Dictionary<PARAMETER, string>>> parameters
            , List<SqlFormat> sQLs
            , string connection
            , bool isNotCondition
            , List<string> ReferencedAssemblies
            , Type returnType
            , string query);

        /// <summary>
        /// 代码构成模板
        /// </summary>
        /// <param name="isNotCondition"></param>
        /// <param name="parameters"></param>
        /// <param name="tryCodeStatementCollection"></param>
        /// <param name="paramterQuery"></param>
        /// <param name="type"></param>
        /// <param name="sqls"></param>
        /// <param name="stringBuilderBlueprint"></param>
        protected void Building(
            bool isNotCondition
            , Dictionary<int, List<Dictionary<PARAMETER, string>>> parameters
            , CodeStatementCollection tryCodeStatementCollection
            , IParamterQuery paramterQuery
            , Type type
            , List<SqlFormat> sqls
            , StringBuilderBlueprint stringBuilderBlueprint
            , string query
            , string connection
            , ITemplateBlueprint templateBlueprint
            , ListBlueprint listBlueprint)
        {
            Dictionary<string, string> SavePar = new Dictionary<string, string>();
            //按顺序添加SQL语句以及条件语句
            for (int sQLsCount = 0; sQLsCount < sqls.Count; sQLsCount++)
            {
                tryCodeStatementCollection.Add(stringBuilderBlueprint.Append(sqls[sQLsCount].SQLText));
                if (!isNotCondition)
                {
                    int CondiIndex = sqls[sQLsCount].CondiIndex;
                    if (CondiIndex != -1)
                    {
                        CreateCondition(SavePar, CondiIndex, parameters, tryCodeStatementCollection, paramterQuery, sqls, sQLsCount, stringBuilderBlueprint, sqls[sQLsCount].oNDITION_TYPE);
                    }
                }
            }

            //返回值
            ReturnBlueprint returnBlueprint = new ReturnBlueprint();
            switch (query)
            {
                case "select":
                    SelectReturn(connection
                        , tryCodeStatementCollection
                        , templateBlueprint
                        , returnBlueprint
                        , stringBuilderBlueprint
                        , listBlueprint);
                    break;

                default:
                    DefaultReturn(connection
                        , tryCodeStatementCollection
                        , templateBlueprint
                        , returnBlueprint
                        , stringBuilderBlueprint
                        , listBlueprint);
                    break;
            }
        }

        /// <summary>
        /// 生成条件语句
        /// </summary>
        /// <param name="savePar"></param>
        /// <param name="CondiIndex"></param>
        /// <param name="parameters"></param>
        /// <param name="tryCodeStatementCollection"></param>
        /// <param name="paramterQuery"></param>
        /// <param name="sqls"></param>
        /// <param name="sQLsCount"></param>
        /// <param name="stringBuilderBlueprint"></param>
        /// <param name="cONDITION_TYPE"></param>
        /// <param name="customName"></param>
        private void CreateCondition(
            Dictionary<string, string> savePar
            , int CondiIndex
            , Dictionary<int, List<Dictionary<PARAMETER, string>>> parameters
            , CodeStatementCollection tryCodeStatementCollection
            , IParamterQuery paramterQuery
            , List<SqlFormat> sqls
            , int sQLsCount
            , StringBuilderBlueprint stringBuilderBlueprint
            , CONDITION_TYPE cONDITION_TYPE
            , string customName = "")
        {
            if (!savePar.ContainsKey($"{CondiIndex}Par{customName}"))
            {
                StringBuilderBlueprint stringBuilderBlueprintTmp = new StringBuilderBlueprint($"par{CondiIndex}{customName}");

                tryCodeStatementCollection.Add(stringBuilderBlueprintTmp.Create());

                for (int parametersChildCount = 0; parametersChildCount < parameters[CondiIndex].Count; parametersChildCount++)
                {
                    tryCodeStatementCollection.AddRange(paramterQuery.Create(stringBuilderBlueprintTmp
                        , parameters[CondiIndex][parametersChildCount]
                        , cONDITION_TYPE));
                }

                savePar.Add($"{CondiIndex}Par{customName}", stringBuilderBlueprintTmp.Field);
            }

            if (sqls[sQLsCount].CondiIndex != -1)
                tryCodeStatementCollection.Add(stringBuilderBlueprint.AppendField($"par{CondiIndex}{customName}"));
        }

        protected abstract void SelectReturn(string connection
            , CodeStatementCollection codeStatementCollection
            , ITemplateBlueprint templateBlueprint
            , ReturnBlueprint returnBlueprint
            , StringBuilderBlueprint stringBuilderBlueprint
            , ListBlueprint listBlueprint);

        protected abstract void DefaultReturn(string connection
            , CodeStatementCollection codeStatementCollection
            , ITemplateBlueprint templateBlueprint
            , ReturnBlueprint returnBlueprint
            , StringBuilderBlueprint stringBuilderBlueprint
            , ListBlueprint listBlueprint);
    }
}
