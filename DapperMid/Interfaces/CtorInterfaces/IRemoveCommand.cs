using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{

    interface IRemoveCommand<T> where T : Datatable
    {
        string GetRemoveCommand();
    }
}