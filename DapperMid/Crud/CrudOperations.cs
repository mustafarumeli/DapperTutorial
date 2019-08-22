using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using DapperMid.PrototypeClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace DapperMid.Crud
{
    class CrudOperations<T> : IRepository<T> where T : DataTable
    {
        ISelect<T> _selectOperations;
        IInsert<T> _insertOperations;
        SqlConnection _db;
        public CrudOperations(SqlConnection db)
        {
            _db = db;
            //Better if provided via a DI Container
            var selectCommand = new SelectCommand<T>();
            _selectOperations = new SelectOperations<T>(db, selectCommand);
            //Better if provided via a DI Container
            var insertCommand = new InsertCommand<T>();
            _insertOperations = new InsertOperation<T>(db, insertCommand);
        }
        public void AddToForeignKeyList(Type type)
        {
            _selectOperations.AddToForeignKeyList(type);
        }

        public void AddToProperties(PropertyInfo property)
        {
            _selectOperations.AddToProperties(property);
        }

        public int Insert(T entity)
        {
            return _insertOperations.Insert(entity);
        }

        public int InsertMany(T[] entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Select(string whereClause = null, string joinDirection = "inner", bool isDistinct = false, int top = -1)
        {
            return _selectOperations.Select(whereClause, joinDirection, isDistinct, top);
        }
    }
}
