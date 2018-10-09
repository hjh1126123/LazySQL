using System;
using System.Linq;
using System.Text;

namespace LazySQL.Infrastructure
{
    public static class ExceptionEextend
    {
        public static Exception ThrowMineFormat(this Exception ex,object obj,string FuncName,params string[] args)
        {
            StringBuilder tmp = new StringBuilder();
            foreach(string tpStr in args)
            {
                tmp.Append($"{tpStr}");
                if (!tpStr.Equals(args.Last()))
                {
                    tmp.Append(",");
                }
            }
            
            return new Exception($"{obj.GetType().Name}.{FuncName}({tmp.ToString()})错误[错误行数【{ex.StackTrace}】错误对象【{ex.Source}】]错误信息【{ex.Message}】");
        }
    }
}
