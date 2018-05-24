using Dapper;
using System;
using System.Collections.Generic;
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
        private string connectionString;
        public string ConnectionString
        {
            get { return connectionString; }
            private set { this.connectionString = value; }
        }

        public DefaultDataRepository(string sql)
        {
            this.Connection = DbFactory.CreateDbContext(sql);
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
