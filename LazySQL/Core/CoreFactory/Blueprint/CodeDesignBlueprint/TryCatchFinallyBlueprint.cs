﻿using LazySQL.Core.CoreFactory.Tools;
using System;
using System.CodeDom;

namespace LazySQL.Core.CoreFactory.Blueprint
{
    public class TryCatchFinallyBlueprint
    {
        public CodeStatement Create(Func<CodeStatementCollection> tryCode)
        {
            return ToolManager.Instance.SecurityTool.CreateTry(tryCode);
        }

        public CodeStatement Create(Func<CodeStatementCollection> tryCode, Func<CodeStatementCollection> catchCode)
        {
            return ToolManager.Instance.SecurityTool.CreateTry(tryCode, catchCode);
        }

        public CodeStatement Create(Func<CodeStatementCollection> tryCode,Func<CodeStatementCollection> catchCode,Func<CodeStatementCollection> finallyCode)
        {
            return ToolManager.Instance.SecurityTool.CreateTry(tryCode, catchCode, finallyCode);
        }
    }
}
