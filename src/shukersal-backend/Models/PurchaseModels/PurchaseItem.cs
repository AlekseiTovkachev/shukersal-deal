namespace shukersal_backend.Models
{
    public class PurchaseItem
    {
 
        // TODO: Check if needed to connect discounts applied 
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        //public Purchase Purchase { get; set; }
        public long ProductId { get; set; }
        //public Product Product { get; set; }
        public long StoreId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
