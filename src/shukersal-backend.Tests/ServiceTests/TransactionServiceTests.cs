using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Utility;
using System.Net;
using Xunit.Abstractions;


namespace shukersal_backend.Tests.ServiceTests
{
    internal class TransactionServiceTests
    {
        private readonly Mock<TransactionController> _purchaseControllerMock;
        private readonly Mock<MarketDbContext> _context;
        private readonly ITestOutputHelper _output;

        public TransactionServiceTests(ITestOutputHelper output)
        {
            _context = new Mock<MarketDbContext>();
            _output = output;
            _purchaseControllerMock = new Mock<TransactionController>();


        }
    }
}
