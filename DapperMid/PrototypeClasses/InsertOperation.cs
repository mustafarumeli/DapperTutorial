using Dapper;
using DapperMid.Attributes;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace DapperMid.PrototypeClasses
{
    class InsertOperation<T> : IInsert where T : DataTable
    {
        IGetInsertSql<T> _getInsertSql { get; }
        SqlConnection _db { get; }

        public InsertOperation(SqlConnection db, IGetInsertSql<T> getInsertSql)
        {
            _getInsertSql = getInsertSql ?? throw new ArgumentNullException(nameof(getInsertSql));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Uses Dapper, performs sql insert operation recursively from bottom foreign key to top
        /// PS: entity is object because We need to add a Recursivability which i couldn't add with generic ,yet C:
        /// </summary>
        /// <param name="entity">varible to insert must be datatable</param>
        /// <returns></returns>
        public int Insert(object entity)
        {
            if (!entity.GetType().IsSubclassOf(typeof(DataTable)))
            {
                throw new Exception("Entity must be a Datatable");
            }
            // reason with expand object is a typical class holds foreign keys as 
            // property with reffering class but in database they are just fields
            // so we need a way to convert datatable class to proper datatable Table
            dynamic expando = new ExpandoObject();
            var expandoDict = expando as IDictionary<string, object>;
            var fKType = typeof(ForeignKeyAttribute);
            Type entityType = entity.GetType();
            foreach (var prop in entityType.GetProperties())
            {
                var fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                if (fkAttr != null)  // If property is reffering a Foreign Key  then
                {
                    // We get that object
                    var innerObj = prop.GetValue(entity);
                    Insert(innerObj); // We insert it to the database (recursively)
                    var id = innerObj.GetType().GetProperty("Id").GetValue(innerObj); // We get the id of it
                    string fkName = fkAttr.ConstructorArguments[0].Value.ToString(); // We get the Field Name in Database
                    expandoDict.Add(fkName, id); // We add it to expando with using dict (it will eventually will be a property(woah!!))
                }
                else // If property is not a Foreign Key then
                {
                    expandoDict.Add(prop.Name, prop.GetValue(entity)); // We add it to expando with using dict (it will eventually will be a property(woah!!))
                }
            }
            string sqlCommand = _getInsertSql.GetInsertSql(entityType); // We get the insert command for entityType(T)
            return _db.Execute(sqlCommand, (object)expando); // We use dapper Execute function we provide sqlCommand and expando as object
        }

    }
}
