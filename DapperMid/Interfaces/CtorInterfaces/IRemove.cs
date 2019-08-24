using DapperMid.DataTables;
namespace DapperMid.Interfaces.CtorInterfaces
{
    interface IRemove<T> : IRemoveAll<T>, IRemoveById<T>, IRemoveWithWhere<T> where T : Datatable
    {
    }
}
