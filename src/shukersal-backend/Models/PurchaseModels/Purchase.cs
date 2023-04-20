using System.ComponentModel.DataAnnotations.Schema;

namespace shukersal_backend.Models
{
    public class Purchase
    {
        public Purchase() { PurchaseItems = new List<PurchaseItem>(); }

        // TODO: Connect billing/delivery service via invoice id / delivery id etc.
        public long Id { get; set; }
        public long MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
