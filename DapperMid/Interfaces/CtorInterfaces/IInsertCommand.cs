using DapperMid.DataTables;
using System;

namespace DapperMid.Interfaces.CtorInterfaces
{

    public interface IInsertCommand<T> where T : Datatable
    {
        string GetInsertCommand(Type type = null);
    }
}