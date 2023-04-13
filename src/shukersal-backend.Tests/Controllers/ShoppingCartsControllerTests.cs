using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shukersal_backend.Controllers.ShoppingCartControllers;
using shukersal_backend.Models.ShoppingCartModels;


namespace shukersal_backend.Tests.Controllers
{
    public class ShoppingCartsControllerTests
    {
        private ShoppingCartsController _controller;
        private Mock<ShoppingCartContext> _mockDbContext;

        public ShoppingCartsControllerTests()
        {
            // Mock the DbContext using Moq
            _mockDbContext = new Mock<ShoppingCartContext>();

            // Create the controller with the mocked DbContext
            _controller = new ShoppingCartsController(_mockDbContext.Object);
        }


        //[Fact]
        public async Task GetShoppingCartByUserId_ReturnsNotFound_ForNonexistentMemberId()
        {
            // Arrange
            long nonExistentMemberId = 999;
            _mockDbContext.Setup(db => db.ShoppingCarts)
                .ReturnsDbSet(new List<ShoppingCart>());

            // Act
            var result = await _controller.GetShoppingCartByUserId(nonExistentMemberId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //[Fact]
        public async Task GetShoppingCartByUserId_ReturnsShoppingCart_ForExistingMemberId()
        {
            // Arrange
            long existingMemberId = 1;
            var shoppingCart = new ShoppingCart { MemberId = existingMemberId };
            _mockDbContext.Setup(db => db.ShoppingCarts)
                .ReturnsDbSet(new List<ShoppingCart> { shoppingCart });

            // Act
            var result = await _controller.GetShoppingCartByUserId(existingMemberId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(shoppingCart, okResult.Value);
        }

        //[Fact]
        public async Task AddItemToCart_ReturnsNotImplemented()
        {
            // Arrange
            long shoppingCartId = 1;
            var newItem = new ShoppingItem { ProductId = 2, Quantity = 1 };

            // Act
            var result = await _controller.AddItemToCart(shoppingCartId, newItem);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(StatusCodes.Status501NotImplemented, statusCodeResult.StatusCode);
        }

        //[Fact]
        public async Task RemoveItemFromCart_ReturnsNotImplemented()
        {
            // Arrange
            long shoppingCartId = 1;
            long itemId = 1;

            // Act
            var result = await _controller.RemoveItemFromCart(shoppingCartId, itemId);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(StatusCodes.Status501NotImplemented, statusCodeResult.StatusCode);
        }
    }
}
