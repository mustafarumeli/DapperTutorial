namespace DapperCrud.Interfaces.IRepositoryInterfaces
{
    public interface IRepositorty<T> : IDelete<T>, IUpdate<T>, IList<T>, IInsert<T> where T : DbTable
    {

    }

}
