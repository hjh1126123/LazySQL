//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Autogeneration.Dao.SQL
{
    
    
    public class userQueryClass
    {
        
        public static System.Data.DataTable userQuery(string user, string pwd, string id)
        {
            System.Text.StringBuilder StrbSQL = new System.Text.StringBuilder();
            LazySQL.SQLite.SQLiteTemplate sqlLiteT = LazySQL.SQLite.SQLiteTemplate.Instance;
            try
            {
                System.Collections.Generic.List<System.Data.SQLite.SQLiteParameter> aList = new System.Collections.Generic.List<System.Data.SQLite.SQLiteParameter>();
                StrbSQL.Append("select * from user where 1=1 ");
                System.Text.StringBuilder par0 = new System.Text.StringBuilder();
                if (!string.IsNullOrWhiteSpace(user))
                {
                    par0.Append(" AND ");
                    par0.Append("user = @user");
                    System.Data.SQLite.SQLiteParameter userPar = new System.Data.SQLite.SQLiteParameter("@user",user);
                    aList.Add(userPar);
                }
                if (!string.IsNullOrWhiteSpace(pwd))
                {
                    par0.Append(" AND ");
                    par0.Append("pwd = @pwd");
                    System.Data.SQLite.SQLiteParameter pwdPar = new System.Data.SQLite.SQLiteParameter("@pwd",pwd);
                    aList.Add(pwdPar);
                }
                if (!string.IsNullOrWhiteSpace(id))
                {
                    par0.Append(" AND ");
                    par0.Append("id = @id");
                    System.Data.SQLite.SQLiteParameter idPar = new System.Data.SQLite.SQLiteParameter("@id",id);
                    aList.Add(idPar);
                }
                StrbSQL.Append(par0);
                return sqlLiteT.ExecuteDataTable("t", StrbSQL, aList);
            }
            catch (System.Exception ex)
            {
                throw ex;;
            }
        }
    }
}
