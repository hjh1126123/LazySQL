using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public interface IParmsBlueprint
    {
        CodeExpression Create(string parName, string valueField);
    }
}
