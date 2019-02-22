using LazySQL.Core.Tools;
using LazySQL.Extends;
using System.CodeDom;

namespace LazySQL.Core.Blueprint.SystemBlueprint
{
    public class ListBlueprint : IBlueprint
    {
        public ListBlueprint()
        {
            SetField("aList");
        }

        public ListBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create<T>()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationHaveValueInList<T>(Field);
        }

        public CodeExpression Create<T>(string value)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationHaveValueInList<T>(Field, value);
        }

        public CodeExpression Add(string field)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithObj(Field, "Add", field);
        }
    }
}
