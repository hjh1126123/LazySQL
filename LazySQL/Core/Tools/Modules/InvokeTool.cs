using System.CodeDom;
using System.Collections.Generic;

namespace LazySQL.Core.Tools.Modules
{
    public class InvokeTool
    {
        public CodeMethodInvokeExpression Invoke(string reference, string method)
        {
            return WriteMethod(reference, method);
        }

        public CodeMethodInvokeExpression InvokeWithValue(string reference, string method, object value)
        {
            return WriteMethod(reference, method, new CodePrimitiveExpression(value));
        }

        public CodeMethodInvokeExpression InvokeWithObj(string reference, string method, string obj)
        {
            return WriteMethod(reference, method, new CodeVariableReferenceExpression(obj));
        }

        public CodeMethodInvokeExpression InvokeWithMore(string reference, string method, params object[] codeFields)
        {
            List<CodeExpression> tempCodeExpressions = new List<CodeExpression>();
            foreach (var field in codeFields)
            {
                if (field is CodeVariableReferenceExpression codeVariableReferenceExpression)
                {
                    tempCodeExpressions.Add(codeVariableReferenceExpression);
                    continue;
                }
                if (field is CodePrimitiveExpression codePrimitiveExpression)
                {
                    tempCodeExpressions.Add(codePrimitiveExpression);
                    continue;
                }
            }
            return WriteMethod(reference, method, tempCodeExpressions.ToArray());
        }        
        
        private CodeMethodInvokeExpression WriteMethod(string filed, string methodName)
        {
            return new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = new CodeVariableReferenceExpression(filed),
                    MethodName = methodName
                }
            };
        }
        private CodeMethodInvokeExpression WriteMethod(string filed, string methodName, CodeExpression codeExpression)
        {
            CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = new CodeVariableReferenceExpression(filed),
                    MethodName = methodName                  
                }
            };
            codeMethodInvokeExpression.Parameters.Add(codeExpression);
            return codeMethodInvokeExpression;
        }
        private CodeMethodInvokeExpression WriteMethod(string filed, string methodName, CodeExpression[] codeExpressions)
        {
            CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = new CodeVariableReferenceExpression(filed),
                    MethodName = methodName
                }
            };
            foreach (var codeExpression in codeExpressions)
                codeMethodInvokeExpression.Parameters.Add(codeExpression);

            return codeMethodInvokeExpression;
        }
    }
}
