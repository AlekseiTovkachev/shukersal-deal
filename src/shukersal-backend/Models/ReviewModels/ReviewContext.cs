using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options)
            : base(options)
        {

        }

        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
    }
}
