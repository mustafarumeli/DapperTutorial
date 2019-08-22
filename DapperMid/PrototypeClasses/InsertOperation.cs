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
    class InsertOperation<T> : IInsert<T> where T : DataTable
    {
        IGetInsertSql<T> _getInsertSql { get; }
        SqlConnection _db { get; }

        public InsertOperation(SqlConnection db, IGetInsertSql<T> getInsertSql)
        {
            _getInsertSql = getInsertSql ?? throw new ArgumentNullException(nameof(getInsertSql));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public int Insert(T entity)
        {
            dynamic expando = new ExpandoObject();
            var expandoDict = expando as IDictionary<string, object>;
            var fKType = typeof(ForeignKeyAttribute);


            foreach (var prop in typeof(T).GetProperties())
            {
                var fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                if (fkAttr != null)
                {
                    var innerObj = prop.GetValue(entity);
                    //must call Crud<ProperType>(_db).Insert();
                    var id = innerObj.GetType().GetProperty("Id").GetValue(innerObj);
                    string fkName = fkAttr.ConstructorArguments[0].Value.ToString();
                    expandoDict.Add(fkName, id);
                }
                else
                {
                    expandoDict.Add(prop.Name, prop.GetValue(entity));
                }
            }
            string sql = _getInsertSql.GetInsertSql();
            return _db.Execute(sql, (object)expando);
        }
    }
}
