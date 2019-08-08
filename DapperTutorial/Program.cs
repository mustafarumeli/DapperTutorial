using System;
using Dapper;
namespace DapperTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var dapperCurd = new DapperCrud<DapperTable>();
            dapperCurd.Insert(new DapperTable { Name = "Mustafa", BirthDate = new DateTime(1995, 03, 21) });
            foreach (DapperTable item in dapperCurd.List)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
