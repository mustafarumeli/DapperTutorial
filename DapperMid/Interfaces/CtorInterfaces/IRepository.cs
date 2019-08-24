using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    interface IRepository<T> : ISelect<T>, IInsert<T>, IInsertMany<T>, IRemove<T> where T : Datatable
    {

    }
}
