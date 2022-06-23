using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class Station
    {
        public Station()
        {
            Cooperants = new HashSet<Cooperant>();
            PlantPurchases = new HashSet<PlantPurchase>();
        }

        public int StationId { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }

        public virtual ICollection<Cooperant> Cooperants { get; set; }
        public virtual ICollection<PlantPurchase> PlantPurchases { get; set; }
    }
}
