using DapperMid.DataTables;
using System;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IGetInsertSql<T> where T : DataTable
    {
        string GetInsertSql(Type type = null);
    }
}