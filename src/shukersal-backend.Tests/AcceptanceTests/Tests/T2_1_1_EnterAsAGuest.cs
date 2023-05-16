using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_1_1_EnterAsAGuest : AcceptanceTest
    {
        public T2_1_1_EnterAsAGuest(ITestOutputHelper output) : base(output) {
            //TODO: guest implementation is not defined yet.
        }

        [Fact]
        public void GetShopsInfo()
        {
            var res = bridge.GetStores();
            res.Wait();
            Assert.Equal(res.Result.Value.Count(), 3);
        }
        [Fact]
        public void GetShopInfo()
        {
            var res = bridge.GetStore(1);
            res.Wait();
            Assert.Equal(res.Result.Value.Name, "1");
        }
        [Fact]
        public void GetShopProductInfo()
        {
            var res = bridge.GetStoreProducts(1);
            res.Wait();
            Assert.Equal(res.Result.Value.Products.Count, 2);
        }

    }
}