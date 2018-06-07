using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Text;

namespace RDemo
{
    public interface IDataRepository
    {
        String ConnectionString { get; }
        IEnumerable<TParam> Select<TParam>(string sql, object anonymityObj = null);
        int ExcuteNoQuery(string sql, object anonymityObj = null);
        int Save(string sql, object anonymityObj = null);
        int Save(string sql, IList<object> anonymityObj);
        int Delete(string sql, params object[] anonymityObj);
        int Update(string sql, params object[] anonymityObj);
    }
    public class DefaultDataRepository : IDataRepository
    {
        protected DbConnection Connection { get; private set; }
        private string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
            private set { this._connectionString = value; }
        }
        public DefaultDataRepository() : this(ConfigurationManager.AppSettings["db.master"])
        {
        }

        public DefaultDataRepository(string connectionString)
        {
            this._connectionString = connectionString;
            this.Connection = DbFactory.CreateDbContext(this._connectionString);
        }

        public IEnumerable<TParam> Select<TParam>(string sql, object anonymityObj = null)
        {
            return DoExcute(() =>
            {
                return this.Connection.Query<TParam>(sql, anonymityObj);
            });
        }
        
        public int ExcuteNoQuery(string sql, object anonymityObj = null)
        {
            return DoExcute(() =>
            {
                return this.Connection.Execute(sql, anonymityObj);
            });
        }

        public int Save(string sql, object anonymityObj = null)
        {
            return DoExcute(() =>
            {
                return this.Connection.Execute(sql, anonymityObj);
            });
        }

        public int Save(string sql,IList<object> anonymityObj)
        {
            return DoExcute(() =>
            {
                return this.Connection.Execute(sql, anonymityObj);
            });
        }

        public int Delete(string sql,params object[] anonymityObj)
        {
            return DoExcute(() =>
            {
                return this.Connection.Execute(sql, anonymityObj);
            });
        }

        public int Update(string sql, params object[] anonymityObj)
        {
            return DoExcute(() =>
            {
                return this.Connection.Execute(sql, anonymityObj);
            });
        }
        protected T DoExcute<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            finally
            { 
            }
        }
        
    }
    
}
