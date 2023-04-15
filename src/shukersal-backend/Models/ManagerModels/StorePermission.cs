using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{

    public enum PermissionType
    {
        Manager_permission = 0,
        Manage_products_permission = 1,
        Manage_discounts_permission = 2,
        Manage_limits_permission = 3,
        Appoint_owner_permission = 4,
        Remove_owner_permission = 5,
        Appoint_manager_permission = 6,
        Edit_manager_permissions_permission = 7,
        Remove_manager_permission = 8,
        Get_manager_info_permission = 11,
        Reply_permission = 12,
        Get_history_permission = 13
    }

    public class StorePermission
    {
        public long Id { get; set; }
        public PermissionType PermissionType { get; set; }
        public long StoreManagerId { get; set; }
        [JsonIgnore]
        public StoreManager StoreManager { get; set; }

        public StorePermission() { }
    }
}
