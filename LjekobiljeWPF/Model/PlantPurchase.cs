using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class PlantPurchase
    {
        public PlantPurchase()
        {
            Payments = new HashSet<Payment>();
            PlantPurchaseEntries = new HashSet<PlantPurchaseEntry>();
        }

        public int PlantPurchaseId { get; set; }
        public int StationId { get; set; }
        public DateTime DatePurchased { get; set; }
        public int CooperantId { get; set; }
        public decimal? TotalValue { get; set; }

        public virtual Cooperant Cooperant { get; set; }
        public virtual Station Station { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PlantPurchaseEntry> PlantPurchaseEntries { get; set; }
    }
}
