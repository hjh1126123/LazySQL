using LazySQL.Core.CoreFactory.Tools;
using LazySQL.Infrastructure;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class SQLiteTemplateBlueprint : ITemplateBlueprint
    {
        public SQLiteTemplateBlueprint()
        {
            SetField("sqlLiteT");
        }

        public SQLiteTemplateBlueprint(string field)
        {
            SetField(field);
        }

        public override CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateObj_Instance_Static<SQLiteTemplate>(Field);
        }

        public override CodeExpression ExecuteNonQuery(string conn, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteNonQuery",
                new CodePrimitiveExpression(conn),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }

        public override CodeExpression ExecuteDataTable(string conn, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteDataTable",
                new CodePrimitiveExpression(conn),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }
    }
}
