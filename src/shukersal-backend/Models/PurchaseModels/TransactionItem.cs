namespace shukersal_backend.Models
{
    public class TransactionItem
    {
 
        // TODO: Check if needed to connect discounts applied 
        public long Id { get; set; }
        public long TransactionId { get; set; }
        //public Transaction Transaction { get; set; }
        public long ProductId { get; set; }
        //public Product Product { get; set; }
        public long StoreId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public int Quantity { get; set; }
        public double FullPrice { get; set; }
        public double FinalPrice { get; set; }
    }
}
