using DapperMid.DataTables;
namespace DapperMid.Interfaces.CtorInterfaces
{
    interface IRemoveWithWhere<T> where T : Datatable
    {
        int RemeoveWithWhereClause(string whereClaues);
    }
}
