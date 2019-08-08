namespace DapperCrud.Interfaces.SqlCommadInterfaces
{
    public interface IGetSelectSql<T> where T : DbTable
    {
        string GetSelectCommand();
    }
}
