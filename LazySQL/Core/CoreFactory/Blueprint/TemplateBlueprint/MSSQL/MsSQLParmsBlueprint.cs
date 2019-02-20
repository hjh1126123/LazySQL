using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

using System.Data.SqlClient;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class MsSQLParmsBlueprint : IBlueprint, IParmsBlueprint
    {
        public MsSQLParmsBlueprint(string field)
        {
            SetField(field);
        }

        public CodeExpression Create(string parName, string valueField)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationWithPar<SqlParameter>(Field, parName, valueField);
        }
    }
}
