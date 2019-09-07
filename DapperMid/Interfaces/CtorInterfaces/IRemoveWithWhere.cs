using DapperMid.DataTables;
using System;
using System.Linq.Expressions;

namespace DapperMid.Interfaces.CtorInterfaces
{
    interface IRemoveWithWhere<T> where T : Datatable
    {
        int RemeoveWithWhereClause(Expression<Predicate<T>> expression, object obj);
    }
}
