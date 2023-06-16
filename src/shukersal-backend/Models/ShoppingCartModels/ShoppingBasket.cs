using System.Text.Json.Serialization;

namespace shukersal_backend.Models;

public class ShoppingBasket
{
    public long Id { get; set; }
    public long ShoppingCartId { get; set; }
    public long StoreId { get; set; }
    public ICollection<ShoppingItem> ShoppingItems { get; set; }
}
