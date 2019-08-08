using DapperCrud.Interfaces.SqlCommadInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperCrud.AbstractClasses.SqlCommad
{
    public class SqlCommands<T> : ISqlCommands<T> where T : DbTable
    {
        public string GetSelectCommand()
        {
            return "Select * from " + typeof(T).Name;
        }
        public string GetDeleteCommand()
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            string sqlCommand = $"Delete {type.Name}";
            sqlCommand += Environment.NewLine + "Where Id = @Id";
            return sqlCommand;
        }
        public string GetInsertCommand()
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            string sqlCommand = $"Insert into {type.Name}(";
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                sqlCommand += property.Name + ",";
            }
            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1);
            sqlCommand += ") ";
            sqlCommand += "values(";
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                sqlCommand += "@" + property.Name + ",";
            }
            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1);
            sqlCommand += ")";
            return sqlCommand;
        }
        public string GetUpdateSql()
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            string sqlCommand = $"Update {type.Name} set " + Environment.NewLine;
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                sqlCommand += property.Name + " = @" + property.Name + ",";
            }
            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1);
            sqlCommand += Environment.NewLine + "Where Id = @Id";
            return sqlCommand;
        }
    }

}
