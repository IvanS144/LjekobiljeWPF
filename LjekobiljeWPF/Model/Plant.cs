using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class Plant
    {
        public Plant()
        {
            PlantPurchaseEntries = new HashSet<PlantPurchaseEntry>();
        }

        public int PlantId { get; set; }
        public decimal RetailPrice { get; set; }
        public string PlantName { get; set; }
        public string LatinName { get; set; }
        public string MedicName { get; set; }

        public virtual ICollection<PlantPurchaseEntry> PlantPurchaseEntries { get; set; }
    }
}
