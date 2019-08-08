using System;

namespace DapperCrud
{
    public class DbTable
    {
        public string Id { get; set; }
        public DbTable()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}