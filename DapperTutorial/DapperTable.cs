using DapperCrud;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperTutorial
{
    public class DapperTable : DbTable
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public override string ToString()
        {
            return $"Name = {Name} BirthDate = {BirthDate} Id = {Id}";
        }
    }
}
