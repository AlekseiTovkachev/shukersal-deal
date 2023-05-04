using FluentAssertions;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.ServiceTests
{
    public class StoreServiceTests
    {
        private readonly Mock<MarketDbContext> _context;
        private readonly StoreController _controller;
        private readonly ITestOutputHelper output;

        //private readonly Mock<ManagerContext> _managerContextMock;
        public StoreServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            _context = new Mock<MarketDbContext>();
            _controller = new StoreController(_context.Object);
        }

        [Fact]
        public async Task GetStores_ReturnsListOfStores()
        {
            // Arrange
            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            _context.Setup(c => c.Stores).ReturnsDbSet(stores);


            // Act
            var response = await _controller.GetStores();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, response.Result.Count());
            Assert.Equal("Store 1", response.Result.ElementAt(0).Name);
            Assert.Equal("Store 2", response.Result.ElementAt(1).Name);
            Assert.Equal("Store 3", response.Result.ElementAt(2).Name);
        }

        [Fact]
        public async Task GetStore_WithExistingStore_ReturnsSuccessResponseWithStore()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Description 1", RootManagerId = 1 };
            var stores = new List<Store>
            {
                store,
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();
            _context.Setup(c => c.Stores).ReturnsDbSet(stores);

            // Act
            var response = await _controller.GetStore(store.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(store, response.Result);
        }

        [Fact]
        public async Task GetStore_WithNonExistingStore_ReturnsErrorResponseWithNotFoundStatusCode()
        {
            // Arrange
            long nonExistingStoreId = 1;
            _context.Setup(c => c.Stores).ReturnsDbSet(new List<Store>().AsQueryable());

            // Act
            var response = await _controller.GetStore(nonExistingStoreId);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(response.Result);
        }

        [Fact]
        public async Task CreateStore_ValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var storeData = new StorePost
            {
                Name = "Test Store",
                Description = "This is a test store.",
                RootManagerMemberId = 1
            };
            var membersList = new List<Models.Member>
            {
                new Models.Member { Id = storeData.RootManagerMemberId }
            }.AsQueryable();

            _context.Setup(m => m.Members).ReturnsDbSet(membersList);
            var member = new Models.Member { Id = storeData.RootManagerMemberId };
            _context.Setup(m => m.Members.FindAsync(storeData.RootManagerMemberId)).ReturnsAsync(member);
            _context.Setup(c => c.Stores).ReturnsDbSet(new List<Store>().AsQueryable());
            _context.Setup(c => c.StorePermissions).ReturnsDbSet(new List<StorePermission>().AsQueryable());
            _context.Setup(c => c.StoreManagers).ReturnsDbSet(new List<StoreManager>().AsQueryable());
            // Act
            var response = await _controller.CreateStore(storeData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Result);
            Assert.Equal(storeData.Name, response.Result.Name);
            Assert.Equal(storeData.Description, response.Result.Description);
        }

        [Fact]
        public async Task CreateStore_InvalidMemberId_ReturnsErrorResponse()
        {
            // Arrange
            var storeData = new StorePost
            {
                Name = "Test Store",
                Description = "This is a test store.",
                RootManagerMemberId = 1
            };
            _context.Setup(m => m.Members).ReturnsDbSet(new List<Models.Member>().AsQueryable());
            // Act
            var response = await _controller.CreateStore(storeData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(response.Result);
        }

        [Fact]
        public async Task UpdateStore_ValidRequest_ReturnsSuccessWithTrue()
        {
            // Arrange
            long id = 1;
            var patch = new StorePatch
            {
                Name = "New Name",
                Description = "New Description"
            };
            var store = new Store { Id = id, Name = "Old Name", Description = "Old Description" };
            _context.Setup(x => x.Stores.FindAsync(id)).ReturnsAsync(store);
            _context.Setup(x => x.MarkAsModified(store)).Verifiable();
            // Act
            var result = await _controller.UpdateStore(id, patch);

            // Assert
            result.Should().BeOfType<Response<bool>>();
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            result.IsSuccess.Should().BeTrue();
            store.Name.Should().Be(patch.Name);
            store.Description.Should().Be(patch.Description);
        }

        [Fact]
        public async Task UpdateStore_StoreNotFound_ReturnsNotFound()
        {
            // Arrange
            long id = 1;
            var patch = new StorePatch
            {
                Name = "New Name",
                Description = "New Description"
            };
            _context.Setup(c => c.Stores).ReturnsDbSet(new List<Store>().AsQueryable());


            // Act
            var result = await _controller.UpdateStore(id, patch);

            // Assert
            result.Should().BeOfType<Response<bool>>();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.ErrorMessage.Should().Be("Store not found");
        }

        [Fact]
        public async Task UpdateStore_UpdatesOnlyName_WhenPatchContainsOnlyName()
        {
            // Arrange
            var patch = new StorePatch { Name = "New Name" };
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", RootManagerId = 1 };
            var stores = new List<Store>
            {
                store,
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            _context.Setup(c => c.Stores.FindAsync(store.Id)).ReturnsAsync(store);
            _context.Setup(x => x.MarkAsModified(store)).Verifiable();

            // Act
            var response = await _controller.UpdateStore(store.Id, patch);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(response.Result);

            Assert.Equal(patch.Name, store.Name);
            Assert.Equal("Old Description", store.Description);
        }

        [Fact]
        public async Task DeleteStore_WhenStoreExists_DeletesStore()
        {
            // Arrange
            var store = new Store { Id = 1 };
            _context.Setup(c => c.Stores.FindAsync(store.Id)).ReturnsAsync(store);


            // Act
            var response = await _controller.DeleteStore(store.Id);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(response.Result);
        }

        [Fact]
        public async Task DeleteStore_WhenStoreDoesNotExist_ReturnsErrorResponse()
        {
            // Arrange
            var storeId = 1;
            _context.Setup(c => c.Stores.FindAsync(storeId)).ReturnsAsync((Store)null);

            // Act
            var response = await _controller.DeleteStore(storeId);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Store not found", response.ErrorMessage);
        }

        [Fact]
        public async Task DeleteStore_WhenStoreContextIsNull_ReturnsErrorResponse()
        {
            // Arrange
            _context.SetupGet(c => c.Stores).Returns((DbSet<Store>)null);


            // Act
            var response = await _controller.DeleteStore(1);

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Entity set 'StoreContext.Stores' is null.", response.ErrorMessage);
        }

        [Fact]
        public async Task GetStoreProducts_ReturnsProducts_WhenStoreExists()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", RootManagerId = 1 };
            var products = new List<Product>
            {
            new Product { Id = 1, Name = "Product 1", StoreId = store.Id },
            new Product { Id = 2, Name = "Product 2", StoreId = store.Id }
            };

            var stores = new List<Store>
            {
                store,
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            _context.Setup(c => c.Stores).ReturnsDbSet(stores);
            _context.Setup(c => c.Products).ReturnsDbSet(products.AsQueryable());

            // Act
            var response = await _controller.GetStoreProducts(store.Id);

            // Assert
            Assert.NotNull(response.Result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(products.Count, response.Result.Count());
            foreach (var product in products)
            {
                Assert.Contains(response.Result, p => p.Id == product.Id && p.Name == product.Name && p.StoreId == product.StoreId);
            }
        }

        [Fact]
        public async Task GetStoreProducts_ReturnsNotFound_WhenStoreDoesNotExist()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", RootManagerId = 1 };


            var stores = new List<Store>
            {
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            _context.Setup(c => c.Stores).ReturnsDbSet(stores);
            _context.Setup(c => c.Products).ReturnsDbSet((new List<Product>()).AsQueryable());

            // Act
            var response = await _controller.GetStoreProducts(store.Id);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(response.Result);
            Assert.Equal("Store not found.", response.ErrorMessage);
        }

        [Fact]
        public async Task AddProduct_WhenStoreNotFound_ReturnsError()
        {
            // Arrange
            long storeId = 1;
            ProductPost post = new ProductPost { Name = "Product 1", Description = "Description 1", Price = 10.00 };
            //_context.Setup(c => c.Stores.FirstOrDefaultAsync(s => s.Id == storeId)).ReturnsAsync((Store)null);
            _context.Setup(c => c.Stores).ReturnsDbSet((new List<Store>()).AsQueryable());
            // Act
            var response = await _controller.AddProduct(storeId, post);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Store not found.", response.ErrorMessage);
        }

        [Fact]
        public async Task AddProduct_WhenCategoryNotFound_ReturnsSuccessWithNullCategory()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", Products = new List<Product>(), RootManagerId = 1 };

            var stores = new List<Store>
            {
                store,
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            }.AsQueryable();

            _context.Setup(c => c.Stores).ReturnsDbSet(stores);
            _context.Setup(c => c.Products).ReturnsDbSet((new List<Product>()).AsQueryable());


            ProductPost post = new ProductPost { Name = "Product 1", Description = "Description 1", Price = 10.00, CategoryId = 0 };

            //_context.Setup(c => c.Stores.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == storeId)).ReturnsAsync(store);
            //_context.Setup(c => c.Categories.FirstOrDefaultAsync(s => s.Id == post.CategoryId)).ReturnsAsync((Category)null);

            // Act
            var response = await _controller.AddProduct(store.Id, post);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Null(response.Result.Category);
            Assert.Equal(1, store.Products.Count);
            _context.Verify(c => c.Products.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task AddProduct_WhenStoreAndCategoryFound_ReturnsSuccessWithCategory()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", Products = new List<Product>(), RootManagerId = 1 };

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
            var result = await _controller.AddProduct(store.Id, post);

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(category.Id, result.Result.Category.Id);
            Assert.Equal(1, store.Products.Count);
            _context.Verify(c => c.Products.Add(It.IsAny<Product>()), Times.Once);
        }


        [Fact]
        public async Task UpdateProduct_WhenProductExists_ReturnsSuccess()
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
            var patch = new ProductPatch
            {
                Name = "New Product Name",
                Description = "New product description",
                Price = 11.99,
                UnitsInStock = 60,
                ImageUrl = "https://example.com/newproduct.jpg",
            };

            // Act
            var result = await _controller.UpdateProduct(existingProduct.StoreId, existingProduct.Id, patch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Equal(patch.Name, existingProduct.Name);
            Assert.Equal(patch.Description, existingProduct.Description);
            Assert.Equal(patch.Price, existingProduct.Price);
            Assert.Equal(patch.UnitsInStock, existingProduct.UnitsInStock);
            Assert.Equal(patch.ImageUrl, existingProduct.ImageUrl);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var productId = 1;
            var storeId = 1;
            _context.Setup(x => x.Products.FindAsync(productId)).ReturnsAsync((Product)null);

            var patch = new ProductPatch
            {
                Name = "New Product Name",
                Description = "New product description",
                Price = 11.99,
                UnitsInStock = 60,
                ImageUrl = "https://example.com/newproduct.jpg",
                CategoryId = 2
            };

            // Act
            var result = await _controller.UpdateProduct(storeId, productId, patch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductNotFoundInSpecifiedStore_ReturnsNotFound()
        {
            // Arrange
            var productId = 1;
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", Products = new List<Product>(), RootManagerId = 1 };

            var patch = new ProductPatch
            {
                Name = "New Name",
                Description = "New Description",
                Price = 10,
                UnitsInStock = 20,
                ImageUrl = "http://new-image.com",
            };

            var product = new Product
            {
                Id = productId,
                StoreId = store.Id + 1, // product belongs to a different store
                Name = "Old Name",
                Description = "Old Description",
                Price = 5,
                UnitsInStock = 10,
                ImageUrl = "http://old-image.com",
            };

            _context.Setup(x => x.Products.FindAsync(productId)).ReturnsAsync(product);
            _context.Setup(x => x.Stores.FindAsync(store.Id)).ReturnsAsync(store);
            // Act
            var result = await _controller.UpdateProduct(store.Id, productId, patch);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found in the specified store.", result.ErrorMessage);
        }


        [Fact]
        public async Task DeleteProduct_WithValidStoreAndProduct_ReturnsSuccessResponse()
        {
            // Arrange
            var store = new Store { Id = 1, Name = "Store 1", Description = "Old Description", Products = new List<Product>(), RootManagerId = 1 };

            var product = new Product { Id = 1, StoreId = store.Id };
            store.Products.Add(product);

            var products = new List<Product> { product };
            _context.Setup(c => c.Products).ReturnsDbSet(products.AsQueryable());
            _context.Setup(x => x.Stores.FindAsync(store.Id)).ReturnsAsync(store);
            _context.Setup(x => x.Products.FindAsync(product.Id)).ReturnsAsync(product);

            _context.Setup(x => x.MarkAsModified(store)).Verifiable();
            //_context.Setup(x => x.SaveChangesAsync());

            // Act
            var result = await _controller.DeleteProduct(store.Id, product.Id);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(product, result.Result);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidStore_ReturnsErrorResponse()
        {
            // Arrange
            var storeId = 1;
            var productId = 1;
            _context.Setup(x => x.Stores.FindAsync(storeId)).ReturnsAsync((Store)null);

            // Act
            var result = await _controller.DeleteProduct(storeId, productId);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Store not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidProduct_ReturnsErrorResponse()
        {
            // Arrange
            var store = new Store { Id = 1 };
            var product = new Product { Id = 1, StoreId = 2 };
            _context.Setup(x => x.Stores.FindAsync(store.Id)).ReturnsAsync(store);
            _context.Setup(x => x.Products.FindAsync(product.Id)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct(store.Id, product.Id);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found in the specified store.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetAllProducts_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var products = new List<Product>
            {
            new Product { Id = 1, Name = "Product 1", Category = new Category { Id = 1, Name = "Category 1" } },
            new Product { Id = 2, Name = "Product 2", Category = new Category { Id = 2, Name = "Category 2" } }
            };
            _context.Setup(x => x.Products).ReturnsDbSet(products.AsQueryable());


            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(products, result.Result);
        }

        [Fact]
        public async Task GetCategories_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var categories = new List<Category>
            {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
            };

            _context.Setup(x => x.Categories).ReturnsDbSet(categories.AsQueryable());
            // Act
            var result = await _controller.GetStoreCategories();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(categories, result.Result);
        }
    }


}
