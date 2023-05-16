namespace shukersal_backend.Models
{
    public class TransactionItem
    {
 
        // TODO: Check if needed to connect discounts applied 
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long ProductId { get; set; }
        public long StoreId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }

        public int Quantity { get; set; }
        public double FullPrice { get; set; }
        public double FinalPrice { get; set; }

        public TransactionItem() { }

        public TransactionItem(long transactionId, ShoppingItem item) {
            TransactionId = transactionId;
            ProductId = item.ProductId;
            StoreId = item.Product.StoreId;
            ProductName = item.Product.Name;
            ProductDescription = item.Product.Description;
            Quantity = item.Quantity;
            FullPrice = item.Product.Price;
            FinalPrice = item.Product.Price;
        }
        
    }
}
