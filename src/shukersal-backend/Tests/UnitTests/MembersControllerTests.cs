using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NuGet.ContentModel;
using shukersal_backend.Controllers.MemberControllers;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
using Xunit;

namespace shukersal_backend.Tests.UnitTests
{
    public class MembersControllerTests
    {
        private readonly MembersController _controller;
        private readonly Mock<MemberContext> _mockMemberContext;
        private readonly Mock<ShoppingCartContext> _mockShoppingCartContext;

        public MembersControllerTests()
        {
            //_mockMemberContext = new Mock<MemberContext>();
            //_mockShoppingCartContext = new Mock<ShoppingCartContext>();
            //_controller = new MembersController(_mockMemberContext.Object, _mockShoppingCartContext.Object);
            
            // Set up the in-memory database for MemberContext
            var memberOptions = new DbContextOptionsBuilder<MemberContext>()
                .UseInMemoryDatabase(databaseName: "TestMemberDatabase")
                .Options;

            // Create a new instance of the MemberContext using the in-memory database options
            var memberDbContext = new MemberContext(memberOptions);

            // Set up the mock member context to use the in-memory database instance
            _mockMemberContext = new Mock<MemberContext>(memberDbContext);

            // Set up the in-memory database for ShoppingCartContext
            var shoppingCartOptions = new DbContextOptionsBuilder<ShoppingCartContext>()
                .UseInMemoryDatabase(databaseName: "TestShoppingCartDatabase")
                .Options;

            // Create a new instance of the ShoppingCartContext using the in-memory database options
            var shoppingCartDbContext = new ShoppingCartContext(shoppingCartOptions);

            // Set up the mock shopping cart context to use the in-memory database instance
            _mockShoppingCartContext = new Mock<ShoppingCartContext>(shoppingCartDbContext);

            _controller = new MembersController(_mockMemberContext.Object, _mockShoppingCartContext.Object);
        }


        [Fact]
        public async Task GetMembers_ReturnsOkResult_WithMembersList()
        {
            // Arrange
            var membersList = new List<Member>()
            {
                new Member { Id = 1, Username = "user1", Password = "password1" },
                new Member { Id = 2, Username = "user2", Password = "password2" },
            }.AsQueryable();

            _mockMemberContext.Setup(c => c.Members).ReturnsDbSet(membersList);

            // Act
            var result = await _controller.GetMembers();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var members = okResult.Value as IEnumerable<Member>;
            Assert.NotNull(members);
            Assert.Equal(2, members.Count());
        }

        [Fact]
        public async Task GetMember_WithExistingId_ReturnsOkResult()
        {
            // Arrange
            var member = new Member { Id = 1, Username = "user1", Password = "password1" };
            _mockMemberContext.Setup(c => c.Members.FindAsync(It.IsAny<long>())).ReturnsAsync(member);

            // Act
            var result = await _controller.GetMember(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetMember_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            _mockMemberContext.Setup(c => c.Members.FindAsync(It.IsAny<long>())).ReturnsAsync((Member)null);

            // Act
            var result = await _controller.GetMember(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostMember_WithValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var memberData = new MemberPost { Username = "user1", Password = "password1" };
            _mockMemberContext.Setup(c => c.Members.Add(It.IsAny<Member>()));
            _mockShoppingCartContext.Setup(c => c.ShoppingCarts.Add(It.IsAny<ShoppingCart>()));
            _mockMemberContext.Setup(c => c.SaveChangesAsync(default)).Returns(Task.FromResult(0));
            _mockShoppingCartContext.Setup(c => c.SaveChangesAsync(default)).Returns(Task.FromResult(0));

            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

    }
}
