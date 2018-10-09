using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public abstract class DisposeBlueprint : IBlueprint
    {
        public CodeExpression Dispose()
        {
            return ToolManager.Instance.InvokeTool.Invoke(Field, "Dispose");
        }
    }
}
