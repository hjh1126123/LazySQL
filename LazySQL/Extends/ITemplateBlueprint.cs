using LazySQL.Core.Blueprint;
using System.CodeDom;

namespace LazySQL.Extends
{
    public abstract class ITemplateBlueprint : IBlueprint
    {
        public abstract CodeExpression Create();
        public abstract CodeExpression ExecuteNonQuery(string connectionString, string commandTextField, string cmdParmsField);
        public abstract CodeExpression ExecuteDataTable(string connectionString, string commandTextField, string cmdParmsField);
    }
}
