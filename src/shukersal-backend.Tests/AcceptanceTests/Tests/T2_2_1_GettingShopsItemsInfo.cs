using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_2_1_GettingShopsItemsInfo : AcceptanceTest
    {
        public T2_2_1_GettingShopsItemsInfo(ITestOutputHelper output) : base(output) {
            bridge.Register(new Models.RegisterPost { Id = 2, Username = "testUsername", Password = "testPassword" });
            bridge.Login(new Models.LoginPost { Username = "testUsername", Password = "testPassword" });
        }
        [Fact]
        public async void GetShopInfo()
        {
            var res = bridge.GetStores();
            res.Wait();
            var res2 = bridge.Register(new RegisterPost { Id = 11, Username = "User", Password = "UserPass2" });
            res2.Wait();
            Assert.False(res2.Result is ActionResult);
        }
        [Fact]
        public async void TestRegisterSameUsername()
        {
            var res = bridge.Register(new RegisterPost { Id = 10, Username = "User", Password = "UserPass" });
            res.Wait();
            var res2 = bridge.Register(new RegisterPost { Id = 11, Username = "User", Password = "UserPass2" });
            res2.Wait();
            Assert.False(res2.Result is ActionResult);
        }
    }

}