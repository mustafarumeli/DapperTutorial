using Dapper;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
namespace DapperMid.PrototypeClasses
{
    class SelectOperations<T> : ISelect<T> where T : Datatable
    {
        List<Type> _fkList { get; }
        HashSet<PropertyInfo> _props { get; }
        SqlConnection _db { get; }
        IGetSelectCommand _selectCommand;
        public SelectOperations(SqlConnection db, IGetSelectCommand selectCommand = null)
        {
            _fkList = new List<Type>();
            _props = new HashSet<PropertyInfo>();
            _db = db;
            _fkList.Add(typeof(T));
            _selectCommand = selectCommand;
        }
        /// <summary>
        /// Uses Reflection To Map One To One ForeignKeys using Dapper
        /// Automaticly Generetes Select Statement With all values (Select * from table1..)
        /// Automaticly Creates inner joins based on ForeignKeyAttribute
        /// </summary>
        /// <param name="con"> SqlConnection Which will be perform on</param>
        /// <param name="whereClause"> Where clause that will be added at the end of command </param>
        /// <param name="joinDirection">This will indicate the direction of join default value is "inner"</param>
        /// <param name="selectType">Enum SelectType determines return value's complexity see Enum Definiton for MoreInfo</param>
        /// <returns>An IEnumerable of Given type</returns>
        public IEnumerable<T> Select(string whereClause, string joinDirection, bool isDistinct, int top)
        {

            string command = _selectCommand.GetSelectWithJoin(joinDirection, isDistinct, top, _props);
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                command += " " + whereClause;
            }
            return _db.Query(command, _fkList.ToArray(),
              objects =>
              {
                  if (objects is null)
                  {
                      throw new ArgumentNullException(nameof(objects));
                  }

                  foreach (PropertyInfo prop in _props)
                  {
                      object valueObject = objects.First(x => x.GetType() == prop.PropertyType);
                      object propObject = objects.First(x => x.GetType() == prop.DeclaringType);
                      prop.SetValue(propObject, valueObject);
                  }
                  return (T)objects[0];
              });
        }
        public void AddToForeignKeyList(Type type)
        {
            if (_fkList.Contains(type))
            {
                throw new Exception("Can't add Duplicate types");
            }
            _fkList.Add(type);
        }
        public void AddToProperties(PropertyInfo property)
        {
            _props.Add(property);
        }
    }
}
