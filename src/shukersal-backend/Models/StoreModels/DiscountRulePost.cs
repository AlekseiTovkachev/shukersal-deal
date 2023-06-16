using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class DiscountRulePost
    {
        //[Required]
        //public long Id { get; set; }
        [Required]
        public DiscountType discountType { get; set; }
        public double Discount { get; set; } // in percentage, 20 means x(1-(20/100)) calculation
        //public DiscountRuleBoolean? discountRuleBoolean { get; set; }
        public DiscountOn discountOn { get; set; }
        public string? discountOnString { get; set; }
        public long StoreId { get; set; }
    }
}
