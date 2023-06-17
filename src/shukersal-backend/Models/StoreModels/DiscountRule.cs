using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shukersal_backend.Models
{
    public enum DiscountType
    {
        SIMPLE = 0,
        CONDITIONAL = 1,
        XOR_MAX = 2,
        XOR_MIN = 3,
        XOR_PRIORRITY = 4,
        MAX = 5,
        ADDITIONAL = 6
    }
    public enum DiscountOn
    {
        STORE = 0,
        CATEGORY = 1,
        PRODUCT = 2
    }

    public class DiscountRule
    {
        // TODO: Implement this with DiscountType
        [Required]
        public long Id { get; set; }
        [Required]
        public DiscountType discountType { get; set; }
        public double Discount { get; set; } // in percentage, 20 means x(1-(20/100)) calculation

        public virtual ICollection<DiscountRule>? Components { get; set; }
        //[InverseProperty("RootDiscountId")]
        public DiscountRuleBoolean? discountRuleBoolean { get; set; }
        public DiscountOn discountOn { get; set; }
        public string? discountOnString { get; set; }

        [JsonIgnore]
        [ForeignKey("StoreId")]
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        public long StoreId { get; set; }

        [JsonIgnore]
        public long? ParentId { get; set; }
    }
}
