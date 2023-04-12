namespace shukersal_backend.Models
{
    public class StorePermission
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public Store Store { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<StoreManager> StoreManagers { get; set; }
    }
}
