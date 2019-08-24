
using DapperMid.DataTables;

namespace DapperMid.Interfaces.CtorInterfaces
{
    interface IInsertInside
    {
        /// <summary>
        /// Before inserting current table we must insert foreign key tables first.
        /// This Method handels this situation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Affected Row Count</returns>
        int Insert(Datatable entity);
    }

}
