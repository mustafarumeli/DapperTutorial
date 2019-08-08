using Dapper;
using DapperCrud.AbstractClasses.SqlCommad;
using DapperCrud.Interfaces.IRepositoryInterfaces;
using DapperCrud.Interfaces.SqlCommadInterfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DapperCrud
{
    public class DapperCrud<T> : IRepositorty<T> where T : DbTable
    {
        ISqlCommands<T> _sqlCommands = new SqlCommands<T>();
        SqlConnection _conn = null;
        public DapperCrud(string connectionString, ISqlCommands<T> sqlCommands = null)
        {
            if (sqlCommands != null)
            {
                _sqlCommands = sqlCommands;
            }
            _conn = new SqlConnection(connectionString);
        }
        public List<T> List
        {
            get
            {
                _conn.Open();
                List<T> list = _conn.Query<T>(_sqlCommands.GetSelectCommand()).AsList<T>();
                _conn.Close();
                return list;
            }
        }
        public int Delete(string id)
        {
            _conn.Open();
            int res = _conn.Execute(_sqlCommands.GetDeleteCommand(), new { Id = id });
            _conn.Close();
            return res;
        }

        public int Insert(T entity)
        {
            _conn.Open();
            int res = _conn.Execute(_sqlCommands.GetInsertCommand(), entity);
            _conn.Close();
            return res;
        }

        public int Update(T entity)
        {
            _conn.Open();
            int res = _conn.Execute(_sqlCommands.GetUpdateSql(), entity);
            _conn.Close();
            return res;
        }
    }
}