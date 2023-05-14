using Newtonsoft.Json.Linq;
using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_4_01_ManageStoreInvetory : AcceptanceTest
    {
        private long StoreId;
        private bool isInitFinished;
        public T2_4_01_ManageStoreInvetory(ITestOutputHelper output) : base(output) {
            isInitFinished = false;
            Init();
            while (!isInitFinished) { }
        }
        private async void Init()
        {

            await bridge.Register(new RegisterPost { Username = "testUsername2401", Password = "testPassword" });
            await bridge.Login(new LoginPost { Username = "testUsername2401", Password = "testPassword" });
            var user = (await bridge.GetLoggedUser()).Value;
            var res = await bridge.CreateStore(new StorePost { Name = "store2401", Description = "", RootManagerMemberId = user.Id });
            if (res.Value != null)
                StoreId = res.Value.Id;
            isInitFinished = true;
        }
        [Fact]
        public async Task TestAddProduct()
        {
            await bridge.AddProduct(StoreId, new ProductPost { Name = "pr1", Description = "test1", IsListed = true });
            var products = (await bridge.GetStoreProducts(StoreId)).Value.Products;
            Assert.Equal(products.FirstOrDefault(res => res.Name == "pr1").Description, "test1");
        }
        [Fact]
        public async Task TestRemoveProduct()
        {
            await bridge.AddProduct(StoreId, new ProductPost { Name = "pr2", Description = "test2", IsListed = true });
            var products = (await bridge.GetStoreProducts(StoreId)).Value.Products;
            await bridge.DeleteProduct(StoreId, products.FirstOrDefault(res => res.Name == "pr2").Id);
            var products2 = (await bridge.GetStoreProducts(StoreId)).Value.Products;
            Assert.Equal(products.Where(res => res.Name == "pr2").Count(), 0);
        }
        [Fact]
        public async Task TestEditProduct()
        {
            await bridge.AddProduct(StoreId, new ProductPost { Name = "pr3", Description = "test3", IsListed = true });
            var products = (await bridge.GetStoreProducts(StoreId)).Value.Products;
            await bridge.UpdateProduct(StoreId, products.FirstOrDefault(res => res.Name == "pr3").Id, new ProductPatch { Description = "test4"});
            var products2 = (await bridge.GetStoreProducts(StoreId)).Value.Products;
            Assert.Equal(products.Where(res => res.Name == "pr3").Count(), 0);
        }
    }
}