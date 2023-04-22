using NuGet.Protocol;
using shukersal_backend.Controllers;
using shukersal_backend.Controllers.StoreControllers;
using shukersal_backend.Models;
using Xunit.Abstractions;
using System.Threading;

namespace shukersal_backend.Tests.Controllers.ManagerUseCases
{
    public class PermissionsManagemntTests : ManagerTestBase
    {
        public PermissionsManagemntTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void TestPostOwner_MemberIsAlreadyAManagerFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.PostStoreOwner(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 1
                }).Result.Result);
        }
        [Fact]
        public void TestPostOwner_AppointerIsNotAManagerFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.PostStoreOwner(
                new OwnerManagerPost
                {
                    AppointerId = 3,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 4
                }).Result.Result);
        }
        [Fact]
        public void TestPostOwner_StoreDoesntExistFailure()
        {

            Assert.IsType<NotFoundResult>(_controller.PostStoreOwner(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 2,
                    MemberId = 3
                }).Result.Result);

        }
        [Fact]
        public void TestPostOwner_NoPermissionFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.PostStoreOwner(
                new OwnerManagerPost
                {
                    AppointerId = 2,
                    BossId = 2,
                    StoreId = 1,
                    MemberId = 3
                }).Result.Result);
        }
        [Fact]
        public void TestPostOwner_Success()
        {
            Assert.IsType<CreatedAtActionResult>(_controller.PostStoreOwner(
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
