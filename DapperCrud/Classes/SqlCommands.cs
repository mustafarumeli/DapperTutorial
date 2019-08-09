using DapperCrud.Interfaces.SqlCommadInterfaces;
using System.Collections.Generic;
using System.Text;

namespace DapperCrud.AbstractClasses.SqlCommad
{
    public class SqlCommands<T> : ISqlCommands<T> where T : DbTable
    {
        SelectCommand<T> _selectCommand;
        DeleteCommand<T> _deleteCommand;
        InsertCommand<T> _insertCommand;
        UpdateCommand<T> _updateCommand;

        public SqlCommands()
        {
            _selectCommand = new SelectCommand<T>();
            _deleteCommand = new DeleteCommand<T>();
            _insertCommand = new InsertCommand<T>();
            _updateCommand = new UpdateCommand<T>();
        }
        public string GetDeleteCommand()
        {
            return _deleteCommand.GetDeleteCommand();
        }

        public string GetInsertCommand()
        {
            return _insertCommand.GetInsertCommand();
        }

        public string GetSelectCommand()
        {
            return _selectCommand.GetSelectCommand();
        }

        public string GetUpdateSql()
        {
            return _updateCommand.GetUpdateSql();
        }
    }

}
