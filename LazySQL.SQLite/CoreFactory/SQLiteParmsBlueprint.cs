using LazySQL.Core.Blueprint;
using LazySQL.Core.Tools;
using LazySQL.Extends;
using System.CodeDom;
using System.Data.SQLite;

namespace LazySQL.SQLite.CoreFactory
{
    public class SQLiteParmsBlueprint : IBlueprint, IParmsBlueprint
    {
        public SQLiteParmsBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create(string parName, string valueField)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationWithPar<SQLiteParameter>(Field, parName, valueField);
        }
    }
}
