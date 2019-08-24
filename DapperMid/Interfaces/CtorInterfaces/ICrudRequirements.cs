using System.Data.SqlClient;

namespace DapperMid.Interfaces.CtorInterfaces
{
    /// <summary>
    /// Crud's must provide a method that expose SqlConnection in addition to other Requirements
    /// </summary>
    public interface ICrudRequirements
    {
        SqlConnection GetCurrentSqlConnection();
    }
}
