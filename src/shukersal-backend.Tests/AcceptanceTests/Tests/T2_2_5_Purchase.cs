using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_2_5_Transaction : AcceptanceTest
    {
        public T2_2_5_Transaction(ITestOutputHelper output) : base(output) {
            
        }

        [Fact]
        public async void PurchaseAShoppingCart_Member_Valid()
        {
            var registerResp = await bridge.Register(new RegisterPost { Username = "testUsername8", Password = "testPassword" });
            var loginResp =await bridge.Login(new LoginPost { Username = "testUsername8", Password = "testPassword" });
            var storeResp = await bridge.CreateStore(new StorePost { Name = "mystore", Description = "mystoredesc", RootManagerMemberId = 1 });
            //var priduct = await bridge.AddProduct(storeResp.Value.Id, new ProductPost {Name="myproduct1" })
        }

    }
}