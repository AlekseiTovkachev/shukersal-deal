using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class ShoppingItem
    {
        public long Id { get; set; }

        public long ShoppingBasketId { get; set; }
        [JsonIgnore]
        public ShoppingBasket ShoppingBasket { get; set; }

        public long ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
