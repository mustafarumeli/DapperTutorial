using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace DapperMid.PrototypeClasses
{
    class RemoveOperations<T> : IRemove<T> where T : Datatable
    {
        readonly SqlConnection _db;
        readonly IRemoveWithWhere<T> _removeWithWhere;
        readonly IRemoveById<T> _removeById;
        readonly IRemoveAll<T> _removeAll;
        readonly IRemoveCommand<T> _removeCommand;
        public RemoveOperations(SqlConnection db, IRemoveCommand<T> removeCommand)
        {
            _removeCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
            _removeWithWhere = new RemoveWithWhereClauseOperation<T>(db, _removeCommand);
            _removeById = new RemoveByIdOperation<T>(db, _removeCommand);
            _removeAll = new RemoveAllOperation<T>(db, _removeCommand);
        }

        public int RemeoveWithWhereClause(Expression<Predicate<T>> expression, object obj)
        {
            return _removeWithWhere.RemeoveWithWhereClause(expression,obj);
        }

        public int Remove(string id)
        {
            return _removeById.Remove(id);
        }

        public int RemoveAll()
        {
            return _removeAll.RemoveAll();
        }
    }
}