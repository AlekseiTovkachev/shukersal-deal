using shukersal_backend.Models;

namespace shukersal_backend.Domain
{
    public abstract class BaseService
    {
        protected readonly MarketDbContext _context;

        public BaseService(MarketDbContext context)
        {
            _context = context;
            // _context.Database.EnsureCreated();
        }
    }
}
