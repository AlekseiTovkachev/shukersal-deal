using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.Models.ShoppingCartModels;
using shukersal_backend.Utility;
using System.Diagnostics;
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
                },
                new DeliveryDetails
                {
                    ReceiverFirstName= "bad_delivery",
                    ReceiverLastName= "bad_delivery",
                    ReceiverAddress= "bad_delivery",
                    ReceiverCountry="Israel",
                    ReceiverCity="Beer Sheva",
                    ReceiverPostalCode="bad_delivery"
                }
            };
        }

        [Fact]
        public async void PurchaseAShoppingCart_Member_Valid()
        {
            var reg = await bridge.AddMember(new MemberPost { Username = "meow", Password = "hatula" });
            //Assert.IsType<CreatedAtActionResult>(reg.Result);
            Assert.IsType<CreatedAtActionResult>(reg.Result);
            var resultValue = (CreatedAtActionResult)reg.Result;
            Assert.IsType<Member>(resultValue.Value);

            var login = await bridge.Login(new LoginPost { Username = "meow", Password = "hatula" });
            Assert.IsType<OkObjectResult>(reg.Result);
            await bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = 1,StoreId=1, Quantity = 1 });
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                IsMember=true,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
                TransactionItems = new List<TransactionItemPost>(),
            });
            Assert.IsType<CreatedAtActionResult>(purchaseResult.Result);
        }

        [Fact]
        public async void PurchaseAShoppingCart_member_not_enough_in_stock()
        {
            await bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = 1, StoreId = 1, Quantity = 1 });
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                IsMember = true,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(0),
            });
            Assert.IsType<BadRequestObjectResult>(purchaseResult.Result);
            var resultValue = (BadRequestObjectResult)purchaseResult.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("", resultValue.Value);
        }

        [Fact]
        public async void PurchaseAShoppingCart_bad_delivery()
        {
            await bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = 1, StoreId = 1, Quantity = 1 }); 
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                IsMember = true,
                BillingDetails = billingDetails.ElementAt(0),
                DeliveryDetails = deliveryDetails.ElementAt(2),//Bad delivery see at the delivery list.
               
            });
            Assert.IsType<BadRequestObjectResult>(purchaseResult.Result);
            var resultValue = (BadRequestObjectResult)purchaseResult.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("delivery Service Confirmation failed", resultValue.Value);
        }

        [Fact]
        public async void PurchaseAShoppingCart_bad_payment()
        {
            
            var addItem=await bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = 1, StoreId = 1, Quantity = 1 });
            Assert.IsType<OkResult>(addItem.Result);
            Assert.IsType<ShoppingItem>(addItem.Value);
            var purchaseResult = await bridge.PurchaseAShoppingCart(
            new TransactionPost
            {
                MemberId = 1,
                IsMember = true,
                BillingDetails = billingDetails.ElementAt(2),//Bad payment see at the payent list.
                DeliveryDetails = deliveryDetails.ElementAt(0),
            });
            Assert.IsType<BadRequestObjectResult>(purchaseResult.Result);
            var resultValue = (BadRequestObjectResult)purchaseResult.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("Payment Service Confirmation failed", resultValue.Value);
        }
    }
}