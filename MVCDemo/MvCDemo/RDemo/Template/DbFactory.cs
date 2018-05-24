using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RDemo
{
    public enum DbType
    {
        None,
        SqlServer,
        MySql,
        Oracle,
        Access,
        Firebird,
    }
    public abstract class DbFactory
    {

        public static string DefaultConnCharset = ConfigurationManager.AppSettings["db.charset"];
        public DbFactory(string connectionString, bool ignoreCase = true)
        {
            this.ConnectionString = connectionString;
        }
        public string ConnectionString { get; private set; }
        public abstract DbConnection CreateConection();

        public static DbConnection CreateDbContext(string connectionString)
        {
            DbFactory dbHelper = null;
            switch (GetDbType(connectionString))
            {
                case DbType.MySql:
                    dbHelper = new MySqlDbHelper(connectionString);
                    break;
                case DbType.Oracle:
                    dbHelper = new OleSqlDbHelper(connectionString);
                    break;
                case DbType.Access:
                    dbHelper = new AccessSqlDbHelper(connectionString);
                    break;
                case DbType.None:
                case DbType.SqlServer:
                case DbType.Firebird:
                default:
                    throw new InvalidOperationException("未接入该类型的数据库处理工具");
            }
            return dbHelper.CreateConection();
        }


        internal static DbType GetDbType(string connectionString, bool throwError = true)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                if (!throwError)
                    return DbType.None;
                throw new InvalidOperationException("没有指定连接字符串");
            }
            if (connectionString.IndexOf("OraOLEDB", StringComparison.OrdinalIgnoreCase) >= 0 || connectionString.IndexOf("MSDAORA", StringComparison.OrdinalIgnoreCase) >= 0)
                return DbType.Oracle;
            if (connectionString.IndexOf("Jet.OLEDB", StringComparison.OrdinalIgnoreCase) >= 0)
                return DbType.Access;
            if (connectionString.IndexOf(".fdb", StringComparison.OrdinalIgnoreCase) > 0)
                return DbType.Firebird;
            if (connectionString.Contains("Data Source="))
                return DbType.SqlServer;
            if (connectionString.IndexOf("Server=", StringComparison.OrdinalIgnoreCase) >= 0)
                return DbType.MySql;
            if (!throwError)
                return DbType.None;
            throw new InvalidOperationException("不支持的连接字符串：" + connectionString);
        }
    }
    public class OleSqlDbHelper : DbFactory
    {
        public OleSqlDbHelper(string connectionString, bool fieldNameToLower = false) : base(connectionString, fieldNameToLower)
        {

        }
        public override DbConnection CreateConection()
        {
            return (DbConnection)new OleDbConnection(this.ConnectionString);
        }
        
    }
    public class AccessSqlDbHelper : OleSqlDbHelper
    {
        public AccessSqlDbHelper(string connectionString, bool fieldNameToLower = false) : base(connectionString, fieldNameToLower)
        {
        }
    }
    public class MySqlDbHelper : DbFactory
    {

        public MySqlDbHelper(string connectionString, bool fieldNameToLower = false) : base(connectionString, fieldNameToLower)
        {

        }
        
        
        public override DbConnection CreateConection()
        {
            string connectionString = this.ConnectionString;
            if (!string.IsNullOrEmpty(DbFactory.DefaultConnCharset))
            {
                connectionString = new Regex("(\\s*charset\\s*=\\s*(\\w|\\s)*;?)|(\\s*character set\\s*=\\s*(\\w|\\s)*;?)", RegexOptions.IgnoreCase).Replace(connectionString, "");
                connectionString = string.Format("{0}{1}{2}", (object)connectionString, connectionString.EndsWith(";") ? (object)"charset=" : (object)";charset=", (object)DbFactory.DefaultConnCharset);
            }
            return (DbConnection)new MySqlConnection(connectionString);
        }        
    }
    /// <summary>
    /// Sql数据库分页需要支持rownumber动态索引
    /// </summary>
    public class SqlDbHelper : DbFactory
    {
        public SqlDbHelper(string connectionString, bool fieldNameToLower = false) : base(connectionString, fieldNameToLower)
        {

        }
        
        
        public override DbConnection CreateConection()
        {
            return new SqlConnection(this.ConnectionString);
        }        
    }
}
