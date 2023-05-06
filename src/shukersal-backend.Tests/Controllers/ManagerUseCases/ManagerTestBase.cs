using NuGet.Protocol;
using shukersal_backend.Models;
using Xunit.Abstractions;
using System.Threading;
using shukersal_backend.ServiceLayer;

namespace shukersal_backend.Tests.Controllers.ManagerUseCases
{
    public class ManagerTestBase
    {
        protected readonly StoreManagerService _controller;
        protected readonly StoreService _storeController;
        protected readonly Mock<MarketDbContext> _context;
        protected readonly ITestOutputHelper output;
        public ManagerTestBase(ITestOutputHelper output)
        {
            this.output = output;

            _context = new Mock<MarketDbContext>();
            
            _controller = new StoreManagersController(_context.Object);
            _storeController = new StoreService(_context.Object);


            var membersList = new List<Member>
            {
                new Member
                {
                    Id = 1,
                    Username = "Test",
                    PasswordHash = "password"
                },
                new Member
                {
                    Id = 2,
                    Username = "Test2",
                    PasswordHash = "password"
                },
                new Member
                {
                    Id = 3,
                    Username = "Test3",
                    PasswordHash = "password"
                },
                new Member
                {
                    Id = 4,
                    Username = "Test4",
                    PasswordHash = "password"
                }
            };
            _context.Setup(m => m.Members).ReturnsDbSet(membersList);

            var p1 = new StorePermission
            {
                Id = 1,
                PermissionType = PermissionType.Manager_permission,
                StoreManagerId = 1
            };
            var p2 = new StorePermission
            {
                Id = 2,
                PermissionType = PermissionType.Get_history_permission,
                StoreManagerId = 2
            };
            var p3 = new StorePermission
            {
                Id = 3,
                PermissionType = PermissionType.Reply_permission,
                StoreManagerId = 2
            };

            var managers = new List<StoreManager>
            {
                new StoreManager
                {
                    Id = 1,
                    MemberId = 1,
                    StoreId = 1,
                    StorePermissions = new List<StorePermission>{p1}
                },
                new StoreManager
                {
                    Id = 2,
                    MemberId = 2,
                    StoreId = 1,
                    StorePermissions = new List<StorePermission>{p2, p3}
                }
            };
            _context.Setup(m => m.StoreManagers).ReturnsDbSet(managers);

            var permission = new List<StorePermission>
            {
                p1
            };
            _context.Setup(m => m.StorePermissions).ReturnsDbSet(permission);

            var stores = new List<Store>
            {
                new Store
                {
                    Id = 1,
                    RootManagerId = 1,
                    Name = "Test"
                }
            };
            _context.Setup(s => s.Stores).ReturnsDbSet(stores);
            var products = new List<Product> { };
            _context.Setup(p => p.Products).ReturnsDbSet(products);


        }
    }
}
