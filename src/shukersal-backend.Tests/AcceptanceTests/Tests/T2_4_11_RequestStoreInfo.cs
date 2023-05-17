using shukersal_backend.Models;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_4_11_RequestStoreInfo : AcceptanceTest
    {
        private long storeId;
        private bool isInitFinished;
        public T2_4_11_RequestStoreInfo(ITestOutputHelper output) : base(output)
        {
            isInitFinished = false;
            Init();
            while (!isInitFinished) { }
        }

        public async void Init()
        {
            await bridge.Login(new LoginPost { Username = "TestM1", Password = "password" });
            var res = await bridge.CreateStore(new StorePost { Name = "store2411", Description = "desc" });
            if (res.Value != null)
                storeId = res.Value.Id;
            isInitFinished = true;
            bridge.PostStoreManager(new OwnerManagerPost { BossId = 101, MemberId = 102, StoreId = storeId });
        }
        [Fact]
        public async Task TestGetStoreManagerInfo()
        {
            var info = await bridge.GetStoreManagers();
            Assert.NotEmpty(
                info.Value.Where(m => m.MemberId == 102).FirstOrDefault()
                .ParentManager
                .StorePermissions.Where(p => p.PermissionType == PermissionType.Manager_permission)
           );
        }
    }
}