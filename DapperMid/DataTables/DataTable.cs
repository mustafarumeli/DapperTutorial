using DapperMid.Interfaces;
using System;
namespace DapperMid.DataTables
{
    public abstract class Datatable : IDatatable<string>
    {
        public string Id { get; }

        protected Datatable()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

