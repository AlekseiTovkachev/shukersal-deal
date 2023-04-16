using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shukersal_backend.Models;
using shukersal_backend.Controllers;
using shukersal_backend.Controllers.StoreControllers;
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
                // empty
            };
            _memberContext.Setup(m => m.Members).ReturnsDbSet(membersList);

            var managers = new List<StoreManager>
            {
                // empty
            };
            _managerContext.Setup(m => m.StoreManagers).ReturnsDbSet(managers);

            var stores = new List<Store>
            {
                // empty
            };
            _storeContext.Setup(s => s.Stores).ReturnsDbSet(stores);
        }

        [Fact]
        public async Task testTests()
        {
            Assert.True(true);
        }

       

        
    }
}
