using System;
using Dapper;
namespace DapperTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            var dapperTableCrud = CrudFactory.DapperTableCrud;
            DapperTable entity = new DapperTable { Name = "Mustafa", BirthDate = new DateTime(1995, 03, 21) };
            int resIns = dapperTableCrud.Insert(entity);
            var list = dapperTableCrud.List;
            int resDel = dapperTableCrud.Delete(list[0].Id);
            int resUpt = dapperTableCrud.Update(entity);
            foreach (DapperTable item in dapperTableCrud.List)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(resDel + "<= Delete " + resIns + "<= Insert " + resUpt + "<= Update");

            Console.ReadKey();
        }
    }
}
