using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;


namespace shukersal_backend.DomainLayer.Objects
{
    public class StoreManagerObject
    {
        private MarketDbContext _context;

        public StoreManagerObject(MarketDbContext context)
        {
            _context = context;
        }

        public StoreManagerObject()
        {

        }


        public virtual async Task<bool> CheckPermission(long storeId, long memberId, PermissionType type)
        {
            var manager = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .FirstOrDefaultAsync(m => m.MemberId == memberId && m.StoreId == storeId);

            if (manager == null || manager.StorePermissions == null)
            {
                return false;
            }
            bool hasPermission = manager.StorePermissions
                .Any(p => p.PermissionType == PermissionType.Manager_permission || p.PermissionType == type);

            return hasPermission;
        }

        public async Task<Response<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            var managers = await _context.StoreManagers.ToListAsync();

            return Response<IEnumerable<StoreManager>>.Success(HttpStatusCode.OK, managers);
        }

        public async Task<Response<StoreManager>> GetStoreManager(long id)
        {
            //addition for testing
            var storeManager = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (storeManager == null)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }

            return Response<StoreManager>.Success(HttpStatusCode.OK, storeManager);
        }


        public async Task<Response<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {

            bool isManagerOfStore = _context.StoreManagers.Any(sm => sm.MemberId == post.MemberId
                && sm.StoreId == post.StoreId);
            if (isManagerOfStore)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }
            var member = _context.Members.FirstOrDefault(m => m.Id == post.MemberId);
            var store = _context.Stores.FirstOrDefault(p => p.Id == post.StoreId);

            if (store == null || member == null)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }

            var appointer = _context.StoreManagers.FirstOrDefault(m => m.Id == post.AppointerId);
            var boss = _context.StoreManagers.FirstOrDefault(m => m.Id == post.BossId);
            if (appointer == null || boss == null)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }
            var storeManager = new StoreManager
            {
                Member = member,
                MemberId = post.MemberId,
                StoreId = post.StoreId,
                Store = store,
                ParentManager = boss,
                ParentManagerId = boss.Id,
                StorePermissions = new List<StorePermission>()
            };
            storeManager.StorePermissions.Add(new StorePermission
            {
                StoreManager = storeManager,
                StoreManagerId = storeManager.Id,
                PermissionType = PermissionType.Reply_permission
            });
            storeManager.StorePermissions.Add(new StorePermission
            {
                StoreManager = storeManager,
                StoreManagerId = storeManager.Id,
                PermissionType = PermissionType.Get_history_permission
            });

            _context.StoreManagers.Add(storeManager);
            await _context.SaveChangesAsync();

            return Response<StoreManager>.Success(HttpStatusCode.OK, storeManager);
        }

        public async Task<Response<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        {
            bool isManagerOfStore = _context.StoreManagers.Any(sm => sm.MemberId == post.MemberId
                && sm.StoreId == post.StoreId);
            if (isManagerOfStore)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }
            var member = _context.Members.FirstOrDefault(m => m.Id == post.MemberId);
            var store = _context.Stores.FirstOrDefault(p => p.Id == post.StoreId);

            if (store == null || member == null)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }

            var appointer = _context.StoreManagers.FirstOrDefault(m => m.Id == post.AppointerId);
            var boss = _context.StoreManagers.FirstOrDefault(m => m.Id == post.BossId);
            if (appointer == null || boss == null)
            {
                return Response<StoreManager>.Error(HttpStatusCode.NotFound, "");
            }
            var storeManager = new StoreManager
            {
                Member = member,
                MemberId = post.MemberId,
                StoreId = post.StoreId,
                Store = store,
                ParentManager = boss,
                ParentManagerId = boss.Id,
                StorePermissions = new List<StorePermission>()
            };
            storeManager.StorePermissions.Add(new StorePermission
            {
                StoreManager = storeManager,
                StoreManagerId = storeManager.Id,
                PermissionType = PermissionType.Manager_permission
            });

            _context.StoreManagers.Add(storeManager);
            await _context.SaveChangesAsync();

            return Response<StoreManager>.Success(HttpStatusCode.OK, storeManager);
        }


        public async Task<Response<bool>> PutStoreManager(long id, StoreManager storeManager)
        {
            if (id != storeManager.Id)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "");
            }

            _context.Entry(storeManager).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreManagerExists(id))
                {
                    return Response<bool>.Error(HttpStatusCode.NotFound, ""); ;
                }
                else
                {
                    throw;
                }
            }

            return Response<bool>.Success(HttpStatusCode.OK, true);
        }


        public async Task<Response<bool>> DeleteStoreManager(long id)
        {
            var storeManager = await _context.StoreManagers.FindAsync(id);
            if (storeManager == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "");
            }

            _context.StoreManagers.Remove(storeManager);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.OK, true);
        }

        private bool StoreManagerExists(long id)
        {
            return _context.StoreManagers.Any(e => e.Id == id);
        }

        public async Task<Response<bool>> AddPermissionToManager(long Id, [FromBody] PermissionType permission)
        {
            if (permission == PermissionType.Manager_permission)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "");
            }

            var manager = _context.StoreManagers.FirstOrDefault(sm => sm.Id == Id);

            if (manager == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "");
            }

            manager.StorePermissions.Add(new StorePermission
            {
                StoreManager = manager,
                StoreManagerId = manager.Id,
                PermissionType = permission
            });
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.OK, true);
        }

        public async Task<Response<bool>> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {

            var manager = await _context.StoreManagers.FindAsync(id);

            if (manager == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "");
            }

            var permissionToRemove = manager.StorePermissions.FirstOrDefault(p => p.PermissionType == permission);

            if (permissionToRemove == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "");
            }

            manager.StorePermissions.Remove(permissionToRemove);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.OK, true);
        }

    }
}
