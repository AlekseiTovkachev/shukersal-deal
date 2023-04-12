using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class ManagerContext : DbContext
    {
        public ManagerContext(DbContextOptions<ManagerContext> options)
            : base(options)
        {

        }

        public DbSet<StoreManager> StoreManagers { get; set; } = null!;
        public DbSet<StorePermission> StorePermissions { get; set; } = null!;


    }
}
