namespace shukersal_backend.Models.ShoppingCartModels
{
    public class ShoppingItemPost
    {
        //public long CartId { get; set; }
        public long ProductId { get; set; }
        public long StoreId { get; set; }
        public int Quantity { get; set; }

    }
}
