using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class ReturnBlueprint : IBlueprint
    {
        public CodeStatement ReturnField(string dataSetField)
        {
            return ToolManager.Instance.ReturnTool.ReturnField(dataSetField);
        }

        public CodeStatement ReturnTrue()
        {            
            return ToolManager.Instance.ReturnTool.ReturnField("true");
        }

        public CodeStatement ReturnFalse()
        {
            return ToolManager.Instance.ReturnTool.ReturnField("false");
        }

        public CodeStatement ReturnDataTable(string dataSetField)
        {
            return ToolManager.Instance.ReturnTool.ReturnField($"{dataSetField}.Tables[0]");
        }

        public CodeStatement ReturnDataTable(string dataSetField, int dataTableCount)
        {
            return ToolManager.Instance.ReturnTool.ReturnField($"{dataSetField}.Tables[{dataTableCount}]");
        }

        public CodeStatement ReturnExpress(CodeExpression codeExpression)
        {
            return ToolManager.Instance.ReturnTool.ReturnExpress(codeExpression);
        }
    }
}
