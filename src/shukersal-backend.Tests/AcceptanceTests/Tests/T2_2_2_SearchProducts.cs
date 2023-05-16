using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_2_2_SearchProducts : AcceptanceTest
    {
        public T2_2_2_SearchProducts(ITestOutputHelper output) : base(output)
        {
            bridge.Register(new Models.RegisterPost { Username = "testUsername", Password = "testPassword" });
            bridge.Login(new Models.LoginPost { Username = "testUsername", Password = "testPassword" });
        }

        //TODO: need the right sig for the function.
    }
}