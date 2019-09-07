using Dapper;
using DapperMid.Attributes;
using DapperMid.DataTables;
using DapperMid.Interfaces;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DapperMid.PrototypeClasses
{
    class InsertOperation<T> : IInsert<T> where T : Datatable
    {
        IInsertCommand<T> _insertCommand { get; }
        SqlConnection _db { get; }
        IInsertInside _insertInside;

        public InsertOperation(SqlConnection db, IInsertCommand<T> insertCommand, IInsertInside insertInside)
        {
            _insertCommand = insertCommand ?? throw new ArgumentNullException(nameof(insertCommand));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _insertInside = insertInside ?? throw new ArgumentNullException(nameof(insertInside));
        }

        /// <summary>
        /// Uses Dapper, performs sql insert operation recursively from bottom foreign key to top
        /// </summary>
        /// <param name="entity">varible to insert must be datatable</param>
        /// <returns>Affected Row Count</returns>
        public int Insert(T entity)
        {
            // reason with expand object is a typical class holds foreign keys as 
            // property with reffering class but in database they are just fields
            // so we need a way to convert datatable class to proper datatable Table
            dynamic expando = new ExpandoObject();
            var expandoDict = expando as IDictionary<string, object>;
            var fKType = typeof(ForeignKeyAttribute);
            Type entityType = entity.GetType();
            foreach (var prop in entityType.GetProperties())
            {
                var fkAttr = prop.GetCustomAttribute<ForeignKeyAttribute>();
                if (fkAttr != null)  // If property is reffering a Foreign Key  then
                {
                    // We get that object
                    var innerObj = prop.GetValue(entity);
                    _insertInside.Insert((Datatable)innerObj); // We insert it to the database (recursively)
                    var id = innerObj.GetType().GetProperty(nameof(IDatatable<T>.Id)).GetValue(innerObj); // We get the id of it
                    string fkName = fkAttr.Name; // We get the Field Name in Database
                    expandoDict.Add(fkName, id); // We add it to expando with using dict (it will eventually will be a property(woah!!))
                }
                else // If property is not a Foreign Key then
                {
                    expandoDict.Add(prop.Name, prop.GetValue(entity)); // We add it to expando with using dict (it will eventually will be a property(woah!!))
                }
            }
            string sqlCommand = _insertCommand.GetInsertCommand(entityType); // We get the insert command for entityType(T)
            return _db.Execute(sqlCommand, (object)expando); // We use dapper Execute function we provide sqlCommand and expando as object
        }


    }
}
