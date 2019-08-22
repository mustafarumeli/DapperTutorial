using DapperMid.Attributes;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperMid.PrototypeClasses
{
    class SelectCommand<T> : IGetSelectCommand where T : DataTable
    {

        /// <summary>
        /// Uses Refelection to create a select command 
        /// uses Joins if neccessary
        /// </summary>
        /// <param name="joinDirection">This will indicate the direction of join default value is "inner"</param>
        /// <returns>select command as string</returns>
        public string GetSelectWithJoin(string joinDirection, bool isDistict, int top, HashSet<PropertyInfo> props)
        {
            var fKType = typeof(ForeignKeyAttribute);
            string command = "Select";

            if (isDistict)
            {
                command += " distinct";
            }
            if (top > -1)
            {
                command += $" Top {top}";
            }
            command += " * from " + typeof(T).Name;
            foreach (var prop in props)
            {
                CustomAttributeData fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                string fkName = fkAttr.ConstructorArguments[0].Value.ToString();
                string currentTableName = prop.PropertyType.Name;
                command += $" {joinDirection} Join {currentTableName} on {prop.ReflectedType.Name}.{fkName} = {currentTableName}.Id";
            }
            return command;
        }
    }
}
