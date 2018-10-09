using System;
using System.Data;
using System.Linq;

namespace LazySQL.Infrastructure
{
    public static class StringExtend
    {
        public static Type ConventToTypes(this String str)
        {
            switch (str.ToLower())
            {
                case "int?":
                    return typeof(int?);

                case "double?":
                    return typeof(double?);

                case "datetime?":
                    return typeof(DateTime?);

                case "datatable":
                    return typeof(DataTable);
            }
            return Type.GetType($"System.{str}", true, true);
        }
    }
}
