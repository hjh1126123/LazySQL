using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Tools.Modules
{
    public class AssignmentTool
    {
        /// <summary>
        /// 赋值字段
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="assignmentValue">赋值 值</param>
        /// <returns></returns>
        public CodeAssignStatement AssignmentWithValue(string fieldName, object assignmentValue)
        {
            return new CodeAssignStatement
            {
                Left = new CodeVariableReferenceExpression
                {
                    VariableName = fieldName
                },
                Right = new CodePrimitiveExpression(assignmentValue)
            };
        }

        /// <summary>
        /// 赋值字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="assignmentValue"></param>
        /// <returns></returns>
        public CodeAssignStatement AssignmentWithField(string fieldNameLeft, string fieldNameRight)
        {
            return new CodeAssignStatement
            {
                Left = new CodeVariableReferenceExpression
                {
                    VariableName = fieldNameLeft
                },
                Right = new CodeVariableReferenceExpression
                {
                    VariableName = fieldNameRight
                }
            };
        }
    }
}
