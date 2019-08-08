using System.Collections.Generic;

namespace DapperCrud.Interfaces.IRepositoryInterfaces
{
    public interface IList<T> where T : DbTable
    {
        List<T> List { get; }
    }
}
