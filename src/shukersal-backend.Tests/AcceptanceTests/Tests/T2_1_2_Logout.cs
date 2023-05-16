using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_1_2_Logout : AcceptanceTest
    {
        public T2_1_2_Logout(ITestOutputHelper output) : base(output) {
            bridge.Register(new Models.RegisterPost { Username = "testUsername1", Password = "testPassword1" });
            bridge.Register(new Models.RegisterPost { Username = "testUsername2", Password = "testPassword2" });
        }

        [Fact]
        public void loginAndlogout()
        {
            var res = bridge.Login(new Models.LoginPost { Username = "testUsername1", Password = "testPassword1" });
            res.Wait();
            string firstToken = res.Result.Value.Token;
            res = bridge.Login(new Models.LoginPost { Username = "testUsername2", Password = "testPassword2" });
            res.Wait();
            string currToken = res.Result.Value.Token;
            Assert.NotEqual(firstToken, currToken);
        }

        public void loginLogoutLogin()
        {
            var res = bridge.Login(new Models.LoginPost { Username = "testUsername1", Password = "testPassword1" });
            res.Wait();
            string firstToken = res.Result.Value.Token;
            res = bridge.Login(new Models.LoginPost { Username = "testUsername2", Password = "testPassword2" });
            res.Wait();
            res = bridge.Login(new Models.LoginPost { Username = "testUsername1", Password = "testPassword1" });
            res.Wait();
            string currToken = res.Result.Value.Token;
            Assert.Equal(firstToken, currToken);
        }

        public void loginNotExists()
        {
            var res = bridge.Login(new Models.LoginPost { Username = "testUsername1", Password = "testPassword1" });
            res.Wait();
            string firstToken = res.Result.Value.Token;
            res = bridge.Login(new Models.LoginPost { Username = "testUsername3", Password = "testPassword3" });
            res.Wait();
            string currToken = res.Result.Value.Token;
            Assert.NotNull(firstToken);
        }






    }
}