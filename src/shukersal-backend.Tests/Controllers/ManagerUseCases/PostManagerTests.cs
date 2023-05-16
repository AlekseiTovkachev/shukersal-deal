using shukersal_backend.Models;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.Controllers.ManagerUseCases
{
    public class PostManagerTests : ManagerTestBase
    {
        public PostManagerTests(ITestOutputHelper output) : base(output)
        {

        }

        /*[Fact]
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

        [Fact]
        public async void TestPostManager_Multithreaded()
        {
            var post1 = new OwnerManagerPost
            {
                AppointerId = 1,
                BossId = 2,
                StoreId = 1,
                MemberId = 3
            };
            var post2 = new OwnerManagerPost
            {
                AppointerId = 1,
                BossId = 2,
                StoreId = 1,
                MemberId = 3
            };

            for (int i = 0; i < 1; i++)
            {

                var result1 = _controller.PostStoreManager(post1);
                var result2 = _controller.PostStoreManager(post2);
                var wres1 = await result1;
                var wres2 = await result2;

                // Assert
                Assert.NotNull(wres1.Result);
                Assert.NotNull(wres2.Result);
                Assert.True(wres1.IsSuccess ^ wres2.IsSuccess);
            }
        }*/




    }
}
