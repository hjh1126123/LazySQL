using LazySQL.Core.Tools;
using LazySQL.Extends;
using System.CodeDom;

namespace LazySQL.Core.Blueprint
{
    public abstract class DisposeBlueprint : IBlueprint
    {
        public CodeExpression Dispose()
        {
            return ToolManager.Instance.InvokeTool.Invoke(Field, "Dispose");
        }
    }
}
