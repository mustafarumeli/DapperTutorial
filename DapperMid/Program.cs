﻿using DapperMid.Crud;
using DapperMid.DataTables;
using DapperMid.Extensions;
using System;
using System.Data.SqlClient;

namespace DapperMid
{
    internal static class Program
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
            //var adress = new Address("asdqewd-");
            //var personCard = new PersonCard("dvdcxv");
            //var secretToken = new SecretToken("Toke123123n");
            //var personSecret = new PersonSecret("has3123 none", secretToken);
            //var person = new Person("ad3213s", adress, personSecret, personCard);
            //  var result = personTable.Insert(person);
            //Console.WriteLine($"{result} ");
            var deletedCount = personTable.RemeoveWithWhereClause(x => x.Adress.Id == "5" || x.PersonCard.Id == "", null);
            Console.WriteLine(deletedCount);
            //Console.WriteLine(personCtor.Min(x => x.Id));
            // Console.WriteLine(personTable?.Count());
            conn.Close();
            Console.ReadKey();
        }
    }
}
