using System;
using DapperMid.Attributes;

namespace DapperMid.DataTables
{
    class PersonSecret : DataTable
    {
        public PersonSecret(string secret, SecretToken secretToken)
        {
            Secret = secret ?? throw new ArgumentNullException(nameof(secret));
            SecretToken = secretToken ?? throw new ArgumentNullException(nameof(secretToken));
        }

        public string Secret { get; set; }
        [ForeignKey("SecretToken_Id")]
        public SecretToken SecretToken { get; set; }
    }
}
