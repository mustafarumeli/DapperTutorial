using DapperCrud.Interfaces.SqlCommadInterfaces;
using System;

namespace DapperCrud.AbstractClasses.SqlCommad
{
    public class DeleteCommand<T> : IGetDeleteSql<T> where T : DbTable
    {
        public string GetDeleteCommand()
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            string sqlCommand = $"Delete {type.Name}";
            sqlCommand += Environment.NewLine + "Where Id = @Id";
            return sqlCommand;
        }
    }

}
