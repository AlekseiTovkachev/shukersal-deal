
using shukersal_backend.Models;
using Xunit.Abstractions;

//{
//    public class MembersControllerTests
//    {
//        private readonly MemberService _controller;
//        private readonly Mock<MarketDbContext> _context;
//        private readonly ITestOutputHelper output;
//        public MembersControllerTests(ITestOutputHelper output)
//        {
//            this.output = output;
//            _context = new Mock<MarketDbContext>();
//            _controller = new MemberService(_context.Object);

//            var membersList = new List<Member>
//            {
//                // empty
//            };
//            _context.Setup(m => m.Members).ReturnsDbSet(membersList);

//            var shoppingCarts = new List<ShoppingCart>
//            {
//                // empty
//            };
//            _context.Setup(s => s.ShoppingCarts).ReturnsDbSet(shoppingCarts);
//        }

//        //[Fact]
//        public async Task PostMember_ValidData_ReturnsCreatedResult()
//        {
//            // Arrange
//            var memberData = new MemberPost
//            {
//                Username = "testuser",
//                Password = "testpassword"
//            };

//            // Configure mock MemberContext to return a valid collection of Member entities



            // Act
            var result = await _controller.AddMember(memberData);


//            // Assert
//            Assert.IsType<CreatedAtActionResult>(result.Result);
//        }

//        //[Fact]
//        public async Task PostMember_InvalidData_ReturnsBadRequestResult()
//        {
//            // Arrange
//            var memberData = new MemberPost
//            {
//                // Incomplete data to make it invalid
//                Username = "testuser",
//                Password = "123"
//            };


            // Act
            var result = await _controller.AddMember(memberData);

//            // Assert
//            Assert.IsType<BadRequestObjectResult>(result.Result);
//        }

//        // [Fact]
//        public async Task PostMember_NullMemberContext_ReturnsProblemResult()
//        {
//            // Arrange
//            var memberData = new MemberPost
//            {
//                Username = "testuser",
//                Password = "testpassword"
//            };

//            _context.Setup(m => m.Members).ReturnsDbSet((DbSet<Member>)null); // Set MemberContext.Members to null


            // Act
            var result = await _controller.AddMember(memberData);


//            // Assert
//            Assert.IsType<BadRequestObjectResult>(result.Result);
//        }
//    }
//}
