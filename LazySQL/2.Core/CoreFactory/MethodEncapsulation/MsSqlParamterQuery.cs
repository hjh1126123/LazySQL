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
            MsSQLParmsBlueprint parameterBlueprint = new MsSQLParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void ExecuteDataTableCircleBuild(CodeStatementCollection codeStatementCollection)
        {
            codeStatementCollection.Add(stringBuilderBlueprint.Append($"@{fieldName}"));
            codeStatementCollection.Add(stringBuilderBlueprint.AppendField("i"));
            codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"i != ({fieldName}List.Count - 1)", () =>
            {
                CodeStatementCollection codeStatementCollectionTmpIF = new CodeStatementCollection();
                codeStatementCollectionTmpIF.Add(stringBuilderBlueprint.Append(","));
                return codeStatementCollectionTmpIF;
            }));

            MsSQLParmsBlueprint parameterBlueprint = new MsSQLParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\" + i", $"{fieldName}List[i]"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void ValueTrue(CodeStatementCollection codeStatementCollection)
        {
            MsSQLParmsBlueprint parameterBlueprint = new MsSQLParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void ValueFalse(CodeStatementCollection codeStatementCollection)
        {
            MsSQLParmsBlueprint parameterBlueprint = new MsSQLParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", "\"''\""));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void SetTrue(CodeStatementCollection codeStatementCollection)
        {
            stringBuilderBlueprint.Append($"{fieldName} = @{fieldName}ParSET");
            MsSQLParmsBlueprint parameterBlueprint = new MsSQLParmsBlueprint($"{fieldName}ParSET");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}ParSET\"", $"{fieldName}ParSET"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }
    }
}
