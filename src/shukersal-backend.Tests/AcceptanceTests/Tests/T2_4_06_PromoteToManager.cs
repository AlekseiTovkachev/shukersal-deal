using shukersal_backend.Models;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_4_06_PromoteToManager : AcceptanceTest
    {
        private long storeId;
        private bool isInitFinished;
        public T2_4_06_PromoteToManager(ITestOutputHelper output) : base(output)
        {
            isInitFinished = false;
            Init();
            while (!isInitFinished) { }
        }
        private async void Init()
        {
            await bridge.Login(new LoginPost { Username = "TestM1", Password = "password" });
            var res = await bridge.CreateStore(new StorePost { Name = "store2406", Description = "", /*RootManagerMemberId = 2 */});
            if (res.Value != null)
                storeId = res.Value.Id;

        }
        [Fact]
        public async Task TestPostManagerPositive()
        {
            var res = await bridge.PostStoreManager(new OwnerManagerPost { BossId = 101, MemberId = 102, StoreId = storeId });
            Assert.NotNull(res.Value);
        }
        [Fact]
        public async Task TestSelfPostManager()
        {
            var res = await bridge.PostStoreManager(new OwnerManagerPost { BossId = 101, MemberId = 101, StoreId = storeId });
            Assert.Null(res.Value);
        }
        [Fact]
        public async Task TestNoPermission()
        {
            var res = await bridge.PostStoreManager(new OwnerManagerPost { BossId = 103, MemberId = 104, StoreId = storeId });
            Assert.Null(res.Value);
        }
        [Fact]
        public async Task TestAlreadyAdded()
        {
            await bridge.PostStoreOwner(new OwnerManagerPost { BossId = 101, MemberId = 104, StoreId = storeId });
            var res = await bridge.PostStoreOwner(new OwnerManagerPost { BossId = 101, MemberId = 104, StoreId = storeId });
            Assert.Null(res.Value);
        }
    }
}