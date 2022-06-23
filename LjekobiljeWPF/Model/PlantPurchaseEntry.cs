using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class PlantPurchaseEntry
    {
        public decimal Quantity { get; set; }
        public decimal RetailPrice { get; set; }
        public int PlantPurchaseId { get; set; }
        public int PlantId { get; set; }

        public virtual Plant Plant { get; set; }
        public virtual PlantPurchase PlantPurchase { get; set; }
    }
}
