using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class Purchasespaid
    {
        public int PlantPurchaseId { get; set; }
        public DateTime DatePurchased { get; set; }
        public int StationId { get; set; }
        public int CooperantId { get; set; }
        public string CooperantName { get; set; }
        public decimal? TotalValue { get; set; }
        public int? Status { get; set; }
    }
}
