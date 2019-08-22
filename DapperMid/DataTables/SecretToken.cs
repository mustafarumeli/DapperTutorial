using System;

namespace DapperMid.DataTables
{
    class SecretToken : DataTable
    {
        public SecretToken(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public string Token { get; set; }
    }
}
