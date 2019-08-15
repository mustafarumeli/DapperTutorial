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
        [ForeignKey("SecretToken_Id")]
        public SecretToken SecretToken { get; set; }
    }
    class Address : DataTable
    {
        public string Desc { get; set; }
    }
    class PersonCard : DataTable
    {
        public string CardNo { get; set; }
    }
    class SecretToken : DataTable
    {
        public string Token { get; set; }
    }
    #endregion
    #region Enums 
    /// <summary>
    /// Lazy is just Select * from Table 
    /// BaseOnly looks for ForeignKeys in given class
    /// Full searches every ForeignKey recuresively for possible foreignKeys 
    /// </summary>
    enum SelectType
    {

        Lazy,
        BaseOnly,
        Full
    }
    #endregion
    public static class Maybe
    {
        public static void Do<T>(this IEnumerable<T> input, Action<T> eval)
        {
            foreach (T item in input)
            {
                eval(item);
            }
        }
    }
    class Ctor<T>
    {
        List<Type> _fKeyList;
        List<PropertyInfo> _props;
        List<(Type type, PropertyInfo prop)> _test;
        public Ctor()
        {
            _fKeyList = new List<Type>();
            _fKeyList.Add(typeof(T));
            _props = new List<PropertyInfo>();
            _test = new List<(Type type, PropertyInfo prop)>();
        }
        /// <summary>
        /// Uses Refelection to create a select command 
        /// uses Joins if neccessary
        /// </summary>
        /// <param name="classType">To Find Base Table Name </param>
        /// <param name="joinDirection">This will indicate the direction of join default value is "inner"</param>
        /// <param name="selectType">Enum SelectType determines return value's complexity see Enum Definiton for MoreInfo</param>
        /// <returns>select command as string</returns>
        string GetSelectWithJoin(Type classType, string joinDirection, SelectType selectType)
        {
            PropertyInfo[] props = classType.GetProperties();
            var baseType = typeof(DataTable);
            var fKType = typeof(ForeignKeyAttribute);
            string baseName = classType.Name;
            string command = "Select * from " + baseName;
            if (selectType == SelectType.Lazy)
            {
                return command;
            }
            foreach (PropertyInfo prop in props)
            {
                CustomAttributeData fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                if (fkAttr != null)
                {
                    // Add Property to the select statement
                    _props.Add(prop);
                    string fkName = fkAttr.ConstructorArguments[0].Value.ToString();
                    string currentTableName = prop.PropertyType.Name;
                    _fKeyList.Add(prop.PropertyType);
                    command += $" {joinDirection} Join {currentTableName} on {baseName}.{fkName} = {currentTableName}.Id";

                    if (selectType == SelectType.Full)
                    {
                        LookInside(prop.PropertyType);
                    }
                    // Checks Property's type for possible ForeignKeys recursively
                    void LookInside(Type propType)
                    {
                        foreach (var inSideProp in propType.GetProperties())
                        {
                            CustomAttributeData fkAttr = inSideProp.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                            if (fkAttr != null)
                            {
                                _props.Add(inSideProp);
                                string insideFkName = fkAttr.ConstructorArguments[0].Value.ToString();
                                string currentTableName = inSideProp.PropertyType.Name;

                                _fKeyList.Add(inSideProp.PropertyType);
                                command += $" {joinDirection} Join {currentTableName} on {propType.Name}.{insideFkName} = {currentTableName}.Id";
                                LookInside(inSideProp.PropertyType);
                            }
                        }
                    }
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
        /// <param name="selectType">Enum SelectType determines return value's complexity see Enum Definiton for MoreInfo</param>
        /// <returns>An IEnumerable of Given type</returns>
        public IEnumerable<T> Select(SqlConnection con, string whereClause = null, string joinDirection = "inner", SelectType selectType = SelectType.BaseOnly)
        {
            string command = GetSelectWithJoin(typeof(T), joinDirection, selectType);
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                command += " " + whereClause;
            }
            if (selectType == SelectType.Lazy)
            {
                return con.Query<T>(command);
            }
            return con.Query(command, _fKeyList.ToArray(),
                objects =>
             {
                 foreach (PropertyInfo prop in _props)
                 {
                     string name = prop.PropertyType.Name;
                     if (selectType == SelectType.Full)
                     {
                         object valueObject = objects.FirstOrDefault(x => x.GetType().Name == name);
                         objects.Do(
                                obj =>
                                {
                                    PropertyInfo cprop = obj.GetType().GetProperty(prop.Name);
                                    if (cprop != null)
                                    {
                                        cprop.SetValue(obj, valueObject);
                                    }
                                });
                     }
                     else
                     {
                         object valueObject = objects.FirstOrDefault(x => x.GetType().Name == name);
                         prop.SetValue(objects[0], valueObject);
                     }

                 }
                 return (T)objects[0];
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
            // IEnumerable<Person> data = pctor.Select(conn); 
            IEnumerable<Person> data = pctor.Select(conn, selectType: SelectType.BaseOnly);
            // IEnumerable<Person> data = pctor.Select(conn, selectType: SelectType.Full);  <= This Equals code bellow
            //string sql = "Select * from Person inner Join Address on Person.Address_Id = Address.Id inner Join PersonSecret on Person.Secret_Id = PersonSecret.Id inner Join SecretToken on PersonSecret.SecretToken_Id = SecretToken.Id inner Join PersonCard on Person.Card_Id = PersonCard.Id";
            //IEnumerable<Person> data = conn.Query<Person, Address, PersonCard, PersonSecret, SecretToken, Person>(sql,
            //    (p, a, pc, ps, st) =>
            //    {
            //        p.Adress = a;
            //        p.PersonCard = pc;
            //        ps.SecretToken = st;
            //        p.PersonSecret = ps;
            //        return p;
            //    });
            Console.ReadKey();
        }
    }
}
