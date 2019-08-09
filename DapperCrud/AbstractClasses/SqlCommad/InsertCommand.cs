using DapperCrud.Interfaces.SqlCommadInterfaces;

namespace DapperCrud.AbstractClasses.SqlCommad
{
    public class InsertCommand<T> : IGetInsertSql<T> where T : DbTable
    {
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
    }

}
