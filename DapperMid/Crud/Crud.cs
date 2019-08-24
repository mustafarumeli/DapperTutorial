using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
namespace DapperMid.Crud
{
    /// <summary>
    /// A Façade? class for Managening Crud(Create,Read,Update,Delete) ....
    /// </summary>
    /// <typeparam name="T">Table which currently perform must inherit from DataTable</typeparam>
    public class Crud<T> : ICrudRequirements, IRepository<T> where T : Datatable
    {
        //neccesary for Decorator Pattern better if given in Ctor probably from a DI container
        CrudOperations<T> _crudOperations;
        SqlConnection _db;
        public Crud(SqlConnection db)
        {
            _db = db;
            //Better if provided via a DI Container
            _crudOperations = new CrudOperations<T>(db);
        }
        public SqlConnection GetCurrentSqlConnection()
        {
            return _db;
        }
        public IEnumerable<T> Select(string whereClause = null, string joinDirection = "inner", bool isDistinct = false, int top = -1)
        {
            return _crudOperations.Select(whereClause, joinDirection, isDistinct, top);
        }
        public void AddToForeignKeyList(Type type)
        {
            _crudOperations.AddToForeignKeyList(type);
        }
        public void AddToProperties(PropertyInfo property)
        {
            _crudOperations.AddToProperties(property);
        }
        public int Insert(T entity)
        {
            return _crudOperations.Insert(entity);
        }
        public int InsertMany(params T[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
