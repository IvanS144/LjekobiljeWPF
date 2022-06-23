using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class BankAccount
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }

        public virtual CooperantBankAccount CooperantBankAccount { get; set; }
    }
}
