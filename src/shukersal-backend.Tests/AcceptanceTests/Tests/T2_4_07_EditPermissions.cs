using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class T2_4_07_EditPermissions : AcceptanceTest
    {
        private long storeId;
        private long managerId;
        private bool isInitFinished;
        public T2_4_07_EditPermissions(ITestOutputHelper output) : base(output) {
            isInitFinished = false;
            Init();
            while (!isInitFinished) { }
        }
        private async void Init()
        {
            await bridge.Login(new LoginPost { Username = "TestM1", Password = "password" });
            var res = await bridge.CreateStore(new StorePost { Name = "store2407", Description = "", RootManagerMemberId = 2 });
            if (res.Value != null)
                storeId = res.Value.Id;
            var res2 = await bridge.PostStoreManager(new OwnerManagerPost { BossId = 101, MemberId = 102, StoreId = storeId });
            if (res2.Value != null)
                managerId = res.Value.Id;
            isInitFinished = true;

        }
        [Fact]
        public async Task TestGivePermission()
        {
            bridge.AddPermissionToManager(managerId, PermissionType.Manage_products_permission);
            var res = await bridge.GetStoreManager(managerId);
            Assert.NotEmpty(res.Value.StorePermissions.Where(sp => sp.PermissionType == PermissionType.Manage_products_permission));
        }
        [Fact]
        public async Task TestTakePostManager()
        {
            bridge.AddPermissionToManager(managerId, PermissionType.Get_history_permission);
            var res = await bridge.GetStoreManager(managerId);
            Assert.Empty(res.Value.StorePermissions.Where(sp => sp.PermissionType == PermissionType.Get_history_permission));
        }
    }
    
}