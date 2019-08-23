using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IRepository<T> : ISelect<T>, IInsert, IInsertMany<T> where T : Datatable
    {

    }
}
