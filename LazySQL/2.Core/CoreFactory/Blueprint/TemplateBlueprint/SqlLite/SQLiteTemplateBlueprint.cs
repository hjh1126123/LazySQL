using LazySQL.Core.CoreFactory.Tools;
using LazySQL.Infrastructure;
using System.CodeDom;
using System.Data;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class SQLiteTemplateBlueprint : IBlueprint
    {
        public SQLiteTemplateBlueprint()
        {
            SetField("sqlLite");
        }

        public SQLiteTemplateBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<SQLiteTemplate>(Field);
        }

        public CodeExpression ExecuteNonQuery(string connectionString, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteNonQuery",
                new CodePrimitiveExpression(connectionString),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }

        public CodeExpression ExecuteDataTable(string connectionString, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteDataTable",
                new CodePrimitiveExpression(connectionString),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }
    }
}
