using System;

namespace DapperMid.DataTables
{
    class Address : Datatable
    {
        public Address()
        {
        }
        public Address(string desc)
        {
            Desc = desc ?? throw new ArgumentNullException(nameof(desc));
        }

        public string Desc { get; set; }

    }
}

