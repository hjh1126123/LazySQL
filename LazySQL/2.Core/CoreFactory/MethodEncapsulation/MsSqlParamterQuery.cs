using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data.SqlClient;

namespace LazySQL.Core.CoreFactory.MethodEncapsulation
{
    public class MsSqlParamterQuery : IParamterQuery
    {
        public CommandBlueprint<SqlCommand> commandBlueprint;
        public MsSqlParamterQuery(CommandBlueprint<SqlCommand> commandBlueprint)
        {
            this.commandBlueprint = commandBlueprint;
        }

        protected override void normalBuild(CodeStatementCollection codeStatementCollection)
        {
            codeStatementCollection.Add(commandBlueprint.CmdParAddWithValue(fieldName));
        }

        protected override void CircleBuild(CodeStatementCollection codeStatementCollection)
        {
            codeStatementCollection.Add(stringBuilderBlueprint.Append($"@{fieldName}"));
            codeStatementCollection.Add(stringBuilderBlueprint.AppendField("i"));
            codeStatementCollection.Add(ToolManager.Instance.ConditionTool.CreateConditionCode($"i != ({fieldName}List.Count - 1)", () =>
            {
                CodeStatementCollection codeStatementCollectionTmpIF = new CodeStatementCollection();
                codeStatementCollectionTmpIF.Add(stringBuilderBlueprint.Append(","));
                return codeStatementCollectionTmpIF;
            }));

            ParameterBlueprint<SqlParameter> parameterBlueprint = new ParameterBlueprint<SqlParameter>($"{fieldName}Par");
            codeStatementCollection.Add(parameterBlueprint.Create($"\"@{fieldName}\" + i"));
            codeStatementCollection.Add(parameterBlueprint.AssignmentObj($"{fieldName}List[i]"));
            codeStatementCollection.Add(commandBlueprint.CmdParAdd(parameterBlueprint.Field));
        }
    }
}
