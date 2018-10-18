using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.MethodEncapsulation
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
            codeStatementCollection.Add(stringBuilderBlueprint.Append($"@{fieldName}"));
            codeStatementCollection.Add(stringBuilderBlueprint.AppendField("i"));
            codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"i != ({fieldName}List.Count - 1)", () =>
            {
                CodeStatementCollection codeStatementCollectionTmpIF = new CodeStatementCollection();
                codeStatementCollectionTmpIF.Add(stringBuilderBlueprint.Append(","));
                return codeStatementCollectionTmpIF;
            }));

            SQLLiteParmsBlueprint parameterBlueprint = new SQLLiteParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\" + i", $"{fieldName}List[i]"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void ExecuteDataTableNormalBuild(CodeStatementCollection codeStatementCollection)
        {
            SQLLiteParmsBlueprint parameterBlueprint = new SQLLiteParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void SetTrue(CodeStatementCollection codeStatementCollection)
        {
            stringBuilderBlueprint.Append($"{fieldName} = @{fieldName}ParSET");
            SQLLiteParmsBlueprint parameterBlueprint = new SQLLiteParmsBlueprint($"{fieldName}ParSET");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}ParSET\"", $"{fieldName}ParSET"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void ValueFalse(CodeStatementCollection codeStatementCollection)
        {
            SQLLiteParmsBlueprint parameterBlueprint = new SQLLiteParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", "\"''\""));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }

        protected override void ValueTrue(CodeStatementCollection codeStatementCollection)
        {
            SQLLiteParmsBlueprint parameterBlueprint = new SQLLiteParmsBlueprint($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\"", $"{fieldName}"));
            codeStatementCollection.Add(listBlueprint.Add(parameterBlueprint.Field));
        }
    }
}
