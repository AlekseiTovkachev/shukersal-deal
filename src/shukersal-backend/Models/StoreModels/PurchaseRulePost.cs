using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models.StoreModels
{
    public class PurchaseRulePost
    {
        //[Required]
        //public long Id { get; set; }
        [Required]
        public PurchaseRuleType purchaseRuleType { get; set; }
        public string? conditionString { get; set; }
        public int conditionLimit { get; set; }
        [Range(0, 24)]
        public int minHour { get; set; }
        [Range(0, 24)]
        public int maxHour { get; set; }
        public long StoreId { get; set; }
        //public int weekDays { get; set; }
    }
}
