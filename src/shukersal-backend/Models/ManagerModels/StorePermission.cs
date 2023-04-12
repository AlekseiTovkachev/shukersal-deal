namespace shukersal_backend.Models
{
    public class StorePermission
    {
        public long Id { get; set; }
        public int PermissionType { get; set; }
        public long StoreManagerId { get; set; }
        public StoreManager StoreManager { get; set; }

        public StorePermission(long id, StoreManager storeManager, int permissionType)
        {
            Id = id;
            StoreManagerId = storeManager.Id;
            StoreManager = storeManager;
            PermissionType = permissionType;
        }
    }
}
