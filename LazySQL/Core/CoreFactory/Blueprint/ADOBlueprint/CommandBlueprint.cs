using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class CommandBlueprint<T> : DisposeBlueprint where T : IDbCommand
    {
        public CommandBlueprint()
        {
            SetField("cmd");
        }

        public CommandBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create()
        {            
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<T>(Field);
        }

        public CodeStatement ConAssign(string connField)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithField($"{Field}.Connection", connField);
        }

        public CodeStatement TraAssign(string traField)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithField($"{Field}.Transaction", traField);
        }

        public CodeStatement CmdTextAssign(string strbField)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithField($"{Field}.CommandText", $"{strbField}.ToString()");
        }

        public CodeExpression CmdParAdd(string field)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore($"{Field}.Parameters", "Add", new CodeVariableReferenceExpression(field));
        }

        public CodeExpression CmdParAddWithValue(string field)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore($"{Field}.Parameters", "AddWithValue", new CodePrimitiveExpression($"@{field.Replace(".", string.Empty)}"), new CodeVariableReferenceExpression($"{field}.ToString()"));
        }

        public CodeExpression CmdParAddWithValue(string field,string value)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore($"{Field}.Parameters", "AddWithValue", new CodePrimitiveExpression($"@{field.Replace(".", string.Empty)}"), new CodePrimitiveExpression($"{value}"));
        }
    }
}
