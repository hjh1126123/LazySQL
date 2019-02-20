using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.MethodEncapsulation;
using LazySQL.SQLite.BlueprintExtends;
using System.CodeDom;

namespace LazySQL.SQLite.MethodEncapsulationExtends
{
    public class SqlLiteParamterQuery : IParamterQuery
    {
        ListBlueprint listBlueprint;
        public SqlLiteParamterQuery(ListBlueprint listBlueprint)
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
