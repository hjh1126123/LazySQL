using LazySQL.Core.Tools;
using System.CodeDom;
using System.Data;

namespace LazySQL.Core.Blueprint.ADOBlueprint
{
    public class TransactionBlueprint<T> : DisposeBlueprint where T : IDbTransaction
    {
        public TransactionBlueprint()
        {
            SetField("ctra");
        }

        public TransactionBlueprint(string Field)
        {
            SetField(Field);
        }

        public CodeExpression Create(string connField)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<T>(Field, $"{connField}.BeginTransaction()");
        }

        public CodeExpression Commit()
        {
            return ToolManager.Instance.InvokeTool.Invoke(Field, "Commit");
        }

        public CodeExpression Rollback()
        {
            return ToolManager.Instance.InvokeTool.Invoke(Field, "Rollback");
        }
    }
}
