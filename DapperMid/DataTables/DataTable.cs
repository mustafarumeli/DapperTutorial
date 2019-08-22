using System;
namespace DapperMid.DataTables
{
    public class DataTable
    {
        public DataTable()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}

