using System.Net;

namespace shukersal_backend.Domain
{
    public class StoreService
    {
        private readonly StoreContext _context;
        //private readonly ManagerContext _managerContext;
        //private readonly ManagerContext _managerContext;
        private readonly MemberContext _memberContext;

        public StoreService(StoreContext context, ManagerContext managerContext, MemberContext memberContext)
        {
            _context = context;
            //_managerContext = managerContext;
            _memberContext = memberContext;

            _context.Database.EnsureCreated();
        }

        public StoreService(StoreContext context, ManagerContext managerContext, MemberContext memberContext, string test)
        {
            _context = context;
            //_managerContext = managerContext;
            _memberContext = memberContext;
        }


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

        public async Task<Response<Store>> CreateStore(StorePost storeData)
        {
            var member = await _memberContext.Members.FindAsync(storeData.RootManagerMemberId);
            if (member == null)
            {
                return Response<Store>.Error(HttpStatusCode.BadRequest, "Illegal user id");
            }

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
                //Member = member,
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



        public async Task<Response<bool>> UpdateStore(long id, StorePost post)
        {

            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }
            store.Description = post.Description;
            store.Name = post.Name;
            //var rootManager = await _managerContext.StoreManagers.FindAsync(store.RootManagerId);
            //store.RootManager = rootManager;

            _context.Entry(store).State = EntityState.Modified;

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

        public async Task<Response<bool>> DeleteStore(long id)
        {
            if (_context.Stores == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
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

        //public async Task<Response<Product>> UpdateProduct(long storeId, long productId, ProductPost post)
        //{
        //    var existingProduct = await _context.Products.FindAsync(productId);

        //    if (existingProduct == null || existingProduct.StoreId != storeId)
        //    {
        //        return Response<Product>.Error(HttpStatusCode.NotFound, "Product not found in the specified store.");
        //    }
        //    Category? category = null;
        //    if (post.CategoryId != 0 && _context.Categories != null)
        //    {
        //        category = await _context.Categories.FindAsync(post.CategoryId);
        //    }


        //    // Update the existing product with the new data
        //    if (post.Name != null) existingProduct.Name = post.Name;

        //    if (post.Description != null) existingProduct.Description = post.Description;

        //    if (category != null) existingProduct.Category = category;

        //    if (post.Price != -1) existingProduct.Price = post.Price;

        //    existingProduct.UnitsInStock = post.UnitsInStock;

        //    _context.Entry(existingProduct).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return Response<Product>.Success(HttpStatusCode.NoContent, existingProduct);
        //}

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


            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Response<Product>.Success(HttpStatusCode.NoContent, existingProduct);
        }



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
            _context.Entry(store).State = EntityState.Modified;
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

