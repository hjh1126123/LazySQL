using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySQL.Infrastructure
{
    public class IOHelper
    {
        private static IOHelper _instance;
        public static IOHelper GetInstance()
        {
            if (_instance == null)
                _instance = new IOHelper();

            return _instance;
        }

        public void CreateDir(string DirPath)
        {
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }
        }

        public void StreamWrite(string name, Action<StreamWriter> writeAction)
        {
            using (StreamWriter sourceWriter = new StreamWriter(name))
            {
                writeAction(sourceWriter);
            }
        }
    }
}
