using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using shukersal_backend.Controllers.ShoppingCartControllers;
using shukersal_backend.Models.ShoppingCartModels;
using Xunit;

namespace shukersal_backend.Tests.Controllers
{
    public class ShoppingCartsControllerTests
    {
        private ShoppingCartsController _controller;
        private Mock<ShoppingCartContext> _mockDbContext;

        public ShoppingCartsControllerTests()
        {
            // Mock the DbContext using Moq
            //_mockDbContext = new Mock<ShoppingCartContext>();

            // Create the controller with the mocked DbContext
            //_controller = new ShoppingCartsController(_mockDbContext.Object);
            // Create a mock instance of the ShoppingCartContext
            _mockDbContext = new Mock<ShoppingCartContext>();

            // Set up the in-memory database
            var options = new DbContextOptionsBuilder<ShoppingCartContext>()
                .UseInMemoryDatabase(databaseName: "TestShoppingCartDatabase")
                .Options;

            // Create a new instance of the ShoppingCartContext using the in-memory database options
            var dbContext = new ShoppingCartContext(options);

            // Set up the mock context to use the in-memory database instance
            _mockDbContext.Setup(c => c.ShoppingCarts).Returns(dbContext.ShoppingCarts);
            _mockDbContext.Setup(c => c.ShoppingBaskets).Returns(dbContext.ShoppingBaskets);
            _mockDbContext.Setup(c => c.ShoppingItems).Returns(dbContext.ShoppingItems);

            // Create an instance of the ShoppingCartsController using the mock context
            _controller = new ShoppingCartsController(_mockDbContext.Object);
        }
    

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
