using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_3_1_Logout : AcceptanceTest
    {
        //allready exists at AT 2.1.2
        public T2_3_1_Logout(ITestOutputHelper output) : base(output) {}


        [Fact]
        public async void Logout_MemberWithItemsInCart()
        {
            var respReg= bridge.Register(new RegisterPost { Username = "testUsername1", Password = "testPassword1" });
            var resLogin = bridge.Login(new LoginPost { Username = "testUsername1", Password = "testPassword1" });
            var storeResp = bridge.CreateStore(new StorePost { Name = "mystore", Description = "mystoredesc", RootManagerMemberId = resLogin.Result.Value.Member.Id });
            var product = await bridge.AddProduct(storeResp.Result.Value.Id, new ProductPost 
                { Name = "myproduct1", Description = "myproduct1Desc", Price = 10, UnitsInStock = 3, IsListed = true, CategoryId = 1 });
            var addItemResp = bridge.AddItemToCart(resLogin.Result.Value.Member.ShoppingCart.Id, new ShoppingItem { Product = product as Product, Quantity = 1 });
            var logoutResp = bridge.Logout(resLogin.Result.Value.Member.Id);
            var resLogin2 =  bridge.Login(new LoginPost { Username = "testUsername1", Password = "testPassword1" });
            var getCartResp = bridge.GetShoppingCartByUserId(resLogin.Result.Value.Member.Id);
            var cart=getCartResp.Result.Value;
            Assert.NotNull(cart);
            Assert.Single(cart.ShoppingBaskets);
            Assert.Single(cart.ShoppingBaskets.ElementAt(0).ShoppingItems);
            Assert.Equal(addItemResp.Result.Value.ProductId,cart.ShoppingBaskets.ElementAt(0).ShoppingItems.ElementAt(0).Product.Id);
            Assert.Equal(addItemResp.Result.Value.Quantity, cart.ShoppingBaskets.ElementAt(0).ShoppingItems.ElementAt(0).Quantity);


        }


    }
}