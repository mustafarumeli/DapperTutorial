using DapperMid.DataTables;
using System.Collections.Generic;
namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface ISelect<T> : ISelectRequirements where T : Datatable
    {
        IEnumerable<T> Select(string whereClause = null, string joinDirection = "inner", bool isDistinct = false, int top = -1);
    }
}
