﻿using NuGet.Protocol;
using shukersal_backend.Controllers;
using shukersal_backend.Controllers.StoreControllers;
using shukersal_backend.Models;
using Xunit.Abstractions;
using System.Threading;

namespace shukersal_backend.Tests.Controllers.ManagerUseCases
{
    public class ManagerTestBase
    {
        protected readonly StoreManagersController _controller;
        protected readonly StoreController _storeController;
        protected readonly Mock<ManagerContext> _managerContext;
        protected readonly Mock<MemberContext> _memberContext;
        protected readonly Mock<StoreContext> _storeContext;
        protected readonly ITestOutputHelper output;
        public ManagerTestBase(ITestOutputHelper output)
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
            _managerContext.Setup(m => m.StoreManagers).ReturnsDbSet(managers);

            var permission = new List<StorePermission>
            {
                p1
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
    }
}
