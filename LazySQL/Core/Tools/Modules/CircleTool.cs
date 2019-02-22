using System;
using System.CodeDom;

namespace LazySQL.Core.Tools.Modules
{
    public class CircleTool
    {
        public CodeIterationStatement CreateCircle(string initStatement,string testExpression,string incrementStatement,Func<CodeStatementCollection> func)
        {
            CodeIterationStatement ret = new CodeIterationStatement
            {
                InitStatement = new CodeSnippetStatement(initStatement),
                TestExpression = new CodeSnippetExpression(testExpression),
                IncrementStatement = new CodeSnippetStatement(incrementStatement)
            };
            ret.Statements.AddRange(func());

            return ret;
        }
    }
}
