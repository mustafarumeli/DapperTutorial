using DapperMid.DataTables;
namespace DapperMid.Interfaces.CtorInterfaces
{
    interface IRemoveAll<T> where T : Datatable
    {
        int RemoveAll();
    }
}
