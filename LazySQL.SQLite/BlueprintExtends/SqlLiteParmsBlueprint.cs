using LazySQL.Core.CoreFactory.Blueprint;
using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data.SQLite;

namespace LazySQL.SQLite.BlueprintExtends
{
    public class SQLLiteParmsBlueprint : IBlueprint, IParmsBlueprint
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
