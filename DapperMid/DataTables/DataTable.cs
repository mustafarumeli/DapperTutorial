using System;
namespace DapperMid.DataTables
{
    public abstract class Datatable
    {
        public string Id { get; }

        protected Datatable()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

