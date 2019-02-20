using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Tools.Modules
{
    public class InitializeTool
    {
        public CodeExpression CreateObj<T>(string fieldName)
        {
            return WriteCode($"{typeof(T).FullName} {fieldName}");
        }
        public CodeExpression CreateObj_Instance_Static<T>(string fieldName)
        {
            return WriteCode($"{typeof(T).FullName} {fieldName} = {typeof(T).FullName}.Instance");
        }
        public CodeExpression CreateAndInstantiation<T>(string fieldName)
        {
            return WriteCode($"{typeof(T).FullName} {fieldName} = new {typeof(T).FullName}()");
        }
        public CodeExpression CreateAndInstantiation<T>(string fieldName, string value)
        {
            return WriteCode($"{typeof(T).FullName} {fieldName} = {value}");
        }
        public CodeExpression CreateAndInstantiationHaveValue<T>(string fieldName, string value)
        {
            return WriteCode($"{typeof(T).FullName} {fieldName} = new {typeof(T).FullName}({value})");
        }
        public CodeExpression CreateAndInstantiationHaveValueInList<T>(string fieldName)
        {
            return WriteCode($"System.Collections.Generic.List<{typeof(T).FullName}> {fieldName} = new System.Collections.Generic.List<{typeof(T).FullName}>()");
        }
        public CodeExpression CreateAndInstantiationHaveValueInList<T>(string fieldName, string value)
        {
            return WriteCode($"System.Collections.Generic.List<{typeof(T).FullName}> {fieldName} = new System.Collections.Generic.List<{typeof(T).FullName}>({value})");
        }
        public CodeExpression CreateAndInstantiationWithPar<T>(string fieldName, params string[] pars)
        {
            string tmpPars = string.Empty;
            for (var i = 0; i < pars.Length; i++)
            {
                tmpPars += pars[i];
                if (i < pars.Length - 1)
                {
                    tmpPars += ",";
                }
            }
            return WriteCode($"{typeof(T).FullName} {fieldName} = new {typeof(T).FullName}({tmpPars})");
        }

        private CodeSnippetExpression WriteCode(string code)
        {
            return new CodeSnippetExpression
            {
                Value = code
            };
        }
    }}