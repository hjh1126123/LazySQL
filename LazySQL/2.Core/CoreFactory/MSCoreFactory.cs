using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.MethodEncapsulation;
using LazySQL.Core.CoreSystem;
using LazySQL.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LazySQL.Core.CoreFactory
{
    public class MSCoreFactory : ICoreFactory
    {
        public MSCoreFactory(SystemMediator systemMediator) : base(systemMediator)
        {
        }

        protected override CodeStatementCollection Build(Dictionary<int, List<Dictionary<PARAMETER, string>>> parameters
            , List<SqlFormat> sQLs
            , string connection
            , bool isNotCondition
            , List<string> ReferencedAssemblies
            , Type type)
        {

            CodeStatementCollection codeStatementCollection = new CodeStatementCollection();

            #region 添加所需零件以及创建零件

            StringBuilderBlueprint stringBuilderBlueprint = new StringBuilderBlueprint();

            ConnectionBlueprint<SqlConnection> connectionBlueprint = new ConnectionBlueprint<SqlConnection>(connection);

            CommandBlueprint<SqlCommand> commandBlueprint = new CommandBlueprint<SqlCommand>();

            DataAdapterBlueprint<SqlDataAdapter> dataAdapterBlueprint = new DataAdapterBlueprint<SqlDataAdapter>();

            DataSetBlueprint<DataSet> dataSetBlueprint = new DataSetBlueprint<DataSet>();

            codeStatementCollection.Add(stringBuilderBlueprint.Create());
            codeStatementCollection.Add(connectionBlueprint.Create());            
            codeStatementCollection.Add(commandBlueprint.Create());
            codeStatementCollection.Add(dataAdapterBlueprint.Create());
            codeStatementCollection.Add(dataSetBlueprint.Create());

            #endregion

            codeStatementCollection.Add(connectionBlueprint.Open());

            TryCatchFinallyBlueprint tryCatchFinallyBlueprint = new TryCatchFinallyBlueprint();




            codeStatementCollection.Add(tryCatchFinallyBlueprint.Create(() =>
            {
                CodeStatementCollection tryCodeStatementCollection = new CodeStatementCollection();

                tryCodeStatementCollection.Add(commandBlueprint.ConAssign(connectionBlueprint.Field));

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
                            tryCodeStatementCollection.AddRange(new MsSqlParamterQuery(commandBlueprint).Create(stringBuilderBlueprintTmp, parameters[parametersCount][parametersChildCount], isLastOne));
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

                
                tryCodeStatementCollection.Add(commandBlueprint.CmdTextAssign(stringBuilderBlueprint.Field));
                tryCodeStatementCollection.Add(dataAdapterBlueprint.CmdAssign(commandBlueprint.Field));
                tryCodeStatementCollection.Add(dataAdapterBlueprint.DsAssign(dataSetBlueprint.Field));

                ReturnBlueprint returnBlueprint = new ReturnBlueprint();
                if (type == typeof(bool))
                {
                    tryCodeStatementCollection.Add(returnBlueprint.ReturnTrue());
                }
                else
                {
                    tryCodeStatementCollection.Add(returnBlueprint.ReturnDataTable(dataSetBlueprint.Field));
                }

                return tryCodeStatementCollection;
            }
            , () =>
            {
                CodeStatementCollection catchCodeStatementCollection = new CodeStatementCollection();
                return catchCodeStatementCollection;
            }
            , () =>
            {
                CodeStatementCollection finallyCodeStatementCollection = new CodeStatementCollection();
                finallyCodeStatementCollection.Add(connectionBlueprint.Dispose());
                finallyCodeStatementCollection.Add(commandBlueprint.Dispose());
                finallyCodeStatementCollection.Add(dataAdapterBlueprint.Dispose());
                finallyCodeStatementCollection.Add(dataSetBlueprint.Dispose());
                return finallyCodeStatementCollection;
            }));

            return codeStatementCollection;
        }
    }
}
