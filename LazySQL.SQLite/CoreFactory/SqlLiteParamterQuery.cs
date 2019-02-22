using LazySQL.Extends;
using System.CodeDom;

namespace LazySQL.SQLite.CoreFactory
{
    public class SqlLiteParamterQuery : IParamterQuery
    {
        Core.Blueprint.SystemBlueprint.ListBlueprint listBlueprint;
        public SqlLiteParamterQuery(Core.Blueprint.SystemBlueprint.ListBlueprint listBlueprint)
        {
            this.listBlueprint = listBlueprint;
        }

        protected override void ExecuteDataTableCircleBuild(CodeStatementCollection codeStatementCollection)
        {
            SimpleExecuteDataTableCircleBuild(new SQLLiteParmsBlueprint($"{fieldName}Par"), listBlueprint, codeStatementCollection);
        }

        protected override void ExecuteDataTableNormalBuild(CodeStatementCollection codeStatementCollection)
        {
            SimpleExecuteDataTableNormalBuild(new SQLLiteParmsBlueprint($"{fieldName}Par"), listBlueprint, codeStatementCollection);
        }

        protected override void SetTrue(CodeStatementCollection codeStatementCollection)
        {
            SimpleSetTrue(new SQLLiteParmsBlueprint($"{fieldName}ParSET"), listBlueprint, codeStatementCollection);
        }

        protected override void ValueFalse(CodeStatementCollection codeStatementCollection)
        {
            SimpleValueFalse(new SQLLiteParmsBlueprint($"{fieldName}ParValue"), listBlueprint, codeStatementCollection);
        }

        protected override void ValueTrue(CodeStatementCollection codeStatementCollection)
        {
            SimpleValueTrue(new SQLLiteParmsBlueprint($"{fieldName}ParValue"), listBlueprint, codeStatementCollection);
        }
    }
}
