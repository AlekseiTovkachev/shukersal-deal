using Azure;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using shukersal_backend.DomainLayer.Controllers;
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
            _output = output;
            _TransactionController = new TransactionController(_context.Object);
            _TransactionController.SetRealDeliveryAdapter("");
            _TransactionController.SetRealpaymentAdapter("");
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
            _context.Setup(x => x.Transactions).ReturnsDbSet(transactions);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems);

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
            _context.Setup(x => x.Stores).ReturnsDbSet(stores);
            _context.Setup(x => x.Transactions).ReturnsDbSet(transactions);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems);

            var response = await _TransactionController.BrowseShopTransactionHistory(1,1);

            //Assert.Equal("", response.ErrorMessage);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(3, response.Result.Count());


            var response2 = await _TransactionController.BrowseShopTransactionHistory(2,1);
            //Assert.Equal("", response2.ErrorMessage);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.NotNull(response2.Result);
            Assert.Empty(response2.Result);

        }

        [Fact]
        public async Task PurchaseAShoppingCart_Success()
        {
            _context.Setup(x => x.Members).ReturnsDbSet(members);
            _context.Setup(x => x.Stores).ReturnsDbSet(stores);
            _context.Setup(x => x.Products).ReturnsDbSet(products);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(shoppingCarts);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(shoppingBaskets);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(shoppingItems);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems);

            var response1 = await _TransactionController.PurchaseAShoppingCart(transactionPosts.ElementAt(1));
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.IsType<Transaction>(response1.Result);
            Assert.Equal(2, response1.Result.TransactionItems.Count);
            Assert.Equal(60, response1.Result.TotalPrice);
        }


        [Fact]
        public async Task PurchaseAShoppingCart_Fail_purchaseRules()
        {
            var purchaseRules1 = new List<PurchaseRule>()
            {
                new PurchaseRule
                {
                    Id = 1,
                    purchaseRuleType = PurchaseRuleType.PRODUCT_AT_LEAST,
                    conditionString = "",
                    conditionLimit=2,
                },
            };

            var stores1 = new List<Store>() 
            {
                new Store {
                    Id = 1,
                    Name = "Store 1",
                    RootManagerId = 1,
                    Products = new List<Product>()
                    {
                        new Product {
                            Name = "product",
                            Description = "Desc",
                            Price = 10,
                            ImageUrl = "www.bla.com",
                            IsListed = true,
                            UnitsInStock = 10,
                            StoreId = 1
                        },
                    },
                    DiscountRules = new List<DiscountRule>(){ },
                    PurchaseRules=new List<PurchaseRule>(){purchaseRules1.ElementAt(0),},
                    
                }
            };
            
            var products1 = new List<Product>() 
            {
                new Product 
                {
                    Id=1,
                    Name = "product",
                    Description = "Desc",
                    Price = 10,
                    ImageUrl = "www.bla.com",
                    IsListed = true,
                    UnitsInStock = 10,
                    StoreId = 1 
                },
            };

            var shoppingCarts1= new List<ShoppingCart>() 
            {
                new ShoppingCart
                {
                    Id=1,
                    Member = members.ElementAt(0),
                    ShoppingBaskets = new List<ShoppingBasket>(),
                }
            };
            var shoppingBaskets1 = new List<ShoppingBasket>()
            {
                 new ShoppingBasket
                 {
                    Id=1,
                    ShoppingCartId = shoppingCarts1.ElementAt(0).Id,
                    ShoppingItems = new List<ShoppingItem>() 
                 }
            };
            var shoppingItems1 = new List<ShoppingItem>()
            {
                new ShoppingItem
                {
                    ShoppingBasketId =1,
                   // ShoppingBasket =shoppingBaskets1.ElementAt(0),
                    ProductId = 1,
                    Product =products1.ElementAt(0),
                    Quantity = 1
                },
            };

            shoppingBaskets1.ElementAt(0).ShoppingItems.Add(shoppingItems1.ElementAt(0));
            shoppingCarts1.ElementAt(0).ShoppingBaskets.Add(shoppingBaskets1.ElementAt(0));
            var transactionItems1 = new List<TransactionItem>()
            {
                new TransactionItem
                {
                    TransactionId = 1,
                    ProductId = 1,
                    StoreId = 1,
                    ProductName = products1.ElementAt(0).Name,
                    ProductDescription = products1.ElementAt(0).Description,
                    Quantity = 1,
                    FullPrice = products1.ElementAt(0).Price,
                    FinalPrice = products1.ElementAt(0).Price,
                },
            };

            var transactionPost = new TransactionPost() 
            {
                MemberId = 1,
                TransactionDate = new DateTime(),
                TotalPrice = 130,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
            };

            _context.Setup(x => x.Members).ReturnsDbSet(members);
            _context.Setup(x => x.PurchaseRules).ReturnsDbSet(purchaseRules1);
            _context.Setup(x => x.Stores).ReturnsDbSet(stores1);
            _context.Setup(x => x.Products).ReturnsDbSet(products1);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(shoppingCarts1);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(shoppingBaskets1);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(shoppingItems1);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems1);


            var response1 = await _TransactionController.PurchaseAShoppingCart(transactionPost);
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal("Purchase rule vaiolation", response1.ErrorMessage);
            
        }

      
        [Fact]
        public async Task PurchaseAShoppingCart_Fail_deliveryNotConfirmed()
        {
            var purchaseRules1 = new List<PurchaseRule>()
            {
                new PurchaseRule
                {
                    Id = 1,
                    purchaseRuleType = PurchaseRuleType.PRODUCT_AT_LEAST,
                    conditionString = "",
                    conditionLimit=2,
                },
            };

            var stores1 = new List<Store>()
            {
                new Store {
                    Id = 1,
                    Name = "Store 1",
                    RootManagerId = 1,
                    Products = new List<Product>()
                    {
                        new Product {
                            Name = "product",
                            Description = "Desc",
                            Price = 10,
                            ImageUrl = "www.bla.com",
                            IsListed = true,
                            UnitsInStock = 10,
                            StoreId = 1
                        },
                    },
                    DiscountRules = new List<DiscountRule>(){ },
                    PurchaseRules=new List<PurchaseRule>(){purchaseRules1.ElementAt(0),},

                }
            };

            var products1 = new List<Product>()
            {
                new Product
                {
                    Id=1,
                    Name = "product",
                    Description = "Desc",
                    Price = 10,
                    ImageUrl = "www.bla.com",
                    IsListed = true,
                    UnitsInStock = 10,
                    StoreId = 1
                },
            };

            var shoppingCarts1 = new List<ShoppingCart>()
            {
                new ShoppingCart
                {
                    Id=1,
                    Member = members.ElementAt(0),
                    ShoppingBaskets = new List<ShoppingBasket>(),
                }
            };
            var shoppingBaskets1 = new List<ShoppingBasket>()
            {
                 new ShoppingBasket
                 {
                    Id=1,
                    ShoppingCartId = shoppingCarts1.ElementAt(0).Id,
                    ShoppingItems = new List<ShoppingItem>()
                 }
            };
            var shoppingItems1 = new List<ShoppingItem>()
            {
                new ShoppingItem
                {
                    ShoppingBasketId =1,
                  //  ShoppingBasket =shoppingBaskets1.ElementAt(0),
                    ProductId = 1,
                    Product =products1.ElementAt(0),
                    Quantity = 1
                },
            };

            shoppingBaskets1.ElementAt(0).ShoppingItems.Add(shoppingItems1.ElementAt(0));
            shoppingCarts1.ElementAt(0).ShoppingBaskets.Add(shoppingBaskets1.ElementAt(0));
            var transactionItems1 = new List<TransactionItem>()
            {
                new TransactionItem
                {
                    TransactionId = 1,
                    ProductId = 1,
                    StoreId = 1,
                    ProductName = products1.ElementAt(0).Name,
                    ProductDescription = products1.ElementAt(0).Description,
                    Quantity = 1,
                    FullPrice = products1.ElementAt(0).Price,
                    FinalPrice = products1.ElementAt(0).Price,
                },
            };

            var transactionPost = new TransactionPost()
            {
                MemberId = 1,
                TransactionDate = new DateTime(),
                TotalPrice = 130,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
            };

            _context.Setup(x => x.Members).ReturnsDbSet(members);
            _context.Setup(x => x.PurchaseRules).ReturnsDbSet(purchaseRules1);
            _context.Setup(x => x.Stores).ReturnsDbSet(stores1);
            _context.Setup(x => x.Products).ReturnsDbSet(products1);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(shoppingCarts1);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(shoppingBaskets1);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(shoppingItems1);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems1);

            _TransactionController.getDeliveryProxy().SetProxyAnswer(false);
            var response1 = await _TransactionController.PurchaseAShoppingCart(transactionPost);
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal("Delivery was not confirmed", response1.ErrorMessage);
            Assert.Equal(10, _context.Object.Products.ElementAt(0).UnitsInStock);


        }

        [Fact]
        public async Task PurchaseAShoppingCart_Fail_paymentNotConfirmed()
        {
            var purchaseRules1 = new List<PurchaseRule>()
            {
                new PurchaseRule
                {
                    Id = 1,
                    purchaseRuleType = PurchaseRuleType.PRODUCT_AT_LEAST,
                    conditionString = "",
                    conditionLimit=2,
                },
            };

            var stores1 = new List<Store>()
            {
                new Store {
                    Id = 1,
                    Name = "Store 1",
                    RootManagerId = 1,
                    Products = new List<Product>()
                    {
                        new Product {
                            Name = "product",
                            Description = "Desc",
                            Price = 10,
                            ImageUrl = "www.bla.com",
                            IsListed = true,
                            UnitsInStock = 10,
                            StoreId = 1
                        },
                    },
                    DiscountRules = new List<DiscountRule>(){ },
                    PurchaseRules=new List<PurchaseRule>(){purchaseRules1.ElementAt(0),},

                }
            };

            var products1 = new List<Product>()
            {
                new Product
                {
                    Id=1,
                    Name = "product",
                    Description = "Desc",
                    Price = 10,
                    ImageUrl = "www.bla.com",
                    IsListed = true,
                    UnitsInStock = 10,
                    StoreId = 1
                },
            };

            var shoppingCarts1 = new List<ShoppingCart>()
            {
                new ShoppingCart
                {
                    Id=1,
                    Member = members.ElementAt(0),
                    ShoppingBaskets = new List<ShoppingBasket>(),
                }
            };
            var shoppingBaskets1 = new List<ShoppingBasket>()
            {
                 new ShoppingBasket
                 {
                    Id=1,
                    ShoppingCartId = shoppingCarts1.ElementAt(0).Id,
                    ShoppingItems = new List<ShoppingItem>()
                 }
            };
            var shoppingItems1 = new List<ShoppingItem>()
            {
                new ShoppingItem
                {
                    ShoppingBasketId =1,
                    //ShoppingBasket =shoppingBaskets1.ElementAt(0),
                    ProductId = 1,
                    Product =products1.ElementAt(0),
                    Quantity = 1
                },
            };

            shoppingBaskets1.ElementAt(0).ShoppingItems.Add(shoppingItems1.ElementAt(0));
            shoppingCarts1.ElementAt(0).ShoppingBaskets.Add(shoppingBaskets1.ElementAt(0));
            var transactionItems1 = new List<TransactionItem>()
            {
                new TransactionItem
                {
                    TransactionId = 1,
                    ProductId = 1,
                    StoreId = 1,
                    ProductName = products1.ElementAt(0).Name,
                    ProductDescription = products1.ElementAt(0).Description,
                    Quantity = 1,
                    FullPrice = products1.ElementAt(0).Price,
                    FinalPrice = products1.ElementAt(0).Price,
                },
            };

            var transactionPost = new TransactionPost()
            {
                MemberId = 1,
                TransactionDate = new DateTime(),
                TotalPrice = 130,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
            };

            _context.Setup(x => x.Members).ReturnsDbSet(members);
            _context.Setup(x => x.PurchaseRules).ReturnsDbSet(purchaseRules1);
            _context.Setup(x => x.Stores).ReturnsDbSet(stores1);
            _context.Setup(x => x.Products).ReturnsDbSet(products1);
            _context.Setup(x => x.ShoppingCarts).ReturnsDbSet(shoppingCarts1);
            _context.Setup(x => x.ShoppingBaskets).ReturnsDbSet(shoppingBaskets1);
            _context.Setup(x => x.ShoppingItems).ReturnsDbSet(shoppingItems1);
            _context.Setup(x => x.TransactionItems).ReturnsDbSet(transactionItems1);

            _TransactionController.getPaymentProxy().SetProxyAnswer(false);
            var response1 = await _TransactionController.PurchaseAShoppingCart(transactionPost);
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal("Payment was not confirmed", response1.ErrorMessage);
            Assert.Equal(10, _context.Object.Products.ElementAt(0).UnitsInStock);

        }
    }

}