using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using DapperMid.PrototypeClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace DapperMid.Crud
{
    class CrudOperations<T> : IRepository<T> where T : Datatable
    {
        readonly ISelect<T> _selectOperations;
        readonly IInsert<T> _insertOperations;
        readonly SqlConnection _db;
        public CrudOperations(SqlConnection db)
        {
            //Better if provided via a DI Container
            _db = db;
            //Better if provided via a DI Container
            var selectCommand = new SelectCommand<T>();
            _selectOperations = new SelectOperations<T>(db, selectCommand);

            //Better if provided via a DI Container
            var insertCommand = new InsertCommand<T>();
            //Better if provided via a DI Container
            var insertInside = new InsertInside(_db);
            _insertOperations = new InsertOperation<T>(db, insertCommand, insertInside);
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

        public int InsertMany(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Select(string whereClause = null, string joinDirection = "inner", bool isDistinct = false, int top = -1)
        {
            return _selectOperations.Select(whereClause, joinDirection, isDistinct, top);
        }
    }
}
