using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public enum DiscountRuleBooleanType
    {
        AND = 0,
        OR = 1,
        CONDITION = 2,
        PRODUCT_AT_LEAST = 3,
        PRODUCT_LIMIT = 4,
        CATEGORY_AT_LEAST = 5,
        CATEGORY_LIMIT = 6,
        TIME_HOUR_AT_DAY = 7,
        TIME_DAY_AT_WEEK = 8
    }
    public class DiscountRuleBoolean
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public DiscountRuleBooleanType discountRuleBooleanType { get; set; }
        public virtual ICollection<DiscountRuleBoolean>? Components { get; set; }
        [JsonIgnore]
        public Store store { get; set; }
        public string conditionString { get; set; }
        public int conditionLimit { get; set; }
        [Range(0, 24)]
        public int minHour { get; set; }
        [Range(0, 24)]
        public int maxHour { get; set; }
        //public bool[] weekDays { get; set; }
    }
}
