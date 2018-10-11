using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.MethodEncapsulation;
using LazySQL.Core.CoreSystem;
using LazySQL.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
            , Type type
            , string query)
        {
            ReferencedAssemblies.Add("LazySQL.dll");

            CodeStatementCollection codeStatementCollection = new CodeStatementCollection();

            #region 添加所需零件以及创建零件

            StringBuilderBlueprint stringBuilderBlueprint = new StringBuilderBlueprint();

            MsSQLTemplateBlueprint msSQLTemplateBlueprint = new MsSQLTemplateBlueprint();

            #endregion

            codeStatementCollection.Add(stringBuilderBlueprint.Create());

            codeStatementCollection.Add(msSQLTemplateBlueprint.Create());

            TryCatchFinallyBlueprint tryCatchFinallyBlueprint = new TryCatchFinallyBlueprint();

            codeStatementCollection.Add(tryCatchFinallyBlueprint.Create(() =>
            {
                CodeStatementCollection tryCodeStatementCollection = new CodeStatementCollection();

                ListBlueprint listBlueprint = new ListBlueprint();

                tryCodeStatementCollection.Add(listBlueprint.Create<SqlParameter>());

                Building(isNotCondition
                    , parameters
                    , tryCodeStatementCollection
                    , new MsSqlParamterQuery(listBlueprint)
                    , type
                    , sQLs
                    , stringBuilderBlueprint
                    , query
                    , connection
                    , msSQLTemplateBlueprint
                    , listBlueprint);

                return tryCodeStatementCollection;
            }));

            return codeStatementCollection;
        }

        protected override void DefaultReturn(string connection, CodeStatementCollection codeStatementCollection, ITemplateBlueprint templateBlueprint, ReturnBlueprint returnBlueprint, StringBuilderBlueprint stringBuilderBlueprint, ListBlueprint listBlueprint)
        {
            codeStatementCollection.Add(returnBlueprint.ReturnExpress(templateBlueprint.ExecuteNonQuery(connection
                           , stringBuilderBlueprint.Field
                           , listBlueprint.Field)));
        }

        protected override void SelectReturn(string connection, CodeStatementCollection codeStatementCollection, ITemplateBlueprint templateBlueprint, ReturnBlueprint returnBlueprint, StringBuilderBlueprint stringBuilderBlueprint, ListBlueprint listBlueprint)
        {
            codeStatementCollection.Add(returnBlueprint.ReturnExpress(templateBlueprint.ExecuteDataTable(connection
                            , stringBuilderBlueprint.Field
                            , listBlueprint.Field)));
        }
    }
}
