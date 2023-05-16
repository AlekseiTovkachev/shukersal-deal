using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models.StoreModels
{
    public class DiscountRuleBooleanPost
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public DiscountRuleBooleanType discountRuleBooleanType { get; set; }
        public virtual ICollection<DiscountRuleBoolean>? Components { get; set; }
        public long storeId { get; set; }
        public string conditionString { get; set; }
        public int conditionLimit { get; set; }
        [Range(0, 24)]
        public int minHour { get; set; }
        [Range(0, 24)]
        public int maxHour { get; set; }
        //public int weekDays { get; set; }
    }
}
