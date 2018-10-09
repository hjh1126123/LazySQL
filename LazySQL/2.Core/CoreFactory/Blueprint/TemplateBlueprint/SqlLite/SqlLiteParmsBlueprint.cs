using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data.SQLite;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class SqlLiteParmsBlueprint : IBlueprint
    {
        public SqlLiteParmsBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create(string parName, string valueField)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationWithPar<SQLiteParameter>(Field, parName, valueField);
        }
    }
}
