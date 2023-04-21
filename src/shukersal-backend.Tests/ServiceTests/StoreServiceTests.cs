using shukersal_backend.Domain;
using shukersal_backend.Models;
using System.Net;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.ServiceTests
{
    public class StoreServiceTests
    {
        private readonly Mock<MemberContext> _memberContextMock;
        private readonly Mock<StoreContext> _storeContextMock;
        private readonly Mock<ManagerContext> _managerContextMock;
        private readonly StoreService _service;
        private readonly ITestOutputHelper output;

        public StoreServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            _memberContextMock = new Mock<MemberContext>();
            _storeContextMock = new Mock<StoreContext>();
            _managerContextMock = new Mock<ManagerContext>();
            //_storeContextMock.Setup(c => c.Database.EnsureCreated()).Returns(true);
            _service = new StoreService(_storeContextMock.Object, _managerContextMock.Object, _memberContextMock.Object, "test");
        }

        [Fact]
        public async Task GetStores_ReturnsListOfStores()
        {
            // Arrange
            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            _storeContextMock.Setup(c => c.Stores).ReturnsDbSet(stores);


            // Act
            var response = await _service.GetStores();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, response.Result.Count());
            Assert.Equal("Store 1", response.Result.ElementAt(0).Name);
            Assert.Equal("Store 2", response.Result.ElementAt(1).Name);
            Assert.Equal("Store 3", response.Result.ElementAt(2).Name);
        }

        [Fact]
        public async Task GetStore_WithExistingStore_ReturnsSuccessResponseWithStore()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Description 1", RootManagerId = 1 };
            var stores = new List<Store>
            {
                store,
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();
            _storeContextMock.Setup(c => c.Stores).ReturnsDbSet(stores);

            // Act
            var response = await _service.GetStore(store.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(store, response.Result);
        }

        [Fact]
        public async Task GetStore_WithNonExistingStore_ReturnsErrorResponseWithNotFoundStatusCode()
        {
            // Arrange
            long nonExistingStoreId = 1;
            _storeContextMock.Setup(c => c.Stores).ReturnsDbSet(new List<Store>().AsQueryable());

            // Act
            var response = await _service.GetStore(nonExistingStoreId);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(response.Result);
        }

        [Fact]
        public async Task CreateStore_ValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var storeData = new StorePost
            {
                Name = "Test Store",
                Description = "This is a test store.",
                RootManagerMemberId = 1
            };
            var membersList = new List<Member>
            {
                new Member { Id = storeData.RootManagerMemberId }
            }.AsQueryable();

            _memberContextMock.Setup(m => m.Members).ReturnsDbSet(membersList);
            var member = new Member { Id = storeData.RootManagerMemberId };
            _memberContextMock.Setup(m => m.Members.FindAsync(storeData.RootManagerMemberId)).ReturnsAsync(member);
            _storeContextMock.Setup(c => c.Stores).ReturnsDbSet(new List<Store>().AsQueryable());
            _managerContextMock.Setup(c => c.StoreManagers).ReturnsDbSet(new List<StoreManager>().AsQueryable());
            // Act
            var response = await _service.CreateStore(storeData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(storeData.Name, response.Result.Name);
            Assert.Equal(storeData.Description, response.Result.Description);
            Assert.Equal(storeData.RootManagerMemberId, response.Result.RootManagerId);
        }

        [Fact]
        public async Task CreateStore_InvalidMemberId_ReturnsErrorResponse()
        {
            // Arrange
            var storeData = new StorePost
            {
                Name = "Test Store",
                Description = "This is a test store.",
                RootManagerMemberId = 1
            };
            _memberContextMock.Setup(m => m.Members).ReturnsDbSet(new List<Member>().AsQueryable());
            // Act
            var response = await _service.CreateStore(storeData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(response.Result);
        }



    }
}
