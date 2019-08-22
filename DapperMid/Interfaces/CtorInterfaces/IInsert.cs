
using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IInsert<T> where T : DataTable
    {
        int Insert(T entity);
    }
}
