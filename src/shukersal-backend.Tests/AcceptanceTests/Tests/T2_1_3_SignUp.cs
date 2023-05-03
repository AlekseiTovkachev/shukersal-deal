using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_1_3_SignUp : AcceptanceTest
    {
        public T2_1_3_SignUp(ITestOutputHelper output) : base(output) { }
        [Fact]
        public async void TestRegisterPositive()
        {
            var res = bridge.Register(new RegisterPost { Id= 10, Username = "User", Password = "UserPass" });
            res.Wait();
            Assert.True(res.Result is ActionResult);
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
        [Fact]
        public void TestRegisterDoubleRequest()
        {
            var res1 = bridge.Register(new RegisterPost { Id = 10, Username = "User", Password = "UserPass" });
            var res2 = bridge.Register(new RegisterPost { Id = 10, Username = "User", Password = "UserPass" });
            res1.Wait();
            res2.Wait();
            Assert.True(res1.Result is ActionResult ^ res2.Result is ActionResult);

        }
    }
}