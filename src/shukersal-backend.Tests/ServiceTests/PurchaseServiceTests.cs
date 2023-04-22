using shukersal_backend.Domain;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
using shukersal_backend.Utility;
using System.Net;
using Xunit.Abstractions;


namespace shukersal_backend.Tests.ServiceTests
{
    internal class PurchaseServiceTests
    {
        private readonly PurchaseService _purchaseService;
        private readonly Mock<PurchaseContext> _purchaseContextMock;
        private readonly Mock<StoreContext> _storeContextMock;
        private readonly Mock<MemberContext> _memberContextMock;
        private readonly Mock<ShoppingCartContext> _shoppingCartContextMock;
        private readonly ITestOutputHelper _output;

        public PurchaseServiceTests(ITestOutputHelper output)
        {
            _purchaseContextMock= new Mock<PurchaseContext>();
            _storeContextMock = new Mock<StoreContext>();
            _memberContextMock = new Mock<MemberContext>();
            _shoppingCartContextMock = new Mock<ShoppingCartContext>();
            _output = output;
            _purchaseService = new PurchaseService(_purchaseContextMock.Object, _storeContextMock.Object,_memberContextMock.Object,_shoppingCartContextMock.Object);
        }
    }
}
