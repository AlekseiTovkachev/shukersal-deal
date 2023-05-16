using shukersal_backend.Models;
using System;
using System.Collections.Generic;
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
        public void RemoveItemPositive()
        {
            var product = bridge.GetStoreProducts(1).Result.Value.Products.ElementAt(1);
            bridge.AddItemToCart(1, new ShoppingItem { Id = 100, Product = product, Quantity = 1 }).Wait();
            var res = bridge.RemoveItemFromCart(1, 100);
            res.Wait();
            Assert.True(res.Result is IActionResult);
        }
        [Fact]
        public void RemoveItemDoesntExist()
        {
            var product = bridge.GetStoreProducts(1).Result.Value.Products.ElementAt(1);
            var res = bridge.RemoveItemFromCart(1, 100);
            res.Wait();
            Assert.False(res.Result is IActionResult);
        }
        [Fact]
        public void RemoveItemDoubleRequest()
        {
            var product = bridge.GetStoreProducts(1).Result.Value.Products.ElementAt(1);
            bridge.AddItemToCart(1, new ShoppingItem { Id = 100, Product = product, Quantity = 1 }).Wait();
            var res1 = bridge.RemoveItemFromCart(1, 100);
            var res2 = bridge.RemoveItemFromCart(1, 100);
            res1.Wait();
            res2.Wait();
            Assert.True(res1.Result is IActionResult ^ res2.Result is IActionResult);
        }
    }
}