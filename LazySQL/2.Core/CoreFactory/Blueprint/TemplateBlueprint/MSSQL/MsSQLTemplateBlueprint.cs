using LazySQL.Core.CoreFactory.Tools;
using LazySQL.Infrastructure;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
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
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<MSSQLTemplate>(Field);
        }

        public override CodeExpression ExecuteDataTable(string connectionString, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteDataTable", 
                new CodePrimitiveExpression(connectionString),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }

        public override CodeExpression ExecuteNonQuery(string connectionString, string commandTextField, string cmdParmsField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithMore(Field,
                "ExecuteNonQuery",
                new CodePrimitiveExpression(connectionString),
                new CodeVariableReferenceExpression(commandTextField),
                new CodeVariableReferenceExpression(cmdParmsField));
        }
    }
}
