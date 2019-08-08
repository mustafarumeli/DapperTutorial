using System;
using System.Collections.Generic;
using System.Text;

namespace DapperCrud.Interfaces.SqlCommadInterfaces
{
    public interface IGetInsertSql<T> where T : DbTable
    {
        string GetInsertCommand();
    }
}
