using System;
using System.Collections.Generic;
using System.Data;

namespace LazySQL.Infrastructure
{
    public static class StringExtend
    {
        /// <summary>
        /// 根据字符串名称转换特殊类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取字符串（英文）的所有大小写的排列组合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> StringAllCombination(this String str)
        {
            List<string> List = new List<string>();
            List.Add(str);
            List<string> ListTemp = new List<string>();
            return Get(List, str.Length - 1);
        }

        private static List<string> Get(List<string> List, int tag)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < List.Count; i++)
            {
                result.Add(List[i].Substring(0, tag) + List[i].Substring(tag, 1).ToUpper() + List[i].Substring(tag + 1, List[i].Length - tag - 1));
                result.Add(List[i].Substring(0, tag) + List[i].Substring(tag, 1).ToLower() + List[i].Substring(tag + 1, List[i].Length - tag - 1));
            }
            if (tag == 0)
                return result;
            tag--;
            return Get(result, tag);
        }
    }
}
