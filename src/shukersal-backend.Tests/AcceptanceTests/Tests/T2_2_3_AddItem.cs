using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
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
        public async void AddItemPositive()
        {

            var res = bridge.AddItemToCart(1, new ShoppingItemPost {ProductId = 1,StoreId=1, Quantity = 1});
            res.Wait();
            Assert.NotNull(res.Result.Value);
            Assert.Equal(1,res.Result.Value.Id);
            
        }
        [Fact]
        public async void EditItemQuantity()
        {
            var productresp = await bridge.GetStoreProducts(1);
            Assert.NotNull(productresp.Value);
            var product = productresp.Value.Products.ElementAt(0);
            var res = bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = product.Id, StoreId = product.StoreId, Quantity = 2 });
            var res2=await  bridge.EditItemQuantity(1, new ShoppingItemPost { ProductId = product.Id, StoreId = product.StoreId, Quantity = 3 });
            Assert.NotNull(res2.Value);
            Assert.Equal(product.Id, res2.Value.Id);
            Assert.Equal(2, res2.Value.Quantity);

            var cart= await bridge.GetShoppingCartByCartId(1);
            Assert.NotNull(cart.Value);
            var basket = cart.Value.ShoppingBaskets.Where(b => b.StoreId == product.StoreId);
            Assert.Single(basket);
            Assert.Single(basket.ElementAt(0).ShoppingItems);
            Assert.Equal(product.Id, basket.ElementAt(0).ShoppingItems.ElementAt(0).ProductId);
            Assert.Equal(3, basket.ElementAt(0).ShoppingItems.ElementAt(0).Quantity);




        }
        [Fact]
        public async void AddItemDoubleRequest()
        {
            var store = await bridge.GetStoreProducts(1);
            Assert.NotNull(store.Value);
            var product=store.Value.Products.First();
            Assert.NotNull(product);
            var res1 =await bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = product.Id, Quantity = 1 });
            var res2 =await bridge.AddItemToCart(1, new ShoppingItemPost { ProductId = product.Id, Quantity = 1 });

            Assert.NotNull(res1.Value);
            Assert.Null(res2.Value);

        }
        [Fact]
        public void AddWrongItem()
        {
            var res = bridge.AddItemToCart(1, new ShoppingItemPost { ProductId=-1, Quantity = 1 });
            res.Wait();
            Assert.False(res.Result is IActionResult);
        }
    }
}