namespace DapperCrud.Interfaces.IRepositoryInterfaces
{
    public interface IUpdate<T> where T : DbTable
    {
        int Update(T entity);
    }
}
