using Azure;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Utility;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using Xunit.Abstractions;


namespace shukersal_backend.Tests.ServiceTests.TransactionUnitTests
{
    public class TransactionServiceTests
    {
        protected readonly TransactionController _TransactionController;
        protected readonly Mock<MarketDbContext> _context;
        protected readonly ITestOutputHelper _output;
        public TransactionServiceTests(ITestOutputHelper output){
            _context = new Mock<MarketDbContext>();
            _output = output;
            _TransactionController = new TransactionController(_context.Object);

        }


        [Fact]
        public async Task GetTransactions()
        {
            var Transactions = new List<Transaction>
            {

                new Transaction { Id = 1, MemberId = 1, TotalPrice = 30 },
                new Transaction { Id = 2, MemberId = 1, TotalPrice = 20 },
                new Transaction { Id = 3, MemberId = 2, TotalPrice = 40 }
            };

            _context.Setup(x => x.Transactions).ReturnsDbSet(Transactions);

            var response = await _TransactionController.GetTransactions();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, response.Result.Count());
            Assert.Equal(1, response.Result.ElementAt(0).Id);
            Assert.Equal(2, response.Result.ElementAt(1).Id);
            Assert.Equal(3, response.Result.ElementAt(2).Id);


        }

        [Fact]
        public async Task CheckPaymentPatterns()
        {

            TransactionPost transaction = new TransactionPost
            {
                MemberId = 1,
                TransactionDate = new DateTime(),
                TotalPrice = 130,
                HolderFirstName = "Holder",

                HolderLastName = "Holder",
                HolderID = "313574357",
                CardNumber = "1111111111111111",
                expirationDate = new DateOnly(),
                CVC = "123",
                ReceiverFirstName = "Holder",
                ReceiverLastName = "Holder",

                ReceiverPhoneNum = "0506255065",

                ReceiverAddress = "Holder",
                ReceiverPostalCode = "2804601"
            };


            Assert.True(_TransactionController.getPaymentService().ConfirmPayment(new PaymentDetails(transaction)));
            _TransactionController.getPaymentService().SetProxyAnswer(false);
            Assert.False(_TransactionController.getPaymentService().ConfirmPayment(new PaymentDetails(transaction)));

            Assert.True(_TransactionController.getDeliveryService().ConfirmDelivery(new DeliveryDetails(transaction, new Dictionary<long, List<TransactionItem>>())));
            _TransactionController.getDeliveryService().SetProxyAnswer(false);
            Assert.False(_TransactionController.getDeliveryService().ConfirmDelivery(new DeliveryDetails(transaction, new Dictionary<long, List<TransactionItem>>())));

        }

        [Fact]
        public async Task GetTransaction()
        {
            var Transactions = new List<Transaction>
            {

                new Transaction { Id = 1, MemberId = 1, TotalPrice = 30 },
                new Transaction { Id = 2, MemberId = 1, TotalPrice = 20 },
                new Transaction { Id = 3, MemberId = 2, TotalPrice = 40 }
            };

            _context.Setup(x => x.Transactions).ReturnsDbSet(Transactions);

            //Existing Transaction
            var response = await _TransactionController.GetTransaction(1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(1, response.Result.Id);

            //Non existing Transaction
            var response2 = await _TransactionController.GetTransaction(5);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);

        }

        [Fact]
        public async Task BrowseTransactionHistory()
        {
            var membersList = new List<Member>
            {
                new Member
                {
                    
                    Username = "Test",
                    PasswordHash = HashingUtilities.HashPassword("ppsskln"),
                    Role="member"
                },

                new Member
                {
                   
                    Username = "Test2",
                    PasswordHash = HashingUtilities.HashPassword("ppsskln"),
                    Role="member"
                },
            };

            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
            };



            var productList = new List<Product>
            {
                new Product { Name = "product",
                Description = "Desc",
                Price = 123,
                ImageUrl = "www.bla.com",
                IsListed = true,

                UnitsInStock = 10,
                StoreId = 1 }
            };



            var shoppingCarts = new List<ShoppingCart>
            { new ShoppingCart{
                Member = membersList[0],
                ShoppingBaskets = new List<ShoppingBasket>()
            }
            };



            var baskets = new List<ShoppingBasket> {
                new ShoppingBasket { ShoppingCartId = shoppingCarts[0].Id, ShoppingItems = new List<ShoppingItem>() }
        };
            var shoppingItems = new List<ShoppingItem> {
                new ShoppingItem {
                ShoppingBasketId = baskets[0].Id,
                ShoppingBasket = baskets[0],
                ProductId = 1,
                Product = productList[0],
                Quantity = 2

            }
        };

            var Transactions = new List<Transaction>
            {

                new Transaction { MemberId = 1, TotalPrice = 30 },
                new Transaction { MemberId = 1, TotalPrice = 20 },
            };

            var items = new List<TransactionItem>
            {
                new TransactionItem{
                    Id=1,
                    TransactionId=1,
                    ProductId=1,
                    StoreId=1,

                },

                 new TransactionItem{
                    Id=2,
                    TransactionId=2,
                    ProductId=1,
                    StoreId=1,

                },

            };

            _context.Setup(x => x.Members).ReturnsDbSet(membersList);
             _context.Setup(x => x.Stores).ReturnsDbSet(stores);
            _context.Setup(x => x.Products).ReturnsDbSet(productList);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(shoppingCarts);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(baskets);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(shoppingItems);
            _context.Setup(x => x.Transactions).ReturnsDbSet(Transactions);
             _context.Setup(x => x.TransactionItems).ReturnsDbSet(items);



            //A member with previous Transactions 
            var response = await _TransactionController.BrowseTransactionHistory(1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(2, response.Result.Count());

            //A member without previous Transactions 
            var response2 = await _TransactionController.BrowseTransactionHistory(2);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);
        }


        [Fact]
        public async Task BrowseShopTransactionHistory()
        {

            var membersList = new List<Member>
            {
                new Member
                {
                    Id=1,
                    Username = "Test",
                    PasswordHash = HashingUtilities.HashPassword("ppsskln"),
                    Role="member"
                },

                new Member
                {
                    Id = 2,
                    Username = "Test2",
                    PasswordHash = HashingUtilities.HashPassword("ppsskln"),
                    Role="member"
                },
            };

            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
            };



            var productList = new List<Product>
            {
                new Product { Name = "product",
                Description = "Desc",
                Price = 123,
                ImageUrl = "www.bla.com",
                IsListed = true,

                UnitsInStock = 10,
                StoreId = 1 }
            };



            var shoppingCarts = new List<ShoppingCart>
            { new ShoppingCart{
                Member = membersList[0],
                ShoppingBaskets = new List<ShoppingBasket>()
            }
            };



            var baskets = new List<ShoppingBasket> {
                new ShoppingBasket { ShoppingCartId = shoppingCarts[0].Id, ShoppingItems = new List<ShoppingItem>() }
        };
            var shoppingItems = new List<ShoppingItem> {
                new ShoppingItem {
                ShoppingBasketId = baskets[0].Id,
                    ShoppingBasket = baskets[0],
                ProductId = 1,
                Product = productList[0],
                Quantity = 2

            }
        };

            var Transactions = new List<Transaction>
            {

                new Transaction { Id = 1, MemberId = 1, TotalPrice = 30 },
                new Transaction { Id = 2, MemberId = 1, TotalPrice = 20 },
                new Transaction { Id = 3, MemberId = 2, TotalPrice = 40 }
            };



            var items = new List<TransactionItem>
            {
                new TransactionItem{
                    Id=1,
                    TransactionId=1,
                    ProductId=1,
                    StoreId=1,

                },

                 new TransactionItem{
                    Id=2,
                    TransactionId=2,
                    ProductId=1,
                    StoreId=1,

                },

            };

            _context.Setup(x => x.Members).ReturnsDbSet(membersList);
            _context.Setup(x => x.Stores).ReturnsDbSet(stores);
            _context.Setup(x => x.Products).ReturnsDbSet(productList);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(shoppingCarts);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(baskets);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(shoppingItems);
            _context.Setup(x => x.Transactions).ReturnsDbSet(Transactions);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(items);

            var response = await _TransactionController.BrowseShopTransactionHistory(1);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(2, response.Result.Count());


            var response2 = await _TransactionController.BrowseShopTransactionHistory(2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);

        }

    }

}