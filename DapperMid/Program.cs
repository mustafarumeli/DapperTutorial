using System;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Linq.Expressions;

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

        public static Ctor<T> Include<T, P>(this Ctor<T> input, Expression<Func<T, P>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            PropertyInfo prop;
            if (property.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExp)
            {
                prop = (PropertyInfo)memberExp.Member;
                input.FkList.Add(prop.PropertyType);
                input.Props.Add(prop);
            }
            else if (property.Body is MemberExpression pMemberExp)
            {
                prop = (PropertyInfo)pMemberExp.Member;
                input.FkList.Add(prop.PropertyType);
                input.Props.Add(prop);
            }

            return input;
        }
    }


    public class Ctor<T>
    {
        public List<Type> FkList = new List<Type>();
        public List<PropertyInfo> Props = new List<PropertyInfo>();
        SqlConnection _db;
        public Ctor(SqlConnection db)
        {
            this._db = db;
            FkList.Add(typeof(T));
        }
        /// <summary>
        /// Uses Refelection to create a select command 
        /// uses Joins if neccessary
        /// </summary>
        /// <param name="classType">To Find Base Table Name </param>
        /// <param name="joinDirection">This will indicate the direction of join default value is "inner"</param>
        /// <returns>select command as string</returns>
        string GetSelectWithJoin(string joinDirection)
        {
            var fKType = typeof(ForeignKeyAttribute);
            string command = "Select * from " + typeof(T).Name;
            foreach (var prop in Props)
            {
                CustomAttributeData fkAttr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == fKType);
                string fkName = fkAttr.ConstructorArguments[0].Value.ToString();
                string currentTableName = prop.PropertyType.Name;
                command += $" {joinDirection} Join {currentTableName} on {prop.ReflectedType.Name}.{fkName} = {currentTableName}.Id";
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
        public IEnumerable<T> Select(string whereClause = null, string joinDirection = "inner")
        {
            string command = GetSelectWithJoin(joinDirection);
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                command += " " + whereClause;
            }
            return _db.Query(command, FkList.ToArray(),
              objects =>
              {
                  foreach (PropertyInfo prop in Props)
                  {
                      object valueObject = objects.First(x => x.GetType() == prop.PropertyType);
                      object propObject = objects.First(x => x.GetType() == prop.DeclaringType);
                      prop.SetValue(propObject, valueObject);
                  }
                  return (T)objects[0];
              });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=DapperTest;Integrated Security=True;"; //Wrong Way
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var pctor = new Ctor<Person>(conn)
                          .Include(x => x.Adress)
                          .Include(x => x.PersonCard)
                          .Include(x => x.PersonSecret)
                          .Include(x => x.PersonSecret.SecretToken);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<Person> data = pctor.Select();
            Console.WriteLine(data.Count());
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
