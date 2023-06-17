using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shukersal_backend.Models
{
    public enum PurchaseRuleType
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
    public class PurchaseRule
    {
        [Required]
        [Key]
        public long Id { get; set; }
        [Required]
        public PurchaseRuleType purchaseRuleType { get; set; }
        public virtual ICollection<PurchaseRule>? Components { get; set; }
        public string? conditionString { get; set; }
        public int conditionLimit { get; set; }
        [Range(0, 24)]
        public int minHour { get; set; }
        [Range(0, 24)]
        public int maxHour { get; set; }
        //public bool[] weekDays { get; set; }
        [JsonIgnore]
        [ForeignKey("StoreId")]
        public long StoreId { get; set; }

        public bool IsRoot { get; set; }
    }
}
