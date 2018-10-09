using System.Collections.Generic;

namespace LazySQL.Infrastructure
{
    public class SQLHelper
    {
        private static SQLHelper _instance;
        public static SQLHelper GetInstance()
        {
            if (_instance == null)
                _instance = new SQLHelper();

            return _instance;
        }

        private Dictionary<string, string> MSSQLConvertSave;
        public string CSharpTypeConvertMSSqlType(string typeName)
        {
            string tempTypeName = typeName.ToLower();
            if (MSSQLConvertSave.ContainsKey(tempTypeName))
            {
                return MSSQLConvertSave[tempTypeName];
            }
            else
            {
                return typeName;
            }
        }

        private SQLHelper()
        {
            MSSQLConvertSave = new Dictionary<string, string>
            {
                { "string" , "NVARCHAR(MAX)" },
                { "datetime" , "DATETIME" },
                { "int", "INT" },
                { "float", "FLOAT" }
            };
        }
    }
}
