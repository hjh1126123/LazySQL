using System.CodeDom;

namespace LazySQL.Core.Tools.Modules
{
    public class ReturnTool
    {
        public CodeMethodReturnStatement ReturnField(string returnStr)
        {
            return new CodeMethodReturnStatement
            {
                Expression = new CodeSnippetExpression(returnStr)
            };
        }

        public CodeMethodReturnStatement ReturnExpress(CodeExpression codeExpression)
        {
            return new CodeMethodReturnStatement
            {
                Expression = codeExpression
            };
        }
    }
}
