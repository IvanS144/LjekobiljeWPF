using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class Cooperant
    {
        public Cooperant()
        {
            CooperantBankAccounts = new HashSet<CooperantBankAccount>();
            PlantPurchases = new HashSet<PlantPurchase>();
        }

        public int CooperantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int StationId { get; set; }
        public string City { get; set; }

        public virtual Station Station { get; set; }
        public virtual ICollection<CooperantBankAccount> CooperantBankAccounts { get; set; }
        public virtual ICollection<PlantPurchase> PlantPurchases { get; set; }
    }
}
