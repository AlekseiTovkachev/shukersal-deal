using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.Domain
{
    public class StoreService : BaseService
    {
        public StoreService(MarketDbContext context) : base(context) { }

        public async Task<Response<IEnumerable<Store>>> GetStores()
        {
            //if (_context.Stores == null)
            //{
            //    return Response<IEnumerable<Store>>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
            //}
            var stores = await _context.Stores
                //.Include(s => s.Products)
                .Include(s => s.DiscountRules).ToListAsync();
            return Response<IEnumerable<Store>>.Success(HttpStatusCode.OK, stores);
        }


        public async Task<Response<Store>> GetStore(long id)
        {
            if (_context.Stores == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
            }
            var store = await _context.Stores
                //.Include(s => s.Products)
                .Include(s => s.DiscountRules)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (store == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Not found");
            }
            return Response<Store>.Success(HttpStatusCode.OK, store);
        }

        //TODO: check if the user is a member
        public async Task<Response<Store>> CreateStore(StorePost storeData, HttpContext httpContext)
        {
            var member = ApiControllerUtilities.GetCurrentMember(_context, httpContext);
            if (member == null)
            {
                return Response<Store>.Error(HttpStatusCode.BadRequest, "Illegal user id");
            }

            //var member = await _context.Members.FindAsync(storeData.RootManagerMemberId);


            var store = new Store
            {
                Name = storeData.Name,
                Description = storeData.Description,
                Products = new List<Product>(),
                DiscountRules = new List<DiscountRule>()
            };

            _context.Stores.Add(store);

            var storeManager = new StoreManager
            {
                MemberId = member.Id,
                Member = member,
                StoreId = store.Id,
                Store = store,
                StorePermissions = new List<StorePermission>()
            };

            _context.StoreManagers.Add(storeManager);

            var permission = new StorePermission
            {
                StoreManager = storeManager,
                StoreManagerId = storeManager.Id,
                PermissionType = PermissionType.Manager_permission
            };

            storeManager.StorePermissions.Add(permission);
            _context.StorePermissions.Add(permission);

            store.RootManager = storeManager;
            store.RootManagerId = storeManager.Id;

            await _context.SaveChangesAsync();

            return Response<Store>.Success(HttpStatusCode.Created, store);
        }


        //TODO: check if the user is a store manager (check permissions too)
        public async Task<Response<bool>> UpdateStore(long id, StorePatch patch)
        {

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }
            if (patch.Description != null) store.Description = patch.Description;
            if (patch.Name != null) store.Name = patch.Name;

            //_context.Entry(store).State = EntityState.Modified;
            _context.MarkAsModified(store);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return Response<bool>.Error(HttpStatusCode.NotFound, "not found");
                }
                else
                {
                    throw;
                }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        //TODO: check if the user is a admin
        public async Task<Response<bool>> DeleteStore(long id)
        {
            if (_context.Stores == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores' is null.");
            }
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        private bool StoreExists(long id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }

        //TODO: check if the user is a store manager (also check permissions)
        public async Task<Response<Product>> AddProduct(long storeId, ProductPost post)
        {
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
        //TODO: check if the user is a store manager (also check permissions)
        public async Task<Response<Product>> UpdateProduct(long storeId, long productId, ProductPatch patch)
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


        //TODO: check if the user is a store manager (also check permissions)
        public async Task<Response<Product>> DeleteProduct(long storeId, long productId)
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

        public async Task<Response<IEnumerable<Product>>> GetStoreProducts(long id)
        {
            if (!StoreExists(id))
            {
                return Response<IEnumerable<Product>>.Error(HttpStatusCode.NotFound, "Store not found.");
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.StoreId == id)
                .ToListAsync();

            return Response<IEnumerable<Product>>.Success(HttpStatusCode.OK, products);
        }

        public async Task<Response<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return Response<IEnumerable<Product>>.Success(HttpStatusCode.OK, products);
        }

        public async Task<Response<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Response<IEnumerable<Category>>.Success(HttpStatusCode.OK, categories);
        }


    }

}

