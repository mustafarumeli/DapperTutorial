using Dapper;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Data.SqlClient;

namespace DapperMid.PrototypeClasses
{
    class RemoveWithWhereClauseOperation<T> : IRemoveWithWhere<T> where T : Datatable
    {
        readonly SqlConnection _db;
        readonly IRemoveCommand<T> removeCommand;

        public RemoveWithWhereClauseOperation(SqlConnection db, IRemoveCommand<T> removeCommand)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            this.removeCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
        }

        public int RemeoveWithWhereClause(string whereClaues)
        {
            var command = removeCommand.GetRemoveCommand();
            command += " " + whereClaues;
            return _db.Execute(command);
        }

    }
}