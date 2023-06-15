using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_3_1_Logout : AcceptanceTest
    {
        //allready exists at AT 2.1.2
        public T2_3_1_Logout(ITestOutputHelper output) : base(output) { }


        [Fact]
        public  void Logout_MemberWithItemsInCart()
        {
            var addItemResp =bridge.AddItemToCart(1, new ShoppingItemPost { ProductId=1,StoreId=1, Quantity = 1 });
            var logoutResp = bridge.Logout(1);
            var resLogin2 = bridge.Login(new LoginPost { Username = "testUsername1", Password = "testPassword1" });
            var getCartResp = bridge.GetShoppingCartByUserId(1);
            var cart = getCartResp.Result.Value;
            Assert.NotNull(cart);
            Assert.Single(cart.ShoppingBaskets);
            Assert.Single(cart.ShoppingBaskets.ElementAt(0).ShoppingItems);
            Assert.NotNull(addItemResp.Result.Value);
            Assert.Equal(addItemResp.Result.Value.ProductId, cart.ShoppingBaskets.ElementAt(0).ShoppingItems.ElementAt(0).ProductId);
            Assert.Equal(addItemResp.Result.Value.Quantity, cart.ShoppingBaskets.ElementAt(0).ShoppingItems.ElementAt(0).Quantity);


        }


    }
}