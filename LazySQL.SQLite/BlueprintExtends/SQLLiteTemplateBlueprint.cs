using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.SQLite.BlueprintExtends
{
    public class SQLLiteTemplateBlueprint : ITemplateBlueprint
    {
        public SQLLiteTemplateBlueprint()
        {
            SetField("sqlLiteT");
        }

        public SQLLiteTemplateBlueprint(string field)
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
