using DapperCrud.Interfaces.SqlCommadInterfaces;
using System;

namespace DapperCrud.AbstractClasses.SqlCommad
{
    public class UpdateCommand<T> : IGetUpdateSql<T> where T : DbTable
    {
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
