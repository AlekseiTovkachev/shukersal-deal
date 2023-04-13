using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class Store
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public long RootManagerId { get; set; }
        [Required]
        public StoreManager RootManager { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<DiscountRule> DiscountRules { get; set; }

        public Store(string name, string description, long rootManagerId)
        {
            Name = name;
            Description = description;
            RootManagerId = rootManagerId;
            Products = new List<Product>();
            DiscountRules = new List<DiscountRule>();
        }
    }
}
