using LazySQL.Core.Tools;
using LazySQL.Extends;
using System.CodeDom;

namespace LazySQL.MSSQL.CoreFactory
{
    public class MsSQLTemplateBlueprint : ITemplateBlueprint
    {
        public MsSQLTemplateBlueprint()
        {
            SetField("msSqlT");
        }

        public MsSQLTemplateBlueprint(string field)
        {
            SetField(field);
        }

        public override CodeExpression Create()
        {            
            return ToolManager.Instance.InitializeTool.CreateObj_Instance_Static<MSSQLTemplate>(Field);
        }

        public override CodeExpression ExecuteDataTable(string conn, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteDataTable",
                new CodePrimitiveExpression(conn),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }

        public override CodeExpression ExecuteNonQuery(string conn, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteNonQuery",
                new CodePrimitiveExpression(conn),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }
    }
}
