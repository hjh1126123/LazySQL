using System.CodeDom;

namespace LazySQL.Extends
{
    public interface IParmsBlueprint
    {
        CodeExpression Create(string parName, string valueField);
    }
}
