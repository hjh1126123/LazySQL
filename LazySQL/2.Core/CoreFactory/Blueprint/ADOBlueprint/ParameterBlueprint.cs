using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Data;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class ParameterBlueprint<T> : IBlueprint where T : IDbDataParameter
    {
        public ParameterBlueprint()
        {
            SetField("DBParm");
        }

        public ParameterBlueprint(string Field)
        {
            SetField(Field);
        }

        public CodeExpression Create(string parName)
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiationWithPar<T>(Field, parName, $"{typeof(SqlDbType).FullName}.NVarChar");
        }

        public CodeStatement AssignmentObj(string value)
        {
            return ToolManager.Instance.AssignmentTool.AssignmentWithField($"{Field}.Value", value);
        }
    }
}
