using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.Domain
{
    public class StoreService
    {
        private readonly StoreContext _context;
        private readonly ManagerContext _managerContext;
        private readonly MemberContext _memberContext;

        public StoreService(StoreContext context, ManagerContext managerContext, MemberContext memberContext)
        {
            _context = context;
            _managerContext = managerContext;
            _memberContext = memberContext;
        }

        public async Task<Response<IEnumerable<Store>>> GetStores()
        {
            if (_context.Stores == null)
            {
                return Response<IEnumerable<Store>>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
            }
            var stores = await _context.Stores
                .Include(s => s.Products)
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
                .Include(s => s.Products)
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
            if (_context.Stores == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'StoreContext.Stores'  is null.");
            }
            if (_managerContext.StoreManagers == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_managerContext.StorePermissions == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'ManagerContext.StorePermissions'  is null.");
            }
            if (_memberContext.Members == null)
            {
                return Response<Store>.Error(HttpStatusCode.NotFound, "Entity set 'MemberContext.Members'  is null.");
            }

            var store = new Store
            {
                Name = storeData.Name,
                Description = storeData.Description,
                RootManagerId = storeData.RootManagerMemberId,
                RootManager = null,
                Products = new List<Product>(),
                DiscountRules = new List<DiscountRule>()
            };

            var member = await _memberContext.Members.FindAsync(storeData.RootManagerMemberId);
            if (member == null)
            {
                return Response<Store>.Error(HttpStatusCode.BadRequest, "Illegal user id");
            }
            var storeManager = new StoreManager
            {
                StoreId = store.Id,
                Store = store,
                MemberId = member.Id,
                Member = member,
                StorePermissions = new List<StorePermission>()
            };
            storeManager.StorePermissions.Add(new StorePermission
            {
                StoreManager = storeManager,
                StoreManagerId = storeManager.Id,
                PermissionType = PermissionType.Manager_permission
            });

            store.RootManager = storeManager;

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            _managerContext.StoreManagers.Add(storeManager);
            await _managerContext.SaveChangesAsync();
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

        public async Task<Response<Product>> AddProduct(long storeId, Product product)
        {
            var store = await _context.Stores.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == storeId);
            if (store == null)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Store not found.");
            }

            // Associate the product with the store
            product.StoreId = storeId;
            product.Store = store;

            store.Products.Add(product);
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return Response<Product>.Success(HttpStatusCode.Created, product);
        }

        public async Task<Response<Product>> UpdateProduct(long storeId, long productId, Product product)
        {
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Store not found.");
            }

            var existingProduct = await _context.Products.FindAsync(productId);

            if (existingProduct == null || existingProduct.StoreId != storeId)
            {
                return Response<Product>.Error(HttpStatusCode.NotFound, "Product not found in the specified store.");
            }

            // Update the existing product with the new data
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            _context.Entry(store).State = EntityState.Modified;
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
    }

}

