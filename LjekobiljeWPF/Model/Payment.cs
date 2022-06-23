using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public decimal Sum { get; set; }
        public int PlantPurchaseId { get; set; }
        public int CooperantAccountId { get; set; }

        public virtual CooperantBankAccount CooperantAccount { get; set; }
        public virtual PlantPurchase PlantPurchase { get; set; }
    }
}
