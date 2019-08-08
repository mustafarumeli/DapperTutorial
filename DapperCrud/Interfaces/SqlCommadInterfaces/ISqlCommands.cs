using System;
using System.Collections.Generic;
using System.Text;

namespace DapperCrud.Interfaces.SqlCommadInterfaces
{
    public interface ISqlCommands<T> : IGetDeleteSql<T>, IGetInsertSql<T>, IGetSelectSql<T>, IGetUpdateSql<T> where T : DbTable
    {
    }
}
