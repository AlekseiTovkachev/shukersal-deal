using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.ServiceLayer;

namespace shukersal_backend.Tests.Controllers
{
    public class ShoppingCartsControllerTests
    {
        private ShoppingCartController _controller;
        private Mock<MarketDbContext> _context;

        public ShoppingCartsControllerTests()
        {
            // Mock the DbContext using Moq
            _context = new Mock<MarketDbContext>();

            // Create the controller with the mocked DbContext
            _controller = new ShoppingCartController(_context.Object);
        }


        [Fact]
        public async Task GetShoppingCartByUserId_ReturnsNotFound_ForNonexistentMemberId()
        {
            // Arrange
            long nonExistentMemberId = 999;
            var result = await _controller.GetShoppingCartByUserId(nonExistentMemberId);
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        //[Fact]
        public async Task GetShoppingCartByUserId_ReturnsShoppingCart_ForExistingMemberId()
        {
            // Arrange
            long existingMemberId = 3;
            var shoppingCart = new ShoppingCart { MemberId = existingMemberId };
            _context.Setup(db => db.ShoppingCarts)
                .ReturnsDbSet(new List<ShoppingCart> { shoppingCart });

            // Act
            var result = await _controller.GetShoppingCartByUserId(existingMemberId);
            var carts=_context.Object.ShoppingCarts.Where(c=>c.MemberId == existingMemberId).ToList();
            Assert.Single(carts);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Equal(result.Result.MemberId, existingMemberId);
            Assert.Empty(result.Result.ShoppingBaskets);

        }

     /*   //[Fact]
        public async Task AddItemToCart_ReturnsNotImplemented()
        {
            // Arrange
            long shoppingCartId = 1;
            var newItem = new ShoppingItem { ProductId = 2, Quantity = 1 };

            // Act
            var result = await _controller.AddItemToCart(shoppingCartId, newItem);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result.Result;
            Assert.Equal(StatusCodes., statusCodeResult);
        }
     */
   /*     //[Fact]
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
   */
    }
}
