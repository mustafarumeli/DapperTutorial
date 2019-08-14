using System;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace DapperMid
{
    #region Attributes
    class ForeignKeyAttribute : Attribute
    {
        string _name;

        public ForeignKeyAttribute(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
    #endregion
    #region Database Tables
    class DataTable
    {
        public int Id { get; set; }
    }
    class Person : DataTable
    {
        public string Name { get; set; }
        [ForeignKey("Address_Id")]
        public Address Adress { get; set; }
        [ForeignKey("Secret_Id")]
        public PersonSecret PersonSecret { get; set; }
        [ForeignKey("Card_Id")]
        public PersonCard PersonCard { get; set; }
    }
    class PersonSecret : DataTable
    {
        public string Secret { get; set; }
    }
    class Address : DataTable
    {
        public string Desc { get; set; }
    }
    class PersonCard : DataTable
    {
        public string CardNo { get; set; }
    }
    #endregion

    class Ctor<T>
    {
        List<Type> _fKeyList;
        List<PropertyInfo> _props;
        public Ctor()
        {
            _fKeyList = new List<Type>();
            _fKeyList.Add(typeof(T));
            _props = new List<PropertyInfo>();
        }
        /// <summary>
        /// Uses Refelection to create a select command 
        /// uses Joins if neccessary
        /// </summary>
        /// <param name="classType">To Find Base Table Name </param>
        /// <param name="joinDirection">This will indicate the direction of join default value is "inner"</param>
        /// <returns>select command as string</returns>
        string GetSelectWithJoin(Type classType, string joinDirection = "inner")
        {
            PropertyInfo[] props = classType.GetProperties();
            var baseType = typeof(DataTable);
            var fKType = typeof(ForeignKeyAttribute);
            string baseName = classType.Name;
            string command = "Select * from " + baseName;
            foreach (PropertyInfo prop in props)
            {
                CustomAttributeData fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                if (fkAttr != null)
                {
                    _props.Add(prop);
                    string fkName = fkAttr.ConstructorArguments[0].Value.ToString();
                    string currentTableName = prop.PropertyType.Name;
                    _fKeyList.Add(prop.PropertyType);
                    command += $" {joinDirection} Join {currentTableName} on {baseName}.{fkName} = {currentTableName}.Id";
                }
            }
            return command;
        }

        /// <summary>
        /// Uses Reflection To Map One To One ForeignKeys using Dapper
        /// Automaticly Generetes Select Statement With all values (Select * from table1..)
        /// Automaticly Creates inner joins based on ForeignKeyAttribute
        /// </summary>
        /// <param name="con"> SqlConnection Which will be perform on</param>
        /// <param name="whereClause"> Where clause that will be added at the end of command </param>
        /// <param name="joinDirection">This will indicate the direction of join default value is "inner"</param>
        /// <param name="command">Method Automaticly Generates the sql code but if you want you can use your own command</param>
        /// <returns>An IEnumerable of Given type</returns>
        public IEnumerable<T> Select(SqlConnection con, string whereClause = null, string joinDirection = "inner", string command = null)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                command = GetSelectWithJoin(typeof(T), joinDirection);
            }
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                command += " " + whereClause;
            }
            return con.Query(command, _fKeyList.ToArray(),
                objects =>
             {
                 T returnObject = (T)objects[0];
                 Type currentType = typeof(T);
                 for (int i = 1; i < objects.Length; i++)
                 {
                     object currentObj = objects[i];
                     Type currentObjectType = currentObj.GetType();

                     foreach (PropertyInfo prop in _props)
                     {
                         if (prop.PropertyType == currentObjectType)
                         {
                             currentType.GetProperty(prop.Name).SetValue(returnObject, currentObj);
                         }
                     }
                 }
                 return returnObject;
             }
            );
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=DapperTest;Integrated Security=True;"; //Wrong Way
            var conn = new SqlConnection(connectionString);

            var pctor = new Ctor<Person>();
            IEnumerable<Person> data = pctor.Select(conn); // <= This Equals code bellow

            //string sql = "Select * from Person inner Join Address on Person.Address_Id = Address.Id inner Join PersonSecret on Person.Secret_Id = PersonSecret.Id inner Join PersonCard on Person.Card_Id = PersonCard.Id Where Person.Id = 0";
            //IEnumerable<Person> data = conn.Query<Person, Address, PersonCard, PersonSecret, Person>(sql,
            //    (p, a, pc, ps) =>
            //    {
            //        p.Adress = a;
            //        p.PersonCard = pc;
            //        p.PersonSecret = ps;
            //        return p;
            //    });
            Console.ReadKey();
        }
    }
}
