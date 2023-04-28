using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Controllers
{
    public abstract class AbstractController
    {
        protected readonly MarketDbContext _context;

        public AbstractController(MarketDbContext context)
        {
            _context = context;
            // _context.Database.EnsureCreated();
        }
    }
}
