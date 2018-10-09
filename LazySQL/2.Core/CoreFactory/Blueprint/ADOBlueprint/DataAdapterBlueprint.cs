using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class DataAdapterBlueprint<T> : DisposeBlueprint where T : IDataAdapter
    {
        public DataAdapterBlueprint()
        {
            SetField("da");
        }

        public DataAdapterBlueprint(string FieldName)
        {
            SetField(FieldName);
        }


        public CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<T>(Field);
        }

        public CodeStatement CmdAssign(string cmdField)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithField($"{Field}.SelectCommand", cmdField);
        }

        public CodeExpression DsAssign(string dsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithObj(Field, "Fill", dsField);
        }
    }
}
