using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_2_3_AddItem : AcceptanceTest
    {
        public T2_2_3_AddItem(ITestOutputHelper output) : base(output)
        {
            bridge.Register(new RegisterPost {Username = "testUsername", Password = "testPassword" });
            bridge.Login(new LoginPost { Username = "testUsername", Password = "testPassword" });
        }
        [Fact]
        public void AddItemPositive()
        {
            var product = bridge.GetStoreProducts(1).Result.Value.Products.ElementAt(1);
            var res = bridge.AddItemToCart(1, new ShoppingItem { Id = 100, Product = product, Quantity = 1});
            res.Wait();
            Assert.True(res.Result is IActionResult);
        }
        [Fact]
        public void AddItemQuantity()
        {
            var product = bridge.GetStoreProducts(1).Result.Value.Products.ElementAt(0);
            var res = bridge.AddItemToCart(1, new ShoppingItem { Id = 100, Product = product, Quantity = 2 });
            res.Wait();
            Assert.False(res.Result is IActionResult);
        }
        [Fact]
        public void AddItemDoubleRequest()
        {
            var product = bridge.GetStoreProducts(1).Result.Value.Products.ElementAt(0);
            var res1 = bridge.AddItemToCart(1, new ShoppingItem { Id = 100, Product = product, Quantity = 1 });
            var res2 = bridge.AddItemToCart(1, new ShoppingItem { Id = 200, Product = product, Quantity = 1 });
            res1.Wait();
            res2.Wait();
            Assert.True(res1.Result is IActionResult ^ res2.Result is IActionResult);
        }
        [Fact]
        public void AddWrongItem()
        {
            var res = bridge.AddItemToCart(1, new ShoppingItem { Id = 100, Product = new Product { Id = -1 }, Quantity = 1 });
            res.Wait();
            Assert.False(res.Result is IActionResult);
        }
    }
}