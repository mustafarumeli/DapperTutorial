using System;

namespace DapperCrud.Interfaces.IRepositoryInterfaces
{
    public interface IInsert<T> where T : DbTable
    {
        int Insert(T Entity);
    }
}
