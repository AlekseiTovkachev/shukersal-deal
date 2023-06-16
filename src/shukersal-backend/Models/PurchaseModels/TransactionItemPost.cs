namespace shukersal_backend.Models.PurchaseModels
{
    public class TransactionItemPost
    {
        public long ProductId { get; set; }
        public long StoreId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }

        public int Quantity { get; set; }
        public double FullPrice { get; set; }
    }
}
