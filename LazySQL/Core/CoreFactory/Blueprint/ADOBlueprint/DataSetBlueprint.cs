using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class DataSetBlueprint<T> : DisposeBlueprint where T : MarshalByValueComponent, IListSource, IXmlSerializable, ISupportInitializeNotification, ISupportInitialize, ISerializable
    {
        public DataSetBlueprint()
        {
            SetField("ds");
        }

        public DataSetBlueprint(string fieldName)
        {
            SetField(fieldName);
        }

        public CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<T>(Field);
        }

        public CodeStatement TableAssign()
        {
            return ToolManager.Instance.ReturnTool.ReturnField($"{Field}.Tables[0]");
        }
    }
}
