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
            , Type type
            , string query)
        {
            ReferencedAssemblies.Add("LazySQL.dll");
            ReferencedAssemblies.Add("System.Data.SQLite.dll");

            CodeStatementCollection codeStatementCollection = new CodeStatementCollection();

            #region 添加所需零件

            StringBuilderBlueprint stringBuilderBlueprint = new StringBuilderBlueprint();

            SQLiteTemplateBlueprint sQLiteTemplateBlueprint = new SQLiteTemplateBlueprint();

            #endregion

            codeStatementCollection.Add(stringBuilderBlueprint.Create());

            codeStatementCollection.Add(sQLiteTemplateBlueprint.Create());

            TryCatchFinallyBlueprint tryCatchFinallyBlueprint = new TryCatchFinallyBlueprint();

            codeStatementCollection.Add(tryCatchFinallyBlueprint.Create(() =>
            {
                CodeStatementCollection tryCodeStatementCollection = new CodeStatementCollection();

                ListBlueprint listBlueprint = new ListBlueprint();

                tryCodeStatementCollection.Add(listBlueprint.Create<SQLiteParameter>());

                Building(isNotCondition
                    , parameters
                    , tryCodeStatementCollection
                    , new SqlLiteParamterQuery(listBlueprint)
                    , type
                    , sQLs
                    , stringBuilderBlueprint
                    , query
                    , connection
                    , sQLiteTemplateBlueprint
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
