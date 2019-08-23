using DapperMid.Attributes;
using System;

namespace DapperMid.DataTables
{
    class Person : Datatable
    {
        public Person()
        {

        }
        public Person(string name, Address adress, PersonSecret personSecret, PersonCard personCard)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Adress = adress ?? throw new ArgumentNullException(nameof(adress));
            PersonSecret = personSecret ?? throw new ArgumentNullException(nameof(personSecret));
            PersonCard = personCard ?? throw new ArgumentNullException(nameof(personCard));
        }

        public string Name { get; set; }
        [ForeignKey("Address_Id")]
        public Address Adress { get; set; }
        [ForeignKey("Secret_Id")]
        public PersonSecret PersonSecret { get; set; }
        [ForeignKey("Card_Id")]
        public PersonCard PersonCard { get; set; }
    }
}
