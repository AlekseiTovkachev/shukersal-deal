using shukersal_backend.Controllers.MemberControllers;
using shukersal_backend.Models;
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

            var membersList = new List<Member>
            {
                // empty
            };
            _memberContextMock.Setup(m => m.Members).ReturnsDbSet(membersList);

            var shoppingCarts = new List<ShoppingCart>
            {
                // empty
            };
            _shoppingCartContextMock.Setup(s => s.ShoppingCarts).ReturnsDbSet(shoppingCarts);
        }

        //[Fact]
        public async Task PostMember_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var memberData = new MemberPost
            {
                Username = "testuser",
                Password = "testpassword"
            };

            // Configure mock MemberContext to return a valid collection of Member entities


            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        //[Fact]
        public async Task PostMember_InvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var memberData = new MemberPost
            {
                // Incomplete data to make it invalid
                Username = "testuser",
                Password = "123"
            };

            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // [Fact]
        public async Task PostMember_NullMemberContext_ReturnsProblemResult()
        {
            // Arrange
            var memberData = new MemberPost
            {
                Username = "testuser",
                Password = "testpassword"
            };

            _memberContextMock.Setup(m => m.Members).ReturnsDbSet((DbSet<Member>)null); // Set MemberContext.Members to null

            // Act
            var result = await _controller.PostMember(memberData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
