using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace LazySQL.Infrastructure
{
    public class CodedomHelper
    {
        private static CodedomHelper _instance;
        public static CodedomHelper GetInstance()
        {
            if (_instance == null)
                _instance = new CodedomHelper();

            return _instance;
        }

        #region 属性

        CodeDomProvider provider;
        Dictionary<string, CompilerParameters> compilerParameterArray;

        #endregion

        private CodedomHelper()
        {
            provider = CodeDomProvider.CreateProvider("CSharp");
            compilerParameterArray = new Dictionary<string, CompilerParameters>();
        }

        #region Write

        public void AddCompilerParameters(string name)
        {
            if (!compilerParameterArray.ContainsKey(name))
            {
                CompilerParameters compilerParameters = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                };                
                compilerParameterArray.Add(name, compilerParameters);
            }
        }

        /// <summary>
        /// 添加自动生成类的引用
        /// </summary>
        public void AddReferencedAssemblies(string name, string ReferencedAssemblie)
        {
            if (compilerParameterArray.ContainsKey(name))
            {
                if (!compilerParameterArray[name].ReferencedAssemblies.Contains(ReferencedAssemblie))
                    compilerParameterArray[name].ReferencedAssemblies.Add(ReferencedAssemblie);
            }
        }

        /// <summary>
        /// 删除自动生成类的引用
        /// </summary>
        /// <param name="ReferencedAssemblie"></param>
        public void RemoveReferencedAssemblies(string name,string ReferencedAssemblie)
        {
            if (compilerParameterArray.ContainsKey(name))
            {
                if (compilerParameterArray[name].ReferencedAssemblies.Contains(ReferencedAssemblie))
                    compilerParameterArray[name].ReferencedAssemblies.Remove(ReferencedAssemblie);
            }
        }

        /// <summary>
        /// 创建代码容器
        /// </summary>
        /// <returns></returns>
        public CodeCompileUnit BuildCodeCompileUnit()
        {
            return new CodeCompileUnit();
        }

        /// <summary>
        /// 创建命名空间
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public CodeNamespace BuildNameSpace(string nameSpace)
        {
            return new CodeNamespace(nameSpace);
        }

        /// <summary>
        /// 创建类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public CodeTypeDeclaration BuildCodeTypeDeclaration(string className)
        {
            return new CodeTypeDeclaration(className)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public
            };
        }

        /// <summary>
        /// 在自动生成类里面创建字段
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="proType"></param>
        /// <param name="memberAttributes"></param>
        /// <param name="class"></param>
        /// <param name="fieldsNote"></param>
        public void AddFields(string proName, Type proType, MemberAttributes memberAttributes, CodeTypeDeclaration @class, string fieldsNote = "无备注")
        {
            CodeMemberField codeMemberField = new CodeMemberField
            {
                Attributes = memberAttributes,
                Name = proName,
                Type = new CodeTypeReference(proType)
            };
            codeMemberField.Comments.Add(new CodeCommentStatement($"这是自动生成的备注[{fieldsNote}]"));
            @class.Members.Add(codeMemberField);
        }

        /// <summary>
        /// 在自动生成类里面创建构造字段
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="proType"></param>
        /// <param name="class"></param>
        /// <param name="fieldsNote"></param>
        public void AddProperties(string proName, Type proType, CodeTypeDeclaration @class, string fieldsNote = "无备注")
        {
            CodeMemberProperty codeMemberProperty = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = proName,
                HasGet = true,
                Type = new CodeTypeReference(proType)
            };
            codeMemberProperty.Comments.Add(new CodeCommentStatement($"这是自动生成的备注[{fieldsNote}]"));
            codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(            
                new CodeThisReferenceExpression(),proName)));
            @class.Members.Add(codeMemberProperty);
        }

        /// <summary>
        /// 在自动生成类里面创建构造字段
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="proType"></param>
        /// <param name="codeExpression"></param>
        /// <param name="class"></param>
        /// <param name="fieldsNote"></param>
        public void AddProperties(string proName, Type proType, CodeExpression codeExpression, CodeTypeDeclaration @class, string fieldsNote = "无备注")
        {
            CodeMemberProperty codeMemberProperty = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = proName,
                HasGet = true,
                Type = new CodeTypeReference(proType)
            };

            codeMemberProperty.Comments.Add(new CodeCommentStatement(
                $"这是自动生成的备注[{fieldsNote}]"));

            codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(codeExpression));

            @class.Members.Add(codeMemberProperty);
        }


        /// <summary>
        /// 在自动类中添加方法
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="returnType">返回类型</param>
        /// <param name="codeParameterDeclarationExpressions">参数组</param>
        /// <param name="codeStatementCollection">代码组</param>
        /// <param name="@class">自动类</param>
        public void AddMethod(string methodName, Type returnType, CodeParameterDeclarationExpression[] codeParameterDeclarationExpressions, CodeStatementCollection codeStatementCollection, CodeTypeDeclaration @class)
        {
            CodeMemberMethod codeMemberMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            if (!string.IsNullOrWhiteSpace(methodName))
            {
                codeMemberMethod.Name = methodName;
            }
            else
            {
                codeMemberMethod.Name = "notMethodName";
            }
            if (returnType != null)
            {
                codeMemberMethod.ReturnType = new CodeTypeReference(returnType);
            }
            if (codeParameterDeclarationExpressions != null && codeParameterDeclarationExpressions.Length > 0)
            {
                foreach (var codeParameterDeclarationExpression in codeParameterDeclarationExpressions)
                    codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);                
            }            
            if(codeStatementCollection != null)
            {
                codeMemberMethod.Statements.AddRange(codeStatementCollection);
            }
            else
            {
                throw new Exception("代码段为空或者有同名方法");
            }
            if (!@class.Members.Contains(codeMemberMethod))
                @class.Members.Add(codeMemberMethod);
        }

        /// <summary>
        /// 在自动类中添加方法
        /// </summary>
        /// <param name="memberAttributes">方法特性</param>
        /// <param name="methodName">方法名</param>
        /// <param name="returnType">返回类型</param>
        /// <param name="codeParameterDeclarationExpressions">参数组</param>
        /// <param name="codeStatementCollection">代码组</param>
        /// <param name="@class">自动类</param>
        public void AddMethod(MemberAttributes memberAttributes,string methodName, Type returnType, CodeParameterDeclarationExpression[] codeParameterDeclarationExpressions, CodeStatementCollection codeStatementCollection, CodeTypeDeclaration @class)
        {
            CodeMemberMethod Method = new CodeMemberMethod
            {
                Attributes = memberAttributes
            };

            if (!string.IsNullOrWhiteSpace(methodName))
                Method.Name = methodName;
            else
                Method.Name = "notMethodName";

            if (returnType != null)
                Method.ReturnType = new CodeTypeReference(returnType);

            if (codeParameterDeclarationExpressions != null && codeParameterDeclarationExpressions.Length > 0)
                foreach (var codeParameterDeclarationExpression in codeParameterDeclarationExpressions)
                    Method.Parameters.Add(codeParameterDeclarationExpression);

            if (codeStatementCollection != null)
                Method.Statements.AddRange(codeStatementCollection);
            else
                throw new Exception("代码段为空或者有同名方法");

            if (!@class.Members.Contains(Method))
            {
                @class.Members.Add(Method);
            }
        }

        /// <summary>
        /// 在自动生成类写入引用方法
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public CodeMethodInvokeExpression WriteMethod(CodeExpression filed, string methodName)
        {
            CodeMethodInvokeExpression _codeMethodInvokeExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = filed,
                    MethodName = methodName
                }
            };
            return _codeMethodInvokeExpression;
        }

        /// <summary>
        /// 在自动生成类写入引用方法
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="methodName"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public CodeMethodInvokeExpression WriteMethod(CodeExpression filed, string methodName, CodeExpression[] codeExpressions)
        {
            CodeMethodInvokeExpression _codeMethodInvokeExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = filed,
                    MethodName = methodName
                }
            };
            foreach (var _codeExpression in codeExpressions)
                _codeMethodInvokeExpression.Parameters.Add(_codeExpression);
            return _codeMethodInvokeExpression;
        }

        /// <summary>
        /// 在自动生成类写入方法
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="methodName"></param>
        /// <param name="codeExpressions"></param>
        /// <returns></returns>
        public CodeMethodInvokeExpression WriteMethod(string filed, string methodName, CodeExpression[] codeExpressions)
        {
            CodeMethodInvokeExpression _codeMethodInvokeExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = new CodeVariableReferenceExpression(filed),
                    MethodName = methodName
                }
            };
            foreach (var _codeExpression in codeExpressions)
                _codeMethodInvokeExpression.Parameters.Add(_codeExpression);
            return _codeMethodInvokeExpression;
        }


        /// <summary>
        /// 在自动生产类写入引用方法
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="methodName"></param>
        /// <param name="codeExpression"></param>
        /// <returns></returns>
        public CodeMethodInvokeExpression WriteMethod(CodeExpression filed, string methodName, CodeExpression codeExpression)
        {
            CodeMethodInvokeExpression _codeMethodInvokeExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    TargetObject = filed,
                    MethodName = methodName
                }
            };
            _codeMethodInvokeExpression.Parameters.Add(codeExpression);
            return _codeMethodInvokeExpression;
        }


        /// <summary>
        /// 在自动生成类里面写入赋值
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="codeExpression">表达式</param>
        public CodeAssignStatement CodeFieldAssignment(string fieldName, CodeExpression codeExpression)
        {
            CodeVariableReferenceExpression left = new CodeVariableReferenceExpression
            {
                VariableName = fieldName
            };
            CodeAssignStatement codeAssignStatement = new CodeAssignStatement
            {
                Left = left,
                Right = codeExpression
            };
            return codeAssignStatement;
        }

        #endregion

        #region GET

        /// <summary>
        /// 获取基础容器
        /// </summary>
        /// <returns></returns>
        public CodeDomProvider GetProvider()
        {
            return provider;
        }

        /// <summary>
        /// 获取编译结果
        /// </summary>
        /// <param name="codeCompileUnit"></param>
        /// <returns></returns>
        public CompilerResults GetCompilerResults(string name, CodeCompileUnit codeCompileUnit)
        {
            CompilerResults cr = provider.CompileAssemblyFromDom(compilerParameterArray[name], codeCompileUnit);
            if (cr.Errors.HasErrors)
            {
                string exMsg = "编译出错!\n";
                foreach (CompilerError err in cr.Errors)
                {
                    exMsg += err.ErrorText + "\n";
                }
                throw new Exception(exMsg);
            }
            return cr;
        }

        /// <summary>
        /// 获取编译结果
        /// </summary>
        /// <param name="codeDomProvider">codeDom基类</param>
        /// <param name="compilerParameters">编译参数</param>
        /// <param name="codeCompileUnit">codeDom图形容器</param>
        /// <returns></returns>
        public CompilerResults GetCompilerResults(CodeDomProvider codeDomProvider, CompilerParameters compilerParameters, CodeCompileUnit codeCompileUnit)
        {
            CompilerResults cr = codeDomProvider.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
            if (cr.Errors.HasErrors)
            {
                string exMsg = "编译出错!\n";
                foreach (CompilerError err in cr.Errors)
                {
                    exMsg += err.ErrorText + "\n";
                }
                throw new Exception(exMsg);
            }
            return cr;
        }


        //↓↓ 待废弃


        public string GetAssemblyLocationPath<T>()
        {
            return typeof(T).Assembly.Location;
        }

        public string GetAssemblyLocationPath(Type type)
        {
            return type.Assembly.Location;
        }

        #endregion
    }
}
