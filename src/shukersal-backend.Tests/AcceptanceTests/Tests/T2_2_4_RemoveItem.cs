using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_2_4_RemoveItem : AcceptanceTest
    {
        public T2_2_4_RemoveItem(ITestOutputHelper output) : base(output)
        {
            bridge.Register(new RegisterPost { Username = "testUsername", Password = "testPassword" });
            bridge.Login(new LoginPost { Username = "testUsername", Password = "testPassword" });
        }
        [Fact]
        public async void RemoveItemPositive()
        {
            var store = await bridge.GetStoreProducts(1);
            Assert.NotNull(store.Value);
            var product=store.Value.Products.ElementAt(1);
            var si = new ShoppingItemPost {ProductId = product.Id,StoreId=store.Value.Id, Quantity = 1 };
            var item = await bridge.AddItemToCart(1, si);
            Assert.NotNull(item.Value);
            var res = await bridge.RemoveItemFromCart(1, item.Value.Id);
            Assert.True(res.Result is IActionResult);
        }
        [Fact]
        public void RemoveItemDoesntExist()
        {
            var res =  bridge.RemoveItemFromCart(1, -1);
            res.Wait();
            Assert.False(res.Result is IActionResult);
        }
        [Fact]
        public void RemoveItemDoubleRequest()
        {
            bridge.Login(new LoginPost { Username = "testUsername", Password = "testPassword" });
            var si = new ShoppingItemPost {ProductId = 1,StoreId=1, Quantity = 1 };
            var item=  bridge.AddItemToCart(1, si);
            item.Wait();
            Assert.True(item.Result is IActionResult);
            var res1 =  bridge.RemoveItemFromCart(1, 1);
            var res2 =  bridge.RemoveItemFromCart(1, 1);
            Assert.True(res1.Result is IActionResult);
            Assert.False(res2.Result is IActionResult);

        }
    }
}