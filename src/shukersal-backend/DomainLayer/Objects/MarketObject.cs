using Microsoft.EntityFrameworkCore;
using System.Net;
using shukersal_backend.Models;
using shukersal_backend.Utility;

namespace shukersal_backend.DomainLayer.Objects
{
    public class MarketObject
    {
        private MarketDbContext _context;

        public MarketObject(MarketDbContext context) {
            _context = context;
        }
        public async Task<Response<IEnumerable<Store>>> GetStores()
        {
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
            var member = await _context.Members.FindAsync(storeData.RootManagerMemberId);
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

        public async Task<Response<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Response<IEnumerable<Category>>.Success(HttpStatusCode.OK, categories);
        }

        private bool StoreExists(long id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }


        #region Transaction Functionality
        // *------------------------------------------------- Transaction Functionality --------------------------------------------------*

        public bool TransactionExists(long id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        #endregion
    }
}
