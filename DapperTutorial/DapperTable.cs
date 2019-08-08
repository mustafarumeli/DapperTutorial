using System;
using System.Collections.Generic;
using System.Text;

namespace DapperTutorial
{
    public class DapperTable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DapperTable()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return $"Name = {Name} BirthDate = {BirthDate} Id = {Id}";
        }
    }
}
