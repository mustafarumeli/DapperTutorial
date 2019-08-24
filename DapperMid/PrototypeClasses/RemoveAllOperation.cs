using Dapper;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Data.SqlClient;

namespace DapperMid.PrototypeClasses
{
    class RemoveAllOperation<T> : IRemoveAll<T> where T : Datatable
    {
        readonly SqlConnection _db;
        readonly IRemoveCommand<T> _removeCommand;

        public RemoveAllOperation(SqlConnection db, IRemoveCommand<T> removeCommand)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _removeCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
        }

        public int RemoveAll()
        {
            string command = _removeCommand.GetRemoveCommand();
            return _db.Execute(command);
        }
    }
}