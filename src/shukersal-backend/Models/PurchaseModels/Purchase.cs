using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class Purchase
    {
        public Purchase() { PurchaseItems = new List<PurchaseItem>(); }

        // TODO: Connect billing/delivery service via invoice id / delivery id etc.
        [Key]
        public long Id { get; set; }
        [Required]
        public long Member_Id_ { get; set; }
       // [JsonIgnore]
        //[ForeignKey("Member_Id_")]
        //[Required]
       // public Member? Member_ { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
