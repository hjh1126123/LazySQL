using System;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Tools.Modules
{
    public class SecurityTool
    {
        /// <summary>
        /// 创建一个Try
        /// </summary>
        /// <param name="tryCode"></param>
        /// <returns></returns>
        public CodeTryCatchFinallyStatement CreateTry(Func<CodeStatementCollection> tryCode)
        {
            CodeCatchClause @catch = new CodeCatchClause
            {
                CatchExceptionType = new CodeTypeReference(typeof(Exception)),
                LocalName = "ex"
            };
            @catch.Statements.Add(new CodeSnippetExpression("throw ex;"));

            CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
            codeTryCatchFinallyStatement.TryStatements.AddRange(tryCode());
            codeTryCatchFinallyStatement.CatchClauses.Add(@catch);

            return codeTryCatchFinallyStatement;
        }


        /// <summary>
        /// 创建一个try{}catch{}
        /// </summary>
        /// <param name="tryCode"></param>
        /// <param name="catchCode"></param>
        public CodeTryCatchFinallyStatement CreateTry(Func<CodeStatementCollection> tryCode, Func<CodeStatementCollection> catchCode)
        {            
            CodeCatchClause @catch = new CodeCatchClause
            {
                CatchExceptionType = new CodeTypeReference(typeof(Exception)),
                LocalName = "ex"
            };
            @catch.Statements.AddRange(catchCode());
            @catch.Statements.Add(new CodeSnippetExpression("throw ex;"));

            CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
            codeTryCatchFinallyStatement.TryStatements.AddRange(tryCode());
            codeTryCatchFinallyStatement.CatchClauses.Add(@catch);

            return codeTryCatchFinallyStatement;
        }

        /// <summary>
        /// 创建一个Try{}catch{}finally{}
        /// </summary>
        /// <param name="tryCode"></param>
        /// <param name="catchCode"></param>
        /// <returns></returns>
        public CodeTryCatchFinallyStatement CreateTry(Func<CodeStatementCollection> tryCode, Func<CodeStatementCollection> catchCode,Func<CodeStatementCollection> finallyCode)
        {
            CodeCatchClause @catch = new CodeCatchClause
            {
                CatchExceptionType = new CodeTypeReference(typeof(Exception)),
                LocalName = "ex"
            };
            @catch.Statements.AddRange(catchCode());
            @catch.Statements.Add(new CodeSnippetExpression("throw ex;"));

            CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
            codeTryCatchFinallyStatement.TryStatements.AddRange(tryCode());
            codeTryCatchFinallyStatement.CatchClauses.Add(@catch);
            codeTryCatchFinallyStatement.FinallyStatements.AddRange(finallyCode());

            return codeTryCatchFinallyStatement;
        }
    }
}
