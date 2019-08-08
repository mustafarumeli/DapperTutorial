using DapperCrud;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperTutorial
{
    public static class CrudFactory
    {
        #region private
        static string _connectionString = "Data Source=.;Initial Catalog=DapperTest;Integrated Security=True;"; //Wrong Way
        static DapperCrud<DapperTable> _dapperTableCrud = null;
        #endregion
        #region public 
        public static DapperCrud<DapperTable> DapperTableCrud
        {
            get
            {
                if (_dapperTableCrud == null)
                {
                    _dapperTableCrud = new DapperCrud<DapperTable>(_connectionString);
                }
                return _dapperTableCrud;
            }
        }
        #endregion
    }
}
