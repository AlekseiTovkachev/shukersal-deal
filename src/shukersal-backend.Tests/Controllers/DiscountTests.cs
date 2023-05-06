using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shukersal_backend.Tests.Controllers
{
    public class DiscountTests
    {
        private DiscountObject _object;
        private Mock<MarketDbContext> _context;
        private Store store;
        private ICollection<ShoppingItem> items;

        public DiscountTests()
        {
            // Mock the DbContext using Moq
            _context = new Mock<MarketDbContext>();

            // Create the controller with the mocked DbContext
            _object = new DiscountObject(_context.Object);
            store = new Store
            {
                Id = 1,
                Name = ""
            };
            items = new List<ShoppingItem>(){
                new ShoppingItem{Product = new Product{Price = 10, Name ="p1"}}
            };
        }


        [Fact]
        public async Task SimpleDiscount()
        {
            // Arrange
            var discountPost = new DiscountRulePost {
                Id = 1,
                discountType = DiscountType.SIMPLE,
                discountOn = DiscountOn.PRODUCT,
                discountOnString = "p1",
                Discount = 10 
            };
            // Act
            var discount = await _object.CreateDiscount(discountPost, store);
            double result = _object.CalculateDiscount(discount.Result, items);

            // Assert
            Assert.Equal(result,1.0);
        }

        //[Fact]
        public async Task GetShoppingCartByUserId_ReturnsShoppingCart_ForExistingMemberId()
        {
            // Arrange
            long existingMemberId = 1;
            var shoppingCart = new ShoppingCart { MemberId = existingMemberId };
            _context.Setup(db => db.ShoppingCarts)
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
