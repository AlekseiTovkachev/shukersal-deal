using FluentAssertions;
using FluentAssertions.Common;
using shukersal_backend.Domain;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using Xunit.Abstractions;


namespace shukersal_backend.Tests.ServiceTests
{
    public class PurchaseServiceTests
    {
        private readonly PurchaseService _purchaseService;
        private readonly Mock<MarketDbContext> _context;
        private readonly ITestOutputHelper _output;

        public PurchaseServiceTests(ITestOutputHelper output)
        {
            _context = new Mock<MarketDbContext>();
            _output = output;
            _purchaseService = new PurchaseService(_context.Object);
        }



        [Fact]
        public async Task GetPurchases()
        {
 

        var purchases=new List<Purchase>
            {

                new Purchase { Id = 1, Member_Id_=1, TotalPrice=30},
                new Purchase { Id = 2, Member_Id_ = 1, TotalPrice=20},
                new Purchase { Id = 3, Member_Id_=2 , TotalPrice=40}
            }.AsQueryable();

             _context.Setup(c => c.Purchases).ReturnsDbSet(purchases);

            var response = await _purchaseService.GetPurchases();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, response.Result.Count());
            Assert.Equal(1, response.Result.ElementAt(0).Id);
            Assert.Equal(2, response.Result.ElementAt(1).Id);
            Assert.Equal(3, response.Result.ElementAt(2).Id);

        }

        [Fact]
        public async Task GetPurchase()
        {

            var purchases = new List<Purchase>
            {
                new Purchase { Id = 1, Member_Id_=1, TotalPrice=30},
                new Purchase { Id = 2, Member_Id_ = 1, TotalPrice=20},
                new Purchase { Id = 3, Member_Id_=2 , TotalPrice=40}
            }.AsQueryable();

            _context.Setup(c => c.Purchases).ReturnsDbSet(purchases);

            //Existing purchase
            var response = await _purchaseService.GetPurchase(1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(1, response.Result.Id);

            //Non existing purchase
            var response2 = await _purchaseService.GetPurchase(5);
            Assert.Equal(HttpStatusCode.NotFound,response2.StatusCode);

        }

        [Fact]
        public async Task BrowesePurchaseHistory()
        {

            var purchases = new List<Purchase>
            {
                new Purchase { Id = 1, Member_Id_=1, TotalPrice=30},
                new Purchase { Id = 2, Member_Id_ = 1, TotalPrice=20},
            }.AsQueryable();

          
            _context.Setup(c => c.Purchases).ReturnsDbSet(purchases);
            //A member with previous purchases 
             var response = await _purchaseService.BrowesePurchaseHistory(1);
             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
             Assert.NotNull(response.Result);
             Assert.Equal(2, response.Result.Count());
            
            //A member without previous purchases 
             var response2 = await _purchaseService.BrowesePurchaseHistory(2);
             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
             Assert.NotNull(response2.Result);
             Assert.Empty(response2.Result);
        }


        [Fact]
        public async Task BroweseShopPurchaseHistory()
        {
            var membersList = new List<Member>
            {
                new Member
                {
                    Id = 1,
                    Username = "Test",
                    PasswordHash = "password"
                },
            };

            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
            }.AsQueryable();


            var purchases = new List<Purchase>
            {
                new Purchase { Id = 1, Member_Id_=1, TotalPrice=30},
                new Purchase { Id = 2, Member_Id_ = 1, TotalPrice=20},
            }.AsQueryable();



            _context.Setup(c => c.Stores).ReturnsDbSet(stores);


            _context.Setup(m => m.Members).ReturnsDbSet(membersList);

            var items = new List<PurchaseItem>
            {
                new PurchaseItem{ 
                    Id=1,
                    PurchaseId=1,
                    ProductId=1,
                    StoreId=1,

                },

                 new PurchaseItem{
                    Id=2,
                    PurchaseId=2,
                    ProductId=1,
                    StoreId=1,

                },

            };

            _context.Setup(c => c.Purchases).ReturnsDbSet(purchases);
            _context.Setup(c => c.PurchaseItems).ReturnsDbSet(items);

            var response = await _purchaseService.BroweseShopPurchaseHistory(1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(2, response.Result.Count());


            var response2 = await _purchaseService.BroweseShopPurchaseHistory(2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);

        }

    }



}
