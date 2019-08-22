using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IRepository<T> : ISelect<T>, IInsert<T>, IInsertMany<T> where T : DataTable
    {

    }
}
