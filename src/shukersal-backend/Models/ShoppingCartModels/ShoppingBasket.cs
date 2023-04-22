using System.Text.Json.Serialization;

namespace shukersal_backend.Models;

public class ShoppingBasket
{
    public long Id { get; set; }

    public long ShoppingCartId { get; set; }
    [JsonIgnore]
    public ShoppingCart ShoppingCart { get; set; }

    public long StoreId { get; set; }
    [JsonIgnore]
    public Store Store { get; set; }

    public ICollection<ShoppingItem> ShoppingItems { get; set; }
}
