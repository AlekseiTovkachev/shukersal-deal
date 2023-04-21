namespace shukersal_backend.Models
{
    public class ManagerContext : DbContext
    {
        public ManagerContext() { }
        public ManagerContext(DbContextOptions<ManagerContext> options)
            : base(options)
        {

        }

        public virtual DbSet<StoreManager> StoreManagers { get; set; } = null!;
        public virtual DbSet<StorePermission> StorePermissions { get; set; } = null!;


    }
}



