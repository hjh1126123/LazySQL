using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public abstract class ITemplateBlueprint : IBlueprint
    {
        public abstract CodeExpression Create();
        public abstract CodeExpression ExecuteNonQuery(string connectionString, string commandTextField, string cmdParmsField);
        public abstract CodeExpression ExecuteDataTable(string connectionString, string commandTextField, string cmdParmsField);
    }
}
