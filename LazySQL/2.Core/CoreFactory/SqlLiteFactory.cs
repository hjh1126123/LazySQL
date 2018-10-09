using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.MethodEncapsulation;
using LazySQL.Core.CoreSystem;
using LazySQL.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SQLite;

namespace LazySQL.Core.CoreFactory
{
    public class SqlLiteFactory : ICoreFactory
    {
        public SqlLiteFactory(SystemMediator systemMediator) : base(systemMediator)
        {

        }

        protected override CodeStatementCollection Build(Dictionary<int, List<Dictionary<PARAMETER, string>>> parameters
            , List<SqlFormat> sQLs
            , string connection
            , bool isNotCondition
            , List<string> ReferencedAssemblies
            , Type type)
        {
            ReferencedAssemblies.Add("LazySQL.dll");
            ReferencedAssemblies.Add("System.Data.SQLite.dll");

            CodeStatementCollection codeStatementCollection = new CodeStatementCollection();

            StringBuilderBlueprint stringBuilderBlueprint = new StringBuilderBlueprint();

            SQLiteTemplateBlueprint sQLiteTemplateBlueprint = new SQLiteTemplateBlueprint();

            TryCatchFinallyBlueprint tryCatchFinallyBlueprint = new TryCatchFinallyBlueprint();

            codeStatementCollection.Add(stringBuilderBlueprint.Create());

            codeStatementCollection.Add(sQLiteTemplateBlueprint.Create());

            codeStatementCollection.Add(tryCatchFinallyBlueprint.Create(() =>
            {
                CodeStatementCollection tryCodeStatementCollection = new CodeStatementCollection();
                ListBlueprint listBlueprint = new ListBlueprint();
                tryCodeStatementCollection.Add(listBlueprint.Create<SQLiteParameter>());

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
                            tryCodeStatementCollection.AddRange(new SqlLiteParamterQuery(listBlueprint).Create(stringBuilderBlueprintTmp, parameters[parametersCount][parametersChildCount], isLastOne));
                        }
                    }
                }

                //按顺序添加SQL语句以及条件语句
                for (int sQLsCount = 0; sQLsCount < sQLs.Count; sQLsCount++)
                {
                    tryCodeStatementCollection.Add(stringBuilderBlueprint.Append(sQLs[sQLsCount].SQLText));
                    if (!isNotCondition)
                    {
                        if (sQLs[sQLsCount].Condition != -1)
                            tryCodeStatementCollection.Add(stringBuilderBlueprint.AppendField($"par{sQLs[sQLsCount].Condition}"));
                    }
                }

                //返回值
                ReturnBlueprint returnBlueprint = new ReturnBlueprint();
                if (type == typeof(bool))
                {                    
                    tryCodeStatementCollection.Add(sQLiteTemplateBlueprint.ExecuteNonQuery(connection, stringBuilderBlueprint.Field, listBlueprint.Field));
                    tryCodeStatementCollection.Add(returnBlueprint.ReturnTrue());
                }
                else
                {
                    tryCodeStatementCollection.Add(returnBlueprint.ReturnExpress(sQLiteTemplateBlueprint.ExecuteDataTable(connection, stringBuilderBlueprint.Field, listBlueprint.Field)));
                }
                return tryCodeStatementCollection;
            }, () =>
             {
                 CodeStatementCollection catchCodeStatementCollection = new CodeStatementCollection();
                 return catchCodeStatementCollection;
             }));

            return codeStatementCollection;
        }
    }
}
