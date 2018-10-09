using System.Text;

namespace LazySQL.Infrastructure
{
    public class StringHelper
    {
        private static StringHelper _instance;
        public static StringHelper GetInstance()
        {
            if (_instance == null)
                _instance = new StringHelper();

            return _instance;
        }
        private StringHelper() { }

        
        public string UnicodeForSQL(string text)
        {
            StringBuilder strB = new StringBuilder();
            strB.Append("N'" + text + "'");

            return strB.ToString();
        }
    }
}