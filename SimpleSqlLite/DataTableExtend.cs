using System.Data;
using System.Text;

namespace SimpleSqlLite
{
    public static class DataTableExtend
    {
        public static string DTString(this DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var c = 0; c < dt.Columns.Count; c++)
            {
                stringBuilder.Append($"{dt.Columns[c].ColumnName}\t");
            }
            stringBuilder.AppendLine();
            for (var r = 0;r < dt.Rows.Count; r++)
            {
                for (var c = 0; c < dt.Columns.Count; c++)
                {
                    stringBuilder.Append($"{dt.Rows[r][dt.Columns[c].ColumnName]}\t");
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
    }
}
