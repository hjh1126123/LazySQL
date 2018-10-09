using System.Collections.Generic;

namespace LazySQL.Infrastructure.Tool
{
    public class ReflectionTool
    {
        private static ReflectionTool _instance;
        public static ReflectionTool GetInstance()
        {
            if (_instance == null)
                _instance = new ReflectionTool();

            return _instance;
        }

        private ReflectionTool() { }


        public bool HaveSomeEnum<T, Attribute>(string proName, string enumTypeName, string equalsName) where Attribute : class, new()
        {
            List<string> _atrExcludes = ReflectionHelper.GetInstance().GetAtrValueFromProWhenEnum<T, Attribute>(proName, enumTypeName);
            bool haveAEnum = false;
            foreach (var _atrExclude in _atrExcludes)
            {
                if (_atrExclude.Equals(equalsName))
                    haveAEnum = true;
            }

            return haveAEnum;
        }
    }
}
