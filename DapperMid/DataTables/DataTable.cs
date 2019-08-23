using System;
namespace DapperMid.DataTables
{
    public abstract class Datatable
    {
        public Datatable()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
    }
}

