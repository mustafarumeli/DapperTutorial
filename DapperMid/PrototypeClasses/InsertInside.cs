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
    class InsertInside : IInsertInside
    {
        IInsertCommand<Datatable> _insertCommand { get; }
        SqlConnection _db { get; }
        public InsertInside(SqlConnection db)
        {
            _insertCommand = new InsertCommand<Datatable>();
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Insert(Datatable entity)
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
                var fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                if (fkAttr != null)  // If property is reffering a Foreign Key  then
                {
                    // We get that object
                    var innerObj = prop.GetValue(entity);
                    Insert((Datatable)innerObj); // We insert it to the database (recursively)
                    var id = innerObj.GetType().GetProperty("Id").GetValue(innerObj); // We get the id of it
                    string fkName = fkAttr.ConstructorArguments[0].Value.ToString(); // We get the Field Name in Database
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
