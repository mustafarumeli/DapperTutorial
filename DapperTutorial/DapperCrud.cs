using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DapperTutorial
{
    public class DapperCrud<T>
    {
        SqlConnection _connection = null;
        public DapperCrud()
        {
            _connection = new SqlConnection("Data Source=.;Initial Catalog=DapperTest;Integrated Security=True;");
        }
        string GetInsertCommand()
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            string sqlCommand = $"Insert into {type.Name} values(";
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                sqlCommand += "@" + property.Name + ",";
            }
            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1);
            sqlCommand += ")";
            return sqlCommand;
        }
        string GetSelectCommand()
        {
            return "Select * from " + typeof(T).Name;
        }
        public List<T> List
        {
            get
            {
                _connection.Open();
                List<T> list = _connection.Query<T>(GetSelectCommand()).AsList<T>();
                _connection.Close();
                return list;
            }
        }
        public void Insert(T entity)
        {
            _connection.Open();
            _connection.Execute(GetInsertCommand(), entity);
            _connection.Close(); 
        }
    }
}
