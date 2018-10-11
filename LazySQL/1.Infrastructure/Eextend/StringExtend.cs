using System;
using System.Data;

namespace LazySQL.Infrastructure
{
    public static class StringExtend
    {
        public static Type ConventToTypes(this String str)
        {
            switch (str.ToLower())
            {
                case "select":
                    return typeof(DataTable);

                default:
                    return typeof(ExecuteNonModel);
            }
        }
    }
}
