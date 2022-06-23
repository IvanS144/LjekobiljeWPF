using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class CooperantBankAccount
    {
        public CooperantBankAccount()
        {
            Payments = new HashSet<Payment>();
        }

        public int AccountId { get; set; }
        public int CooperantId { get; set; }

        public virtual BankAccount Account { get; set; }
        public virtual Cooperant Cooperant { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
