using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using Xunit.Abstractions;


namespace shukersal_backend.Tests.ServiceTests
{
    internal class PurchaseServiceTests
    {
        private readonly DomainLayer.Controllers.PurchaseLogic _purchaseService;
        private readonly Mock<MarketDbContext> _context;
        private readonly ITestOutputHelper _output;

        public PurchaseServiceTests(ITestOutputHelper output)
        {
            _context = new Mock<MarketDbContext>();
            _output = output;
            _purchaseService = new DomainLayer.Controllers.PurchaseLogic(_context.Object);
        }
    }
}
