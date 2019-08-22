using System;

namespace DapperCrud
{
    public class DbTable
    {
        public string Id { get; }
        public DbTable()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}