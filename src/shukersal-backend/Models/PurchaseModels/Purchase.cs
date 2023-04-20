using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class Purchase
    {
        public Purchase() { PurchaseItems = new List<PurchaseItem>(); }

        // TODO: Connect billing/delivery service via invoice id / delivery id etc.
        public long Id { get; set; }

        [ForeignKey("Member_")]
        [JsonIgnore]
        public long Member_Id_ { get; set; }
        [JsonIgnore]
        public Member? Member_ { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
