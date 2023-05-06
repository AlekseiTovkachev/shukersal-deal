﻿using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Objects
{
    public class MarketObject
    {
        private MarketDbContext _context;

        public MarketObject(MarketDbContext context)
        {
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

        public async Task<Response<Store>> CreateStore(StorePost storeData, Member member)
        {

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

        public async Task<Response<bool>> UpdateStore(long id, StorePatch patch, Member member)
        {

            var manager = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .FirstOrDefaultAsync(m => m.MemberId == member.Id && m.StoreId == id);

            if (manager == null)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "The user is not authorized to update store");
            }

            bool hasPermission = manager.StorePermissions.Any(p => p.PermissionType == PermissionType.Manager_permission);

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

        public async Task<Response<bool>> DeleteStore(long id, Member member)
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
    }
}