using System;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace DapperMid.DataTables
{

    class Address : DataTable
    {
        public Address(string desc)
        {
            Desc = desc ?? throw new ArgumentNullException(nameof(desc));
        }

        public string Desc { get; set; }
    }
}

