using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests
{
    public class MultiThreadTests
    {
        private readonly Mock<MarketDbContext> _context;
        private readonly StoreController _controller;
        private readonly MemberController _memberController;
        private readonly ITestOutputHelper output;
        private readonly Member dummyMember;

        //private readonly Mock<ManagerContext> _managerContextMock;
        public MultiThreadTests(ITestOutputHelper output)
        {
            this.output = output;
            _context = new Mock<MarketDbContext>();

            _context.Setup(m => m.Members).ReturnsDbSet(new List<Member>());
            _context.Setup(s => s.ShoppingCarts).ReturnsDbSet(new List<ShoppingCart>());

            var _market = new MarketObject(_context.Object);
            var _manager = new Mock<StoreManagerObject>();
            var _store = new StoreObject(_context.Object, _market, _manager.Object);
            _manager.Setup(x => x.CheckPermission(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PermissionType>()))
                    .ReturnsAsync(true);
            dummyMember = new Member
            {
                Id = 1,
                Username = "Test",
            };
            //_controller = new StoreController(_context.Object);
            _controller = new StoreController(_context.Object, _market, _store, _manager.Object);
            _memberController = new MemberController(_context.Object); 


        }

        [Fact]
        public async void DoubleRegister()
        {
            for (int i = 0; i < 1; i++)
            {
                var res1 = _memberController.RegisterMember(new RegisterPost { Username = "User" + i.ToString(), Password = "Pass1" });
                var res2 = _memberController.RegisterMember(new RegisterPost { Username = "User" + i.ToString(), Password = "Pass2" });
                var wres1 = await res1;
                var wres2 = await res2;
                Assert.True(wres1.IsSuccess ^ wres2.IsSuccess);
            }
        }
        [Fact]
        public void DoubleBasket()
        {
            var res1 = _memberController.RegisterMember(new RegisterPost { Username = "User", Password = "Pass1" });
            var res2 = _memberController.RegisterMember(new RegisterPost { Username = "User", Password = "Pass2" });
            res1.Wait();
            res2.Wait();
            Assert.True(res1.IsCompletedSuccessfully && res2.IsCompletedSuccessfully);
        }

    }
}
