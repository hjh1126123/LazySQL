using System;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Tools.Modules
{
    public class ConditionTool
    {
        public CodeConditionStatement CreateConditionCode(string condition, Func<CodeStatementCollection> trueFs)
        {
            CodeConditionStatement codeConditionStatement = new CodeConditionStatement
            {
                Condition = new CodeVariableReferenceExpression(condition)
            };
            if (trueFs != null)
            {
                codeConditionStatement.TrueStatements.AddRange(trueFs());
            }
            return codeConditionStatement;
        }

        public CodeConditionStatement CreateConditionCode(string condition, Func<CodeStatementCollection> trueFs, Func<CodeStatementCollection> falseFs)
        {
            CodeConditionStatement codeConditionStatement = new CodeConditionStatement
            {
                Condition = new CodeVariableReferenceExpression(condition)
            };
            if (trueFs != null)
            {
                codeConditionStatement.TrueStatements.AddRange(trueFs());
            }
            if (falseFs != null)
            {
                codeConditionStatement.FalseStatements.AddRange(falseFs());
            }
            return codeConditionStatement;
        }
    }
}
