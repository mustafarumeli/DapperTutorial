namespace DapperCrud.Interfaces.SqlCommadInterfaces
{
    public interface IGetUpdateSql<T> where T : DbTable
    {
        string GetUpdateSql();
    }
}
