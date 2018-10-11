using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data.SQLite;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class SQLLiteParmsBlueprint : IBlueprint
    {
        public SQLLiteParmsBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create(string parName, string valueField)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationWithPar<SQLiteParameter>(Field, parName, valueField);
        }
    }
}
