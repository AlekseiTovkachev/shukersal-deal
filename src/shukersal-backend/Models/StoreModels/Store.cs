using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Store
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public long? RootManagerId { get; set; }
        [JsonIgnore]
        [ForeignKey("RootManagerId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
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
