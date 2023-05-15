using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
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
        public void DoubleRegister_TwoThreads()
        {
            // Arrange
            bool isThread1Completed = false;
            bool isThread2Completed = false;
            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 1; i++)
                    _memberController.RegisterMember(new RegisterPost { Username = "User" + i.ToString(), Password = "Pass2" });
                isThread1Completed = true;
            });

            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 1; i++)
                    _memberController.RegisterMember(new RegisterPost { Username = "User" + i.ToString(), Password = "Pass1" });
                isThread2Completed = true;
            });

            // Act
            thread1.Start();
            thread2.Start();

            // Wait for both threads to complete
            thread1.Join();
            thread2.Join();

            // Assert
            Assert.True(isThread1Completed ^ isThread2Completed);
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

        [Fact]
        public async Task AddProduct_Multithreaded()
        {
            // Arrange
            var store = new Store
            {
                Id = 1,
                Name = "Store 1",
                Description = "Old Description",
                Products = new List<Product>(),
                RootManagerId = 1
            };

            var stores = new List<Store>
            {
                store,
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            ProductPost post = new ProductPost { Name = "Product 1", Description = "Description 1", Price = 10.00, CategoryId = 2 };
            var category = new Category { Id = post.CategoryId, Name = "Category 1" };

            _context.Setup(c => c.Stores).ReturnsDbSet(stores);
            _context.Setup(c => c.Categories).ReturnsDbSet((new List<Category> { category }).AsQueryable());
            _context.Setup(c => c.Products).ReturnsDbSet((new List<Product>()).AsQueryable());

            // Act

            for (int i = 0; i < 1; i++)
            {
                post.Name += i;
                var result1 = _controller.AddProduct(store.Id, post, dummyMember);
                var result2 = _controller.AddProduct(store.Id, post, dummyMember);
                var wres1 = await result1;
                var wres2 = await result2;

                // Assert
                Assert.True(wres1.IsSuccess && wres2.IsSuccess);
            }



            // Assert
            //Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            //Assert.Equal(category.Id, result.Result.Category.Id);
            //Assert.Equal(1, store.Products.Count);
            //_context.Verify(c => c.Products.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_Multithreaded()
        {
            // Arrange
            var existingProduct = new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "Product 1 description",
                Price = 10.99,
                UnitsInStock = 50,
                ImageUrl = "https://example.com/product1.jpg",
                Category = new Category { Id = 1, Name = "Category 1" },
                StoreId = 1,
                Store = new Store { Id = 1, Name = "Store 1" }
            };
            //_context.Setup(x => x.Products.FindAsync(1)).ReturnsAsync(existingProduct);
            _context.Setup(x => x.Products.FindAsync(existingProduct.Id)).ReturnsAsync(existingProduct);
            var patch1 = new ProductPatch
            {
                Name = "New Product Name",
                Description = "New product description",
                Price = 11.99,
                UnitsInStock = 60,
                ImageUrl = "https://example.com/newproduct.jpg",
            };
            var patch2 = new ProductPatch
            {
                Name = "New New Product Name",
                Description = "New product description",
                Price = 11.99,
                UnitsInStock = 60,
                ImageUrl = "https://example.com/newproduct.jpg",
            };

            // Act

            for (int i = 0; i < 1; i++)
            {

                var result1 = _controller.UpdateProduct(existingProduct.StoreId, existingProduct.Id, patch1, dummyMember);
                var result2 = _controller.UpdateProduct(existingProduct.StoreId, existingProduct.Id, patch2, dummyMember);
                var wres1 = await result1;
                var wres2 = await result2;

                // Assert
                Assert.NotNull(wres1.Result);
                Assert.NotNull(wres2.Result);
                Assert.True(wres1.Result.Name == patch1.Name || wres2.Result.Name == patch2.Name);
            }
        }
    }
}
