namespace shukersal_backend.Models
{
    public class PurchaseItem
    {
        private long id1;
        private long id2;
        private double price;

        public PurchaseItem(long id1, Purchase purchase, long id2, Product product, int quantity, double price)
        {
            this.id1 = id1;
            Purchase = purchase;
            this.id2 = id2;
            Product = product;
            Quantity = quantity;
            this.price = price;
        }

        // TODO: Check if needed to connect discounts applied 
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
