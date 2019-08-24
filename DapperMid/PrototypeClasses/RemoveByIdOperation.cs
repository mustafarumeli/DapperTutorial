using Dapper;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Data.SqlClient;

namespace DapperMid.PrototypeClasses
{
    class RemoveByIdOperation<T> : IRemoveById<T> where T : Datatable
    {
        readonly SqlConnection _db;
        readonly IRemoveCommand<T> _removeCommand;

        public RemoveByIdOperation(SqlConnection db, IRemoveCommand<T> removeCommand)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _removeCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
        }

        public int Remove(string id)
        {
            string command = _removeCommand.GetRemoveCommand();
            command += " Where Id = @id";
            return _db.Execute(command, new { id });
        }
    }
}