using LazySQL.Core.Tools;
using System.CodeDom;
using System.Data;

namespace LazySQL.Core.Blueprint.ADOBlueprint
{
    public class ConnectionBlueprint<T> : DisposeBlueprint where T : IDbConnection
    {
        string ConnectionText;
        public ConnectionBlueprint(string ConnectionText)
        {
            SetField("conn");
            this.ConnectionText = ConnectionText;
        }

        public ConnectionBlueprint(string FieldName, string ConnectionText)
        {
            SetField(FieldName);
            this.ConnectionText = ConnectionText;
        }

        public CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationHaveValue<T>(Field, $"\"{ConnectionText}\"");
        }

        public CodeExpression Open()
        {
            return ToolManager.Instance.InvokeTool.Invoke(Field, "Open");
        }
    }
}
