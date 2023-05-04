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
        private readonly TransactionController _TransactionController;
        private readonly Mock<MarketDbContext> _context;
        private readonly ITestOutputHelper _output;
        private List<Transaction>? transactions;
        private List<Member>? members;
        private List<TransactionPost>? transactionPosts;
        private List<TransactionItem>? transactionItems;
        private List<Store>? stores;
        private List<Product>? products;
        private List<ShoppingCart>? shoppingCarts;
        private List<ShoppingBasket>? shoppingBaskets;
        private List<ShoppingItem>? shoppingItems;
        
        public TransactionServiceTests(ITestOutputHelper output){
            _context = new Mock<MarketDbContext>();
            _output = output;
            _TransactionController = new TransactionController(_context.Object);
            InitData();

        }

        private void InitData()
        {
            transactions= new List<Transaction>
            {
                new Transaction { Id = 1, MemberId = 1, TotalPrice = 30 },
                new Transaction { Id = 2, MemberId = 1, TotalPrice = 20 },
                new Transaction { Id = 3, MemberId = 2, TotalPrice = 40 }
            };

              members = new List<Member>
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

              transactionPosts = new List<TransactionPost>
            {
                new TransactionPost
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
                },
                new TransactionPost
            {
                MemberId =   members[0].Id,
                HolderFirstName = "HolderFirstName",
                HolderLastName = "HolderLastName",
                HolderID = "123456789",
                CardNumber = "1111111111111111",
                ReceiverFirstName= "ReceiverFirstName",
                ReceiverLastName= "ReceiverLastName",
                ReceiverPhoneNum="0500000000",
                ReceiverAddress= "ReceiverAddress",
                ReceiverPostalCode="1234567"
            }

            };

            

              transactionItems= new List<TransactionItem>
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

              stores= new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
            };

              products=new List<Product>
            {
                new Product { Name = "product",
                Description = "Desc",
                Price = 10,
                ImageUrl = "www.bla.com",
                IsListed = true,

                UnitsInStock = 10,
                StoreId = 1 },

                new Product { Name = "product2",
                Description = "Desc",
                Price = 20,
                ImageUrl = "www.bla.com",
                IsListed = true,

                UnitsInStock = 10,
                StoreId = 2 }
            };

              shoppingCarts =new List<ShoppingCart>
            { new ShoppingCart{
                Member =   members[0],
                ShoppingBaskets = new List<ShoppingBasket>()
            }
            };

              shoppingBaskets= new List<ShoppingBasket> {
                new ShoppingBasket {
                    ShoppingCartId =   shoppingCarts[0].Id, ShoppingItems = new List<ShoppingItem>() }
        };

              shoppingItems= new List<ShoppingItem> {
                new ShoppingItem {
                ShoppingBasketId =   shoppingBaskets[0].Id,
                    ShoppingBasket =   shoppingBaskets[0],
                ProductId = 1,
                Product =   products[0],
                Quantity = 2

            },
                new ShoppingItem {
                ShoppingBasketId =   shoppingBaskets[0].Id,
                    ShoppingBasket =   shoppingBaskets[0],
                ProductId = 2,
                Product =   products[0],
                Quantity = 2

            }
        };

        }

        [Fact]
        public async Task GetTransactions()
        {
            _context.Setup(x => x.Transactions).ReturnsDbSet(transactions);

            var response = await _TransactionController.GetTransactions();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(3, response.Result.Count());

        }

        [Fact]
        public async Task CheckPaymentPatterns()
        {

            TransactionPost transaction = transactionPosts.ElementAt(0);
            PaymentDetails paymentDetails = new PaymentDetails(transaction);
            DeliveryDetails deliveryDetails = new DeliveryDetails(transaction, new Dictionary<long, List<TransactionItem>>());

            Assert.True(_TransactionController.getPaymentService().ConfirmPayment(paymentDetails));
            _TransactionController.getPaymentService().SetProxyAnswer(false);
            Assert.False(_TransactionController.getPaymentService().ConfirmPayment(paymentDetails));

            Assert.True(_TransactionController.getDeliveryService().ConfirmDelivery(deliveryDetails));
            _TransactionController.getDeliveryService().SetProxyAnswer(false);
            Assert.False(_TransactionController.getDeliveryService().ConfirmDelivery(deliveryDetails));
        }

        [Fact]
        public async Task GetTransaction()
        {
            _context.Setup(x => x.Transactions).ReturnsDbSet(  transactions);

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
            _context.Setup(x => x.Transactions).ReturnsDbSet(  transactions);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(  transactionItems);

            //A member with previous Transactions 
            var response = await _TransactionController.BrowseTransactionHistory(1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(2, response.Result.Count());

            //A member without previous Transactions 
            var response2 = await _TransactionController.BrowseTransactionHistory(3);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);
        }


        [Fact]
        public async Task BrowseShopTransactionHistory()
        {
            _context.Setup(x => x.Transactions).ReturnsDbSet(  transactions);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(  transactionItems);

            var response = await _TransactionController.BrowseShopTransactionHistory(1);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(2, response.Result.Count());


            var response2 = await _TransactionController.BrowseShopTransactionHistory(2);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);

        }
        
        [Fact]
        public async Task PurchaseAShoppingCart_Success()
        {
            _context.Setup(x => x.Members).ReturnsDbSet(  members);
            _context.Setup(x => x.Stores).ReturnsDbSet(  stores);
            _context.Setup(x => x.Products).ReturnsDbSet(  products);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(  shoppingCarts);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(  shoppingBaskets);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(  shoppingItems);

            var response1 = await _TransactionController.PurchaseAShoppingCart(transactionPosts.ElementAt(1));
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.IsType<Transaction>(response1.Result);
            Assert.Equal(2, response1.Result.TransactionItems.Count);
            Assert.Equal(60, response1.Result.TotalPrice);
        }

    }

}