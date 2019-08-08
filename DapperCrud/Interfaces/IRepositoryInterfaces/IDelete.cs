namespace DapperCrud.Interfaces.IRepositoryInterfaces
{
    public interface IDelete<T> where T : DbTable
    {
        int Delete(string id);
    }

}
