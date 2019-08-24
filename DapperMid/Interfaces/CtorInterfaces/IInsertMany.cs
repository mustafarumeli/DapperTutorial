
using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IInsertMany<T> where T : Datatable
    {
        int InsertMany(params T[] entities);
    }
}
