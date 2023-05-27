using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Objects
{
    public class StoreObject
    {
        private MarketDbContext _context;
        private MarketObject _market;
        private StoreManagerObject _manager;

        public StoreObject(MarketDbContext context, MarketObject market, StoreManagerObject manager)
        {
            _context = context;
            _market = market;
            _manager = manager;
        }

        public async Task<Response<Product>> AddProduct(long storeId, ProductPost post, Member member)
        {

            //var manager = await _context.StoreManagers
            //    .Include(m => m.StorePermissions)
            //    .FirstOrDefaultAsync(m => m.MemberId == member.Id && m.StoreId == storeId);

            //if (manager == null)
            //{
            //    return Response<Product>.Error(HttpStatusCode.Unauthorized, "The user is not authorized to update store");
            //}
            //bool hasPermission = manager.StorePermissions.Any(p => p.PermissionType == PermissionType.Manager_permission);
            bool hasPermission = await _manager.CheckPermission(storeId, member.Id, PermissionType.Manage_products_permission);

            if (!hasPermission)
            {
                return Response<Product>.Error(HttpStatusCode.Unauthorized, "The user is not authorized to update store");
            }

            var store = await _context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == storeId);
            if (store == null)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Store not found.");
            }
            Category? category = null;
            if (post.CategoryId != 0 && _context.Categories != null)
            {
                category = await _context.Categories
                                .FirstOrDefaultAsync(s => s.Id == post.CategoryId);
            }
            if (store == null)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Store not found.");
            }

            var product = new Product
            {
                Name = post.Name,
                Description = post.Description,
                Price = post.Price,
                ImageUrl = post.ImageUrl,
                IsListed = post.IsListed,
                UnitsInStock = post.UnitsInStock,
                Category = category,
                StoreId = storeId,
                Store = store
            };

            store.Products.Add(product);
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return Response<Product>.Success(HttpStatusCode.Created, product);
        }

        public async Task<Response<Product>> UpdateProduct(long storeId, long productId, ProductPatch patch, Member member)
        {
            var existingProduct = await _context.Products.FindAsync(productId);

            if (existingProduct == null || existingProduct.StoreId != storeId)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Product not found in the specified store.");
            }
            Category? category = null;
            if (patch.CategoryId != -1 && _context.Categories != null)
            {
                category = await _context.Categories.FindAsync(patch.CategoryId);
            }

            // Update the existing product with the new data
            if (patch.Name != null) existingProduct.Name = patch.Name;

            if (patch.Description != null) existingProduct.Description = patch.Description;

            if (patch.Price != -1) existingProduct.Price = patch.Price;

            if (patch.UnitsInStock != -1) existingProduct.UnitsInStock = patch.UnitsInStock;

            if (patch.ImageUrl != null) existingProduct.ImageUrl = patch.ImageUrl;

            if (category != null) existingProduct.Category = category;


            //_context.Entry(existingProduct).State = EntityState.Modified;
            _context.MarkAsModified(existingProduct);
            await _context.SaveChangesAsync();

            return Response<Product>.Success(HttpStatusCode.NoContent, existingProduct);
        }

        public async Task<Response<Product>> DeleteProduct(long storeId, long productId, Member member)
        {
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Store not found.");
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null || product.StoreId != storeId)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Product not found in the specified store.");
            }
            store.Products.Remove(product);
            // Remove the product from the store
            _context.Products.Remove(product);
            //_context.Entry(store).State = EntityState.Modified;
            _context.MarkAsModified(store);
            await _context.SaveChangesAsync();

            return Response<Product>.Success(HttpStatusCode.NotFound, product);
        }

        public async Task<Response<IEnumerable<Product>>> GetProducts(long storeId)
        {
            var response = await _market.GetStore(storeId);
            if (!response.IsSuccess || response.Result == null)
            {
                return Response<IEnumerable<Product>>.Error(response);
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.StoreId == storeId)
                .ToListAsync();

            return Response<IEnumerable<Product>>.Success(HttpStatusCode.OK, products);
        }

        public async Task<Response<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Response<IEnumerable<Product>>.Success(HttpStatusCode.OK, products);
        }

        public async Task<Response<Product>> GetProduct(long id)
        {

            var product = await _context.Products
                .FindAsync(id);
            if (product == null)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Not found");
            }
            return Response<Product>.Success(HttpStatusCode.OK, product);
        }
    }
}
