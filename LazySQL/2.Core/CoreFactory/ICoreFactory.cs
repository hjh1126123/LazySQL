using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.MethodEncapsulation;
using LazySQL.Core.CoreSystem;
using LazySQL.Infrastructure;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LazySQL.Core.CoreFactory
{
    public abstract class ICoreFactory
    {
        SystemMediator systemMediator = null;
        public ICoreFactory(SystemMediator systemMediator)
        {
            this.systemMediator = systemMediator;
        }

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
                throw ex.ThrowMineFormat(this, "Production", name);
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
                systemMediator.StorageSystem.SaveMethodInfo(name,
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
                throw ex.ThrowMineFormat(this, "Production", name);
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
            /// 是否是条件语句
            /// </summary>
            public bool Where { get; set; }
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
            /// 是否是Where
            /// </summary>
            public bool IsWhere { get; set; }
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

            XmlNode sqlNode = XmlHelper.GetInstance().GetNode(xmlNode, "SQL");
            if (sqlNode == null)
                throw new Exception($"{name}不包含SQL节点，该XML文档是无效的");

            string sQL = sqlNode.InnerText.Replace("\r", " ").Replace("\t", " ").Replace("\n", " ").Trim();

            //诺是无任何条件标记则为true (即是无 {0},{1},{2},... 中任意一个)
            bool isNotActiveConditionInsQL;

            //格式化SQL字段，分割SQL并写出对应条件键值
            List<SqlFormat> sqlList = SqlFormatAction(sQL, maxConditionsCount, out isNotActiveConditionInsQL);

            #region 格式化条件字符

            List<CodeParameterDeclarationExpression> codeParameterDeclarationExpressions = new List<CodeParameterDeclarationExpression>();

            //从XML中获取Parameters所有内容
            XmlNode parametersFarther = XmlHelper.GetInstance().GetNode(xmlNode, "Parameters");
            parameters = null;
            if (parametersFarther != null)
            {
                parameters = XmlHelper.GetInstance().GetNode(xmlNode, "Parameters").ChildNodes;
            }
            //将Parameters内容格化式
            Dictionary<int, List<Dictionary<PARAMETER, string>>> parametersDict = ParFormatAction(codeParameterDeclarationExpressions, parameters);

            #endregion

            //返回类型
            string query = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "query");
            if (string.IsNullOrWhiteSpace(query))
                query = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "Query");
            if (string.IsNullOrWhiteSpace(query))
                query = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "QUERY");
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

            codeCompileUnit = new CodeCompileUnit();

            //创建命名空间
            CodeNamespace nameSpace = new CodeNamespace("Autogeneration.Dao.SQL");

            //创建类
            CodeTypeDeclaration codeTypeDeclarations = new CodeTypeDeclaration();
            codeTypeDeclarations.Name = $"{name}Class";
            codeTypeDeclarations.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            //创建方法
            CodeMemberMethod codeMemberMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static
            };
            codeMemberMethod.Name = name;
            codeMemberMethod.ReturnType = new CodeTypeReference(codeReturnType);

            //为方法添加参数
            codeMemberMethod.Parameters.AddRange(codeParameterDeclarationExpressions.ToArray());
            List<string> referencedAssemblies = new List<string>();
            codeMemberMethod.Statements.AddRange(Build(parametersDict
                , sqlList
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
        private List<SqlFormat> SqlFormatAction(string sQL
            , int maxConditionsCount
            , out bool isNotActiveConditionInsQL)
        {
            //最终输出产物 { [条件:0,SQL:...,IsWhere:...] ,[条件:1,SQL:...,IsWhere:...] ,...}
            List<SqlFormat> sqlList = new List<SqlFormat>();

            List<string> spliteCount = new List<string>();
            List<CondiIndex> indexList = new List<CondiIndex>();

            isNotActiveConditionInsQL = true;

            //获取({0},{1},{2}...)的位置，并格式化为 { [条件(ConditionSign):0,位置(Index):153], [条件:1,位置:178] ,...}
            for (var sqlCount = 0; sqlCount < maxConditionsCount; sqlCount++)
            {
                int index = 0;
                spliteCount.Add($"{{{sqlCount}}}");
                while (index != -1)
                {
                    index = sQL.IndexOf($"{{{sqlCount}}}", index + $"{{{sqlCount}}}".Length);
                    if (index == -1)
                        break;

                    indexList.Add(new CondiIndex
                    {
                        Index = index,
                        CondiCount = sqlCount,
                        Where = false
                    });
                    isNotActiveConditionInsQL = false;
                }
            }

            //获取({0?},{1?},{2?}...)的位置，并格式化为 { [条件(ConditionSign):0,位置(Index):153], [条件:1,位置:178] ,...}
            for (var sqlCount = 0; sqlCount < maxConditionsCount; sqlCount++)
            {
                int index = 0;
                spliteCount.Add($"{{{sqlCount}?}}");
                while (index != -1)
                {
                    index = sQL.IndexOf($"{{{sqlCount}?}}", index + $"{{{sqlCount}?}}".Length);
                    if (index == -1)
                        break;

                    indexList.Add(new CondiIndex
                    {
                        Index = index,
                        CondiCount = sqlCount,
                        Where = true
                    });
                    isNotActiveConditionInsQL = false;
                }
            }

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
                        sqlFormat.IsWhere = indexList[count].Where;
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
                    SQLText = sqlSplite[0],
                    IsWhere = false
                });
            }

            return sqlList;
        }

        /// <summary>
        /// 将Xml获取到的条件字段格式化
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private Dictionary<int, List<Dictionary<PARAMETER, string>>> ParFormatAction(List<CodeParameterDeclarationExpression> codeParameterDeclarationExpressions
            , XmlNodeList parameters)
        {
            Dictionary<int, List<Dictionary<PARAMETER, string>>> parametersDict = new Dictionary<int, List<Dictionary<PARAMETER, string>>>();
            if (parameters != null)
            {
                for (var i = 0; i < parameters.Count; i++)
                {
                    string index = XmlHelper.GetInstance().GetNodeAttribute(parameters[i], "INDEX");
                    string parName = XmlHelper.GetInstance().GetNodeAttribute(parameters[i], "NAME");
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
                    codeParameterDeclarationExpressions.Add(new CodeParameterDeclarationExpression(typeof(string), parName.Replace("@", "").Replace(".", string.Empty)));
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
            string name = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "NAME");
            if (string.IsNullOrWhiteSpace(name))
                name = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "name");

            //获取节点symbol属性
            string symbol = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "SYMBOL");
            if (string.IsNullOrWhiteSpace(symbol))
                symbol = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "symbol");

            //获取节点target属性
            string target = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "TARGET");
            if (string.IsNullOrWhiteSpace(target))
                target = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "target");

            //获取节点template属性
            string template = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "TEMPLATE");
            if (string.IsNullOrWhiteSpace(template))
                template = XmlHelper.GetInstance().GetNodeAttribute(xmlNode, "template");

            #region 节点属性添加至属性字典(parameters)

            if (!string.IsNullOrWhiteSpace(name))
            {
                tmpDict.Add(PARAMETER.NAME, name);
            }
            else
            {
                throw new Exception("没有添加NAME或name字段");
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
        protected abstract CodeStatementCollection Build(Dictionary<int, List<Dictionary<PARAMETER, string>>> parameters
            , List<SqlFormat> sQLs
            , string connection
            , bool isNotCondition
            , List<string> ReferencedAssemblies
            , Type returnType
            , string query);

        /// <summary>
        /// 代码构成享元模板
        /// </summary>
        /// <param name="isNotCondition"></param>
        /// <param name="parameters"></param>
        /// <param name="tryCodeStatementCollection"></param>
        /// <param name="paramterQuery"></param>
        /// <param name="type"></param>
        /// <param name="sqls"></param>
        /// <param name="stringBuilderBlueprint"></param>
        protected void Building(bool isNotCondition
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
            //拥有条件字段时
            if (!isNotCondition)
            {
                //循环创建拼接语句
                for (int parametersCount = 0; parametersCount < parameters.Count; parametersCount++)
                {
                    StringBuilderBlueprint stringBuilderBlueprintTmp = new StringBuilderBlueprint($"par{parametersCount}");

                    tryCodeStatementCollection.Add(stringBuilderBlueprintTmp.Create());

                    for (int parametersChildCount = 0; parametersChildCount < parameters[parametersCount].Count; parametersChildCount++)
                    {
                        bool isLastOne = (parametersChildCount == parameters[parametersCount].Count - 1);

                        tryCodeStatementCollection.AddRange(paramterQuery.Create(stringBuilderBlueprintTmp
                            , parameters[parametersCount][parametersChildCount]
                            , isLastOne
                            , type
                            , query));
                    }
                }
            }

            //按顺序添加SQL语句以及条件语句
            for (int sQLsCount = 0; sQLsCount < sqls.Count; sQLsCount++)
            {
                tryCodeStatementCollection.Add(stringBuilderBlueprint.Append(sqls[sQLsCount].SQLText));
                if (!isNotCondition)
                {
                    if (sqls[sQLsCount].CondiIndex != -1)
                        tryCodeStatementCollection.Add(stringBuilderBlueprint.AppendField($"par{sqls[sQLsCount].CondiIndex}"));
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
