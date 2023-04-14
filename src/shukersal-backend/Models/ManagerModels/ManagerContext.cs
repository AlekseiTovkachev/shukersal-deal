using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class ManagerContext : DbContext
    {
        public readonly int MANAGER_PERMISSION = 0;
        public readonly int MANAGE_PRODUCTS_PERMISSION = 1;
        public readonly int MANAGE_DISCOUNTS_PERMISSION = 2;
        public readonly int MANAGE_LIMITS_PERMISSION = 3;
        public readonly int APPOINT_OWNER_PERMISSION = 4;
        public readonly int REMOVE_OWNER_PERMISSION = 5;
        public readonly int APPOINT_MANAGER_PERMISSION = 6;
        public readonly int EDIT_MANAGER_PERMISSIONS_PERMISSION = 7;
        public readonly int REMOVE_MANAGER_PERMISSION = 8;
        public readonly int GET_MANAGER_INFO_PERMISSION = 11;
        public readonly int REPLY_PERMISSION = 12;
        public readonly int GET_HISTORY_PERMISSION = 13;
        public ManagerContext(DbContextOptions<ManagerContext> options)
            : base(options)
        {

        }

        public DbSet<StoreManager> StoreManagers { get; set; } = null!;
        public DbSet<StorePermission> StorePermissions { get; set; } = null!;

        public StoreManager? SearchManager(Store store, Member member)
        {
            foreach (StoreManager manager in StoreManagers)
                if (manager.Member == member && manager.Store == store)
                    return manager;
            return null;
        }

        public bool CheckPermission(StoreManager? manager, int permissionType)
        {
            if (manager == null)
                return false;
            foreach (StorePermission permission in manager.StorePermissions)
                if (permission.PermissionType == MANAGER_PERMISSION || permission.PermissionType == permissionType)
                    return true;
            return false;
        }

        public bool CheckPermission(Store store, Member member, int permissionType)
        {
            return CheckPermission(SearchManager(store, member), permissionType);
        }

    }
    
}
