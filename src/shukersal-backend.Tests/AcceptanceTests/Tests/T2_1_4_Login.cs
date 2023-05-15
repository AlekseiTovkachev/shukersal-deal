using Microsoft.AspNetCore.Mvc;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;
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
        public T2_1_4_Login(ITestOutputHelper output) : base(output) {
            bridge.Register(new Models.RegisterPost { Username = "testUsername1", Password = "testPassword1" });
            bridge.Register(new Models.RegisterPost { Username = "testUsername2", Password = "testPassword2" });
        }

        [Fact]
        public async void TestLogInPositive()
        {
            var res = bridge.Login(new LoginPost { Username = "testUsername1", Password = "testPassword1" });
            res.Wait();
            Assert.IsType<LoginResponse>(res.Result.Value);
            string userName = res.Result.Value.Member.Username;
            Assert.Equal(userName, "testUsername1");
        }


        public void TokenTest1()
        {
            var res = bridge.Login(new Models.LoginPost { Username = "testUsername1", Password = "testPassword1" });
            res.Wait();
            string firstToken = res.Result.Value.Token;
            res = bridge.Login(new Models.LoginPost { Username = "testUsername2", Password = "testPassword2" });
            res.Wait();
            string currToken = res.Result.Value.Token;
            Assert.NotEqual(firstToken, currToken);
        }


        public void TokenTest2()
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


        [Fact]
        public async void TestLogInWrongPassword()
        {
            var res = bridge.Login(new LoginPost { Username = "testUsername1", Password = "wrongPassword" });
            res.Wait();
            Assert.IsType<BadRequestObjectResult>(res.Result.Result);
            var resultValue = (BadRequestObjectResult)res.Result.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("Wrong username or password.", resultValue.Value);
        }


        [Fact]
        public async void TestLogInWrongUsername()
        {
            var res = bridge.Login(new LoginPost { Username = "wrongUsername", Password = "testPassword1" });
            res.Wait();
            Assert.IsType<BadRequestObjectResult>(res.Result.Result);
            var resultValue = (BadRequestObjectResult)res.Result.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, resultValue.StatusCode);
            Assert.Equal("Wrong username or password.", resultValue.Value);
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