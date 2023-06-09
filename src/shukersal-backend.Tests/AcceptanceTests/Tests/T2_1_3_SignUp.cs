﻿using NuGet.Protocol;
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
        public async void TestRegisterNotNull()
        {
            var res = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass" });
            res.Wait();
            Assert.NotNull(res);
        }

        [Fact]
        public async void TestRegisterUserName()
        {
            var res = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass" });
            res.Wait();
            string resUserName = res.Result.Value.Username;
            Assert.Equal("User", resUserName); 
        }

        [Fact]
        public async void TestRegisterPassword()
        {
            var res = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass" });
            res.Wait();
            string resPassword = res.Result.Value.Password;
            Assert.Equal("UserPass", resPassword);
        }
        [Fact]
        public async void TestRegisterSameUsername()
        {
            var res = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass" });
            res.Wait();
            var res2 = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass2" });
            res2.Wait();
            Assert.IsType<BadRequestObjectResult>(res2.Result);
        }
        public async void TestRegisterBadPassword()
        {
            var res = bridge.Register(new RegisterPost { Username = "User", Password = "P" });
            res.Wait();;
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }
        [Fact]
        public void TestRegisterDoubleRequest()
        {
            var res1 = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass" });
            var res2 = bridge.Register(new RegisterPost { Username = "User", Password = "UserPass" });
            res1.Wait();
            res2.Wait();
            Assert.True(res1.Result is ActionResult ^ res2.Result is ActionResult);

        }
    }
}