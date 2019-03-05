using LazySQL.Extends;
using System.CodeDom;

namespace LazySQL.SQLite.CoreFactory
{
    public class SQLiteParamterQuery : IParamterQuery
    {
        Core.Blueprint.SystemBlueprint.ListBlueprint listBlueprint;
        public SQLiteParamterQuery(Core.Blueprint.SystemBlueprint.ListBlueprint listBlueprint)
        {
            this.listBlueprint = listBlueprint;
        }

        protected override void ExecuteDataTableCircleBuild(CodeStatementCollection codeStatementCollection)
        {
            SimpleExecuteDataTableCircleBuild(new SQLiteParmsBlueprint($"{fieldName}Par"), listBlueprint, codeStatementCollection);
        }

        protected override void ExecuteDataTableNormalBuild(CodeStatementCollection codeStatementCollection)
        {
            SimpleExecuteDataTableNormalBuild(new SQLiteParmsBlueprint($"{fieldName}Par"), listBlueprint, codeStatementCollection);
        }

        protected override void SetTrue(CodeStatementCollection codeStatementCollection)
        {
            SimpleSetTrue(new SQLiteParmsBlueprint($"{fieldName}ParSET"), listBlueprint, codeStatementCollection);
        }

        protected override void ValueFalse(CodeStatementCollection codeStatementCollection)
        {
            SimpleValueFalse(new SQLiteParmsBlueprint($"{fieldName}ParValue"), listBlueprint, codeStatementCollection);
        }

        protected override void ValueTrue(CodeStatementCollection codeStatementCollection)
        {
            SimpleValueTrue(new SQLiteParmsBlueprint($"{fieldName}ParValue"), listBlueprint, codeStatementCollection);
        }
    }
}
