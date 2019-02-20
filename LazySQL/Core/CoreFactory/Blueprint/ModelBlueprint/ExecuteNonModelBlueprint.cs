using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class ExecuteNonModelBlueprint : IBlueprint
    {
        public ExecuteNonModelBlueprint()
        {
            SetField("ExecuteModel");
        }

        public ExecuteNonModelBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<ExecuteNonModel>(Field);
        }

        public CodeStatement AssignMsg(string @value)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithValue($"{Field}.Message", @value);
        }

        public CodeStatement AssignSuccess(bool @value)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithValue($"{Field}.Success", @value);
        }

        public CodeStatement AssignMsg(int @value)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithValue($"{Field}.Result", @value);
        }
    }
}
