using DapperMid.Attributes;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Linq;
using System.Reflection;

namespace DapperMid.PrototypeClasses
{
    class InsertCommand<T> : IGetInsertSql<T> where T : DataTable
    {
      
        public string GetInsertSql(Type type = null)
        {
            type = type ?? typeof(T);
            Type fkType = typeof(ForeignKeyAttribute);
            PropertyInfo[] properties = type.GetProperties();
            string sqlCommand = $"Insert into {type.Name}(";
            foreach (PropertyInfo property in properties)
            {

                CustomAttributeData fkAttr = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == fkType);
                string name = fkAttr != null ? fkAttr.ConstructorArguments[0].Value.ToString() : property.Name;
                sqlCommand += $"[{name}],";
            }
            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1);
            sqlCommand += ") ";
            sqlCommand += "values(";
            foreach (PropertyInfo property in properties)
            {
                CustomAttributeData fkAttr = property.CustomAttributes.FirstOrDefault(x => x.AttributeType == fkType);
                string name = fkAttr != null ? fkAttr.ConstructorArguments[0].Value.ToString() : property.Name;
                sqlCommand += $"@{name},";
            }
            sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1);
            sqlCommand += ")";
            return sqlCommand;
        }
    }
}
