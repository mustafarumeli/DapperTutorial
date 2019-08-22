
using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IInsertMany<T> where T : DataTable
    {
        int InsertMany(T[] entities);
    }
}
