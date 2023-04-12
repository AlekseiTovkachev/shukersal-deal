namespace shukersal_backend.Models
{
    public class Store
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long RootManagerId { get; set; }
        public StoreManager RootManager { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<DiscountRule> DiscountRules { get; set; }
    }
}
