using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class Store
    {
        public long Id { get; set; }
        [Required]

        public string Name { get; set; }
        public string Description { get; set; }
        public long RootManagerId { get; set; }
        [JsonIgnore]
        [ForeignKey("RootManagerId")]
        public virtual StoreManager? RootManager { get; set; }
        [Required]
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
        [Required]
        public virtual ICollection<DiscountRule> DiscountRules { get; set; }
        public DiscountRule? AppliedDiscountRule { get; set; }
        [Required]
        public virtual ICollection<PurchaseRule> PurchaseRules { get; set; }
        public PurchaseRule? AppliedPurchaseRule { get; set; }

    }
}
