using DapperMid.DataTables;
namespace DapperMid.Interfaces.CtorInterfaces
{

    interface IRemoveById<T> where T : Datatable
    {
        int Remove(string id);
    }
}
