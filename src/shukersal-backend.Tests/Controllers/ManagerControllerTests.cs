using shukersal_backend.Controllers;
using shukersal_backend.Controllers.StoreControllers;
using shukersal_backend.Models;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.Controllers
{
    public class ManagerControllerTests
    {
        private readonly StoreManagersController _controller;
        private readonly StoreController _storeController;
        private readonly Mock<ManagerContext> _managerContext;
        private readonly Mock<MemberContext> _memberContext;
        private readonly Mock<StoreContext> _storeContext;
        private readonly ITestOutputHelper output;
        public ManagerControllerTests(ITestOutputHelper output)
        {
            this.output = output;

            _managerContext = new Mock<ManagerContext>();
            _memberContext = new Mock<MemberContext>();
            _storeContext = new Mock<StoreContext>();

            _controller = new StoreManagersController(_managerContext.Object, _memberContext.Object, _storeContext.Object);
            _storeController = new StoreController(_storeContext.Object, _managerContext.Object, _memberContext.Object);


            var membersList = new List<Member>
            {
                new Member
                {
                    Id = 1,
                    Username = "Test",
                    Password = "password"
                },
                new Member
                {
                    Id = 2,
                    Username = "Test2",
                    Password = "password"
                },
                new Member
                {
                    Id = 3,
                    Username = "Test3",
                    Password = "password"
                },
                new Member
                {
                    Id = 4,
                    Username = "Test4",
                    Password = "password"
                }
            };
            _memberContext.Setup(m => m.Members).ReturnsDbSet(membersList);

            var p = new StorePermission
            {
                Id = 1,
                PermissionType = PermissionType.Manager_permission,
                StoreManagerId = 1
            };

            var managers = new List<StoreManager>
            {
                new StoreManager
                {
                    Id = 1,
                    MemberId = 1,
                    StoreId = 1,
                    StorePermissions = new List<StorePermission>{p}
                }
            };
            _managerContext.Setup(m => m.StoreManagers).ReturnsDbSet(managers);

            var permission = new List<StorePermission>
            {
                p
            };
            _managerContext.Setup(m => m.StorePermissions).ReturnsDbSet(permission);

            var stores = new List<Store>
            {
                new Store
                {
                    Id = 1,
                    RootManagerId = 1,
                    Name = "Test"
                }
            };
            _storeContext.Setup(s => s.Stores).ReturnsDbSet(stores);
            var products = new List<Product> { };
            _storeContext.Setup(p => p.Products).ReturnsDbSet(products);


        }

        [Fact]
        public async Task testFounder()
        {
            Assert.IsAssignableFrom<ActionResult<StoreManager>>(await _controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 2
                }));
            Assert.IsType<BadRequestObjectResult>(await _controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 1
                }));
            Assert.IsType<NotFoundResult>(await _controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 3,
                    BossId = 1,
                    StoreId = 1,
                    MemberId = 4
                }));
            Assert.IsType<NotFoundResult>(await _controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 1,
                    StoreId = 2,
                    MemberId = 2
                }));
            Assert.IsType<NotFoundResult>(await _controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 2,
                    BossId = 2,
                    StoreId = 1,
                    MemberId = 3
                }));
            Assert.IsAssignableFrom<ActionResult<StoreManager>>(await _controller.PostStoreManager(
                new OwnerManagerPost
                {
                    AppointerId = 1,
                    BossId = 2,
                    StoreId = 1,
                    MemberId = 3
                }));

        }




    }
}
