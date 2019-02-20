using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.MethodEncapsulation
{
    public class MsSqlParamterQuery : IParamterQuery
    {
        ListBlueprint listBlueprint;
        public MsSqlParamterQuery(ListBlueprint listBlueprint)
        {
            this.listBlueprint = listBlueprint;
        }

        protected override void ExecuteDataTableNormalBuild(CodeStatementCollection codeStatementCollection)
        {
            SimpleExecuteDataTableNormalBuild(new MsSQLParmsBlueprint($"{fieldName}Par"), listBlueprint, codeStatementCollection);
        }

        protected override void ExecuteDataTableCircleBuild(CodeStatementCollection codeStatementCollection)
        {
            SimpleExecuteDataTableCircleBuild(new MsSQLParmsBlueprint($"{fieldName}Par"), listBlueprint, codeStatementCollection);
        }

        protected override void ValueTrue(CodeStatementCollection codeStatementCollection)
        {
            SimpleValueTrue(new MsSQLParmsBlueprint($"{fieldName}ParValue"), listBlueprint, codeStatementCollection);
        }

        protected override void ValueFalse(CodeStatementCollection codeStatementCollection)
        {
            SimpleValueFalse(new MsSQLParmsBlueprint($"{fieldName}ParValue"), listBlueprint, codeStatementCollection);
        }

        protected override void SetTrue(CodeStatementCollection codeStatementCollection)
        {
            SimpleSetTrue(new MsSQLParmsBlueprint($"{fieldName}ParSET"), listBlueprint, codeStatementCollection);
        }
    }
}
