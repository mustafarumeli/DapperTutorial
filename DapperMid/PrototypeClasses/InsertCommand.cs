using DapperMid.Attributes;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Linq;
using System.Reflection;

namespace DapperMid.PrototypeClasses
{
    class InsertCommand<T> : IGetInsertSql<T> where T : Datatable
    {

        public string GetInsertSql(Type type = null)
        {
            type = type ?? typeof(T);
            Type fkType = typeof(ForeignKeyAttribute);
            PropertyInfo[] properties = type.GetProperties();
            string sqlCommand = $"Insert into {type.Name}(";
            string afterValue = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                CustomAttributeData fkAttr = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == fkType);
                string name = fkAttr != null ? fkAttr.ConstructorArguments[0].Value.ToString() : property.Name;
                sqlCommand += $"[{name}],";
                afterValue += $"@{name},";
            }
            sqlCommand = sqlCommand.Remove(sqlCommand.Length - 1, 1);
            sqlCommand += ") ";
            sqlCommand += "values(";
            sqlCommand += afterValue.Remove(afterValue.Length - 1, 1);
            sqlCommand += ")";
            return sqlCommand;
        }
    }
}
