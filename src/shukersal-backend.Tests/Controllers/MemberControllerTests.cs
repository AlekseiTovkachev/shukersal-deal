using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shukersal_backend.Models;
using shukersal_backend.Controllers.MemberControllers;
using shukersal_backend.Models.ShoppingCartModels;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.Controllers
{
    public class MemberControllerTests
    {
        private readonly MembersController _controller;
        private readonly Mock<MemberContext> _memberContextMock;
        private readonly Mock<ShoppingCartContext> _shoppingCartContextMock;
        private readonly ITestOutputHelper output;
        public MemberControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            _memberContextMock = new Mock<MemberContext>();
            _shoppingCartContextMock = new Mock<ShoppingCartContext>();
            _controller = new MembersController(_memberContextMock.Object, _shoppingCartContextMock.Object);
        }

        [Fact]
        public async Task PostMember_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var memberData = new MemberPost
            {
                Username = "testuser",
                Password = "testpassword"
            };

            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<ObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostMember_InvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var memberData = new MemberPost
            {
                // Incomplete data to make it invalid
                Username = "testuser"
            };
            // Seems like the API doesn't accept such values

            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostMember_NullMemberContext_ReturnsProblemResult()
        {
            // Arrange
            var memberData = new MemberPost
            {
                Username = "testuser",
                Password = "testpassword"
            };

            _memberContextMock.Setup(m => m.Members).Returns((DbSet<Member>)null); // Set MemberContext.Members to null

            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
