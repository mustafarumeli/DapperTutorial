
using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface IInsert<T> where T : Datatable
    {
        int Insert(T entity);
    }

}
