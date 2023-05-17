using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using System.Net;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{

    public class T2_2_5_Transaction : AcceptanceTest
    {
        private List<PaymentDetails> billingDetails;
        private List<DeliveryDetails> deliveryDetails;


        public T2_2_5_Transaction(ITestOutputHelper output) : base(output)
        {
            TransactionAT_init();
            //using the init data from the bridge
        }

        private void TransactionAT_init()
        {
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
                     },
                     new PaymentDetails
                    {
                         HolderFirstName = "bad_payment",
                         HolderLastName = "bad_payment",
                         HolderID = "bad_payment",
                         CardNumber = "bad_payment",
                         ExpirationDate = new DateOnly(),
                         CVC = "000",
                     }
            };

            deliveryDetails = new List<DeliveryDetails>
            {
                new DeliveryDetails
                {
                     ReceiverFirstName = "Holder",
                    ReceiverLastName = "Holder",
                    ReceiverPhoneNum = "0506255065",
                    ReceiverAddress = "Holder",
                    ReceiverPostalCode = "2804601"
                },
                new DeliveryDetails
                {
                    ReceiverFirstName= "ReceiverFirstName",
                    ReceiverLastName= "ReceiverLastName",
                    ReceiverPhoneNum="0500000000",
                    ReceiverAddress= "ReceiverAddress",
                    ReceiverPostalCode="1234567"
                },
                new DeliveryDetails
                {
                    ReceiverFirstName= "bad_delivery",
                    ReceiverLastName= "bad_delivery",
                    ReceiverPhoneNum="bad_delivery",
                    ReceiverAddress= "bad_delivery",
                    ReceiverPostalCode="bad_delivery"
                }
            };
        }

        [Fact]
        public async void PurchaseAShoppingCart_Member_Valid()
        {
            var storeResp = await bridge.CreateStore(new StorePost { Name = "mystore", Description = "mystoredesc"/*, RootManagerMemberId = 2 */});
            var product = await bridge.AddProduct(storeResp.Value.Id, new ProductPost { Name = "myproduct1", Description = "myproduct1Desc", Price = 10, UnitsInStock = 3, IsListed = true, CategoryId = 1 });
            await bridge.AddItemToCart(1, new ShoppingItem { Product = product as Product, Quantity = 1 });
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
                TransactionItems = new List<TransactionItem> {
            new TransactionItem { ProductId = ((Product)product).Id,StoreId = storeResp.Value.Id,ProductName = "myproduct1",ProductDescription = "myproduct1Desc",FullPrice = 10 } }
            });
            Assert.NotNull(purchaseResult.Value);
            var history = await bridge.BrowseTransactionHistory(1);
            Assert.NotNull(purchaseResult.Value);
            Assert.Equal(1, history.Value.TransactionItems.Count);
            var products = await bridge.GetStoreProducts(storeResp.Value.Id);
            var updated = products.Value.Products.ElementAt(0);
            Assert.Equal(2, updated.UnitsInStock);
            var shoppingCart = bridge.GetShoppingCartByUserId(2).Result.Value;
            Assert.Equal(0, shoppingCart.ShoppingBaskets.Count);
        }


        public async void PurchaseAShoppingCart_member_not_enough_in_stock()
        {
            var storeResp = await bridge.GetStore(1);
            var product = await bridge.AddProduct(storeResp.Value.Id, new ProductPost { Name = "myproduct2", Description = "myproduct2Desc", Price = 10, UnitsInStock = 0, IsListed = true, CategoryId = 1 });
            await bridge.AddItemToCart(1, new ShoppingItem { Product = product as Product, Quantity = 1 });
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1, //not right user id
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
                TransactionItems = new List<TransactionItem> {
            new TransactionItem { ProductId = ((Product)product).Id,StoreId = storeResp.Value.Id,ProductName = "myproduct1",ProductDescription = "myproduct1Desc",FullPrice = 10 } }
            });
            Assert.NotNull(purchaseResult.Value);
            Assert.IsType<BadRequestObjectResult>(purchaseResult.Result);
            var resultValue = (BadRequestObjectResult)purchaseResult.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("", resultValue.Value);
        }

        public async void PurchaseAShoppingCart_bad_delivery()
        {
            var storeResp = await bridge.GetStore(1);
            var product = await bridge.AddProduct(storeResp.Value.Id, new ProductPost { Name = "myproduct2", Description = "myproduct2Desc", Price = 10, UnitsInStock = 0, IsListed = true, CategoryId = 1 });
            await bridge.AddItemToCart(1, new ShoppingItem { Product = product as Product, Quantity = 1 });
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(2),//Bad delivery see at the delivery list.
                TransactionItems = new List<TransactionItem> {
            new TransactionItem { ProductId = ((Product)product).Id,StoreId = storeResp.Value.Id,ProductName = "myproduct1",ProductDescription = "myproduct1Desc",FullPrice = 10 } }
            });
            Assert.NotNull(purchaseResult.Value);
            Assert.IsType<BadRequestObjectResult>(purchaseResult.Result);
            var resultValue = (BadRequestObjectResult)purchaseResult.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("delivery Service Confirmation failed", resultValue.Value);
        }


        public async void PurchaseAShoppingCart_bad_payment()
        {
            var storeResp = await bridge.GetStore(1);
            var product = await bridge.AddProduct(storeResp.Value.Id, new ProductPost { Name = "myproduct3", Description = "myproduct2Desc", Price = 10, UnitsInStock = 0, IsListed = true, CategoryId = 1 });
            await bridge.AddItemToCart(1, new ShoppingItem { Product = product as Product, Quantity = 1 });
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                BillingDetails = billingDetails.ElementAt(2),//Bad payment see at the payent list.
                DeliveryDetails = deliveryDetails.ElementAt(0),
                TransactionItems = new List<TransactionItem> {
            new TransactionItem { ProductId = ((Product)product).Id,StoreId = storeResp.Value.Id,ProductName = "myproduct1",ProductDescription = "myproduct1Desc",FullPrice = 10 } }
            });
            Assert.NotNull(purchaseResult.Value);
            Assert.IsType<BadRequestObjectResult>(purchaseResult.Result);
            var resultValue = (BadRequestObjectResult)purchaseResult.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("Payment Service Confirmation failed", resultValue.Value);
        }
    }
}