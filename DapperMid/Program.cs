using DapperMid.Crud;
using DapperMid.DataTables;
using DapperMid.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DapperMid
{
    static class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=DapperTest;Integrated Security=True;"; //Wrong Way
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var personTable = new Crud<Person>(conn)
                            .Include(x => x.Adress)
                            .Include(x => x.PersonCard)
                            .Include(x => x.PersonSecret)
                            .Include(x => x.PersonSecret.SecretToken);
            var adress = new Address("' OR 1=1 --");
            var personCard = new PersonCard("123");
            var secretToken = new SecretToken("Token");
            var personSecret = new PersonSecret("has none", secretToken);
            var person = new Person("ads", adress, personSecret, personCard);
            var result = personTable.Insert(person);
            Console.WriteLine($"{result} ");
             IEnumerable<Person> data = personTable.Select(top: 100000);
            ////Console.WriteLine(personCtor.Min(x => x.Id));
            //Console.WriteLine(personTable?.Count());
            conn.Close();
            Console.ReadKey();
        }
    }
}
