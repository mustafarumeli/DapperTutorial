using System;

namespace DapperMid.DataTables
{
    class PersonCard : DataTable
    {
        public PersonCard(string cardNo)
        {
            CardNo = cardNo ?? throw new ArgumentNullException(nameof(cardNo));
        }

        public string CardNo { get; set; }
    }

}
