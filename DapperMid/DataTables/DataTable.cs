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
    public class DataTable
    {
        public DataTable()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}

