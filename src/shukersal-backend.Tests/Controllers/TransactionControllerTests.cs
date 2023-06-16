using Azure;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Utility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using Xunit.Abstractions;


namespace shukersal_backend.Tests.Controllers
{
    public class TransactionControllerTests
    {
        private readonly TransactionController _TransactionController;
        private readonly Mock<MarketDbContext> _context;
        private readonly ITestOutputHelper _output;

        private readonly Mock<TransactionObject> _transactionObject;

        private readonly Mock<MarketObject> _marketObject;
        private readonly Mock<StoreObject> _storeObject;

        private readonly Mock<MemberObject> _memberObject;
        private readonly Mock<StoreManagerObject> _managerObject;

        private List<Transaction>? transactions;
        private List<Member>? members;
        private List<TransactionPost>? transactionPosts;
        private List<TransactionItem>? transactionItems;
        private List<Store>? stores;
        private List<Product>? products;
        private List<ShoppingCart>? shoppingCarts;
        private List<ShoppingBasket>? shoppingBaskets;
        private List<ShoppingItem>? shoppingItems;
        private List<PaymentDetails>? billingDetails;
        private List<DeliveryDetails>? deliveryDetails;
        private List<PurchaseRule>? purchaseRules;
        
        

        public TransactionControllerTests(ITestOutputHelper output)
        {

            _context = new Mock<MarketDbContext>();
            _marketObject = new Mock<MarketObject>(_context.Object);
            _transactionObject = new Mock<TransactionObject>(_context.Object,_marketObject.Object);
            _memberObject = new Mock<MemberObject>(_context.Object);
            _managerObject = new Mock<StoreManagerObject>(_context.Object);
            _storeObject = new Mock<StoreObject>(_context.Object, _marketObject.Object, _managerObject.Object);
            _TransactionController = new TransactionController(_context.Object,  _marketObject.Object,  _transactionObject.Object,  _managerObject.Object,  _memberObject.Object,  _storeObject.Object);
            _managerObject.Setup(x => x.CheckPermission(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PermissionType>()))
                    .ReturnsAsync(true);
            _TransactionController.SetRealDeliveryAdapter("");
            _TransactionController.SetRealpaymentAdapter("");
            _output = output;
            InitData();
        }

        private void InitData()
        {
            transactions = new List<Transaction>
            {
                new Transaction { Id = 1, MemberId = 1, TotalPrice = 30 },
                new Transaction { Id = 2, MemberId = 1, TotalPrice = 20 },
                new Transaction { Id = 3, MemberId = 2, TotalPrice = 40 }
            };

            members = new List<Member>
            {
                new Member
                {
                    Id = 1,
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

            billingDetails = new List<PaymentDetails>
                {
                    new PaymentDetails
                    {
                     HolderFirstName = "Holder",
                    HolderLastName = "Holder",
                    HolderID = "313574357",
                    CardNumber = "1111111111111111",
                    ExpirationDate = new DateOnly(),
                    CVC = "123",
                    },
                     new PaymentDetails
                    {
                         HolderFirstName = "HolderFirstName",
                         HolderLastName = "HolderLastName",
                         HolderID = "123456789",
                         CardNumber = "1111111111111111",
                         ExpirationDate = new DateOnly(),
                         CVC = "123",
                    }
                };

            deliveryDetails = new List<DeliveryDetails>
            {
                new DeliveryDetails
                {
                     ReceiverFirstName = "Holder",
                    ReceiverLastName = "Holder",
                    ReceiverAddress = "Holder",
                    ReceiverCountry="Israel",
                    ReceiverCity="Beer Sheva",
                    ReceiverPostalCode = "2804601"
                },
                new DeliveryDetails
                {
                    ReceiverFirstName= "ReceiverFirstName",
                    ReceiverLastName= "ReceiverLastName",
                    ReceiverAddress= "ReceiverAddress",
                    ReceiverCountry="Israel",
                    ReceiverCity="Beer Sheva",
                    ReceiverPostalCode="1234567"
                }
            };
            transactionItems = new List<TransactionItem>
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
            transactionPosts = new List<TransactionPost>
            {
                new TransactionPost
                {
                    MemberId = 1,
                    TransactionDate = new DateTime(),
                    TotalPrice = 130,
                    BillingDetails=billingDetails.ElementAt(0),
                    DeliveryDetails=deliveryDetails.ElementAt(0),
                },
                new TransactionPost
            {
                MemberId =   members[0].Id,
                TransactionDate = new DateTime(),
                TotalPrice = 130,
                BillingDetails=billingDetails.ElementAt(1),
                DeliveryDetails=deliveryDetails.ElementAt(1),

            }

            };


            stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>(), },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
            };

            products = new List<Product>
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

            shoppingCarts = new List<ShoppingCart>
            { new ShoppingCart{
                Member =   members[0],
                ShoppingBaskets = new List<ShoppingBasket>()
            }
            };

            shoppingBaskets = new List<ShoppingBasket> {
                new ShoppingBasket {
                    ShoppingCartId =   shoppingCarts[0].Id, ShoppingItems = new List<ShoppingItem>() }
        };

            shoppingItems = new List<ShoppingItem> {
                new ShoppingItem {
                ShoppingBasketId =   shoppingBaskets[0].Id,
                    //ShoppingBasket =   shoppingBaskets[0],
                ProductId = 1,
                Product =   products[0],
                Quantity = 2

            },
                new ShoppingItem {
                ShoppingBasketId =   shoppingBaskets[0].Id,
             //       ShoppingBasket =   shoppingBaskets[0],
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
            _TransactionController.SetRealDeliveryAdapter("");
            _TransactionController.SetRealpaymentAdapter("");
            TransactionPost transaction = transactionPosts.ElementAt(0);
            Assert.True(_TransactionController.getPaymentProxy().ConfirmPayment(billingDetails.ElementAt(0)));
            _TransactionController.getPaymentProxy().SetProxyAnswer(false);
            Assert.False(_TransactionController.getPaymentProxy().ConfirmPayment(billingDetails.ElementAt(0)));

            Assert.True(_TransactionController.getDeliveryProxy().ConfirmDelivery(deliveryDetails.ElementAt(0), transactionItems));
            _TransactionController.getDeliveryProxy().SetProxyAnswer(false);
            Assert.False(_TransactionController.getDeliveryProxy().ConfirmDelivery(deliveryDetails.ElementAt(0), transactionItems));
        }

        [Fact]
        public async Task GetTransaction()
        {
            _context.Setup(x => x.Transactions).ReturnsDbSet(transactions);

            //Existing Transaction
            var response = await _TransactionController.GetTransaction(1,1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(1, response.Result.Id);

            //Non existing Transaction
            var response2 = await _TransactionController.GetTransaction(5,1);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);

        }

        [Fact]
        public async Task BrowseTransactionHistory()
        {
            _context.Setup(x => x.Transactions).ReturnsDbSet(transactions);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems);

            //A member with previous Transactions 
            var response = await _TransactionController.BrowseTransactionHistory(1,1);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(2, response.Result.Count());

            //A member without previous Transactions 
            var response2 = await _TransactionController.BrowseTransactionHistory(3,3);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);
        }


        [Fact]
        public async Task BrowseShopTransactionHistory()
        {

            var member1 = new List<Member>
            {
                new Member
                {
                    Id = 1,
                    Username = "Test",
                    PasswordHash = HashingUtilities.HashPassword("ppsskln"),
                    Role = "member"
                }
            }.AsQueryable();


            var stores1 = new List<Store> 
            { 
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },

            }.AsQueryable();

            var storeManager = new List<StoreManager> {
                new StoreManager
                {
                    MemberId = member1.ElementAt(0).Id,
                    StoreId = stores1.ElementAt(0).Id,
                    Store = stores1.ElementAt(0),
                    StorePermissions = new List<StorePermission>()
                },
                new StoreManager
                {
                    MemberId = member1.ElementAt(0).Id,
                    StoreId = stores1.ElementAt(1).Id,
                    Store = stores1.ElementAt(1),
                    StorePermissions = new List<StorePermission>()
                },
            }.AsQueryable();

            var permission = new List<StorePermission>
            {
                new StorePermission
                {
                    StoreManager = storeManager.ElementAt(0),
                    StoreManagerId = storeManager.ElementAt(0).Id,
                    PermissionType = PermissionType.Manager_permission
                },
                new StorePermission
                {
                    StoreManager = storeManager.ElementAt(1),
                    StoreManagerId = storeManager.ElementAt(1).Id,
                    PermissionType = PermissionType.Manager_permission
                }
            }.AsQueryable();

            var product1 = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    StoreId = stores1.ElementAt(0).Id,
                    Name = "Old Name",
                    Description = "Old Description",
                    Price = 5,
                    UnitsInStock = 10,
                    ImageUrl = "http://old-image.com",
                }

            }.AsQueryable();

            var transaction1 = new List<Transaction>
            {
                new Transaction
                {
                    Id=1,
                    IsMember=true,
                    MemberId=1,
                    TotalPrice=5,
                    TransactionItems= new List<TransactionItem>()
                }
                
            }.AsQueryable();

            var transactonItems1 = new List<TransactionItem>
            {
                new TransactionItem
                {
                    Id=1,
                    TransactionId = 1,
                    ProductId = 1,
                    StoreId = 1,
                    ProductName = product1.ElementAt(0).Name,
                    ProductDescription = product1.ElementAt(0).Description,
                    Quantity = 1,
                    FullPrice = product1.ElementAt(0).Price,
                    FinalPrice = product1.ElementAt(0).Price,
                }

            }.AsQueryable();

            storeManager.ElementAt(0).StorePermissions.Add(permission.ElementAt(0));
            storeManager.ElementAt(1).StorePermissions.Add(permission.ElementAt(1));
            transaction1.ElementAt(0).TransactionItems.Add(transactonItems1.ElementAt(0));
            
            
            _context.Setup(c => c.Members).ReturnsDbSet(member1);


            _context.Setup(c => c.Stores).ReturnsDbSet(stores1);
            _context.Setup(c => c.StoreManagers).ReturnsDbSet(storeManager);
            _context.Setup(c => c.StorePermissions).ReturnsDbSet(permission);
            _context.Setup(x => x.Transactions).ReturnsDbSet(transaction1);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactonItems1);

            var response = await _TransactionController.BrowseShopTransactionHistory(1,1);

            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(1,response.Result.Count());
            Assert.Equal(1, response.Result.ElementAt(0).Id);
            Assert.Equal(5, response.Result.ElementAt(0).TotalPrice);
            Assert.Equal(1, response.Result.ElementAt(0).TransactionItems.Count());
            Assert.Equal(1, response.Result.ElementAt(0).TransactionItems.ElementAt(0).Id);
            Assert.Equal(1, response.Result.ElementAt(0).TransactionItems.ElementAt(0).Quantity);


            var response2 = await _TransactionController.BrowseShopTransactionHistory(2,1);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);

        }
    
    }

}