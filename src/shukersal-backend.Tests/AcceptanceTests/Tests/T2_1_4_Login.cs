using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_1_4_Login : AcceptanceTest
    {
        public T2_1_4_Login(ITestOutputHelper output) : base(output) { }
        [Fact]
        public async void TestLogInPositive()
        {
            var res = bridge.Login(new LoginPost { Username = "Admin", Password = "AdminPass" });
            res.Wait();
            Assert.True(res.Result is ActionResult);
        }
        [Fact]
        public void TestLogInWrongPassword()
        {
            var res = bridge.Login(new LoginPost { Username = "Admin2", Password = "AdminPass1" });
            Assert.False(res.Result is ActionResult);
        }
        [Fact]
        public void TestLogInAlreadyLogIn()
        {
            var res1 = bridge.Login(new LoginPost { Username = "Admin2", Password = "AdminPass" });
            var res2 = bridge.Login(new LoginPost { Username = "Admin2", Password = "AdminPass" });
            res1.Wait();
            res2.Wait();
            Assert.True(res1.Result is ActionResult ^ res2.Result is ActionResult);

        }
    }
}