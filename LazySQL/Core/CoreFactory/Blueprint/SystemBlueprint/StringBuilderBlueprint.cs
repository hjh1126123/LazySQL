﻿using LazySQL.Core.CoreFactory.Tools;
using System.CodeDom;
using System.Text;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class StringBuilderBlueprint : IBlueprint
    {
        public StringBuilderBlueprint()
        {
            SetField("StrbSQL");
        }

        public StringBuilderBlueprint(string Field)
        {
            SetField(Field);            
        }

        public CodeExpression Create()
        {
            return ToolManager.Instance.InitializeTool.CreateAndInstantiation<StringBuilder>(Field);
        }

        public CodeExpression Append(string Text)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithValue(Field, "Append", Text);
        }

        public CodeExpression AppendField(string AssField)
        {
            return ToolManager.Instance.InvokeTool.InvokeWithObj(Field, "Append", AssField);
        }
    }
}
