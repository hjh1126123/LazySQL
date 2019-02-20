using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
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
