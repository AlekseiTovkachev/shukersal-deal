using NuGet.Protocol;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Models;
using Xunit.Abstractions;
using System.Threading;

namespace shukersal_backend.Tests.Controllers.ManagerUseCases
{
    public class PostManagerTests : ManagerTestBase
    {
        public PostManagerTests(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public void TestPostManager_MemberIsAlreadyAManagerFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 1
                }).Result.Result);
        }
        [Fact]
        public void TestPostManager_AppointerIsNotAManagerFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 3,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 4
                }).Result.Result);
        }
        [Fact]
        public void TestPostManager_StoreDoesntExistFailure()
        {
            
            Assert.IsType<NotFoundResult>(_controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 2,
                    MemberId = 3
                }).Result.Result);
            
        }
        [Fact]
        public void TestPostManager_NoPermissionFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 2,
                    BossId = 2,
                    StoreId = 1,
                    MemberId = 3
                }).Result.Result);
        }
        [Fact]
        public void TestPostManager_Success()
        {
            Assert.IsType<CreatedAtActionResult>(_controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 2,
                    StoreId = 1,
                    MemberId = 3
                }).Result.Result);
        }




    }
}
