using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        [JsonIgnore]
        public Member Member { get; set; }
        public ShoppingCart() { }
        public ICollection<ShoppingBasket> ShoppingBaskets { get; set; }
    }
}
