using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.Models
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions<MemberContext> options)
            : base(options)
        {

        }

        public DbSet<Member> Members { get; set; } = null!;
    }
}
