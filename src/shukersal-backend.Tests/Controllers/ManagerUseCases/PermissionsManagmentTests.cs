using NuGet.Protocol;
using shukersal_backend;
using shukersal_backend.Models;
using Xunit.Abstractions;
using System.Threading;

namespace shukersal_backend.Tests.Controllers.ManagerUseCases
{
    public class PostOwnerTests : ManagerTestBase
    {
        public PostOwnerTests(ITestOutputHelper output) : base(output)
        {

        }

        /*[Fact]
        public void TestAddPermission_Success()
        {
            Assert.IsType<OkResult>(_controller.AddPermissionToManager(2, PermissionType.Appoint_owner_permission).Result);
        }
        [Fact]
        public void TestAddPermission_PermissionAlreadyExistsFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.AddPermissionToManager(2, PermissionType.Get_history_permission).Result);
        }
        [Fact]
        public void TestAddPermission_OwnerFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.AddPermissionToManager(1, PermissionType.Appoint_owner_permission).Result);
        }

        [Fact]
        public void TestRemovePermission_Success()
        {
            Assert.IsType<OkResult>(_controller.AddPermissionToManager(2, PermissionType.Appoint_owner_permission).Result);
        }
        [Fact]
        public void TestRemovePermission_PermissionDoesntExistFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.AddPermissionToManager(2, PermissionType.Get_history_permission).Result);
        }
        [Fact]
        public void TestRemovePermission_OwnerFailure()
        {
            Assert.IsType<NotFoundResult>(_controller.AddPermissionToManager(1, PermissionType.Appoint_owner_permission).Result);
        }*/
    }
}
