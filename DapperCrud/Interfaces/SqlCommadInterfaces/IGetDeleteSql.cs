namespace DapperCrud.Interfaces.SqlCommadInterfaces
{
    public interface IGetDeleteSql<T> where T : DbTable
    {
        string GetDeleteCommand();
    }
}
