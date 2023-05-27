﻿using Microsoft.AspNetCore.Mvc;
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

        public virtual async Task<bool> CheckPermission(long managerId, PermissionType type)
        {
            var manager = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .FirstOrDefaultAsync(m => m.Id == managerId);

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

        public async Task<Response<IEnumerable<StoreManager>>> GetStoreManagersByMemberId(long memberId)
        {
            var storeManagers = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .Where(s => s.MemberId == memberId)
                .ToListAsync();

            if (storeManagers.Count == 0)
            {
                return Response<IEnumerable<StoreManager>>.Error(HttpStatusCode.NotFound, "");
            }

            return Response<IEnumerable<StoreManager>>.Success(HttpStatusCode.OK, storeManagers);
        }

        public async Task<Response<StoreManagerTreeNode>> GetStoreManagersByStoreId(long storeId)
        {
            var storeManagers = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                //(m => m.ParentManager)
                .Where(s => s.StoreId == storeId)
                .ToListAsync();

            if (storeManagers.Count == 0)
            {
                return Response<StoreManagerTreeNode>.Error(HttpStatusCode.NotFound, "");
            }

            var tree = BuildStoreManagerTree(storeManagers);



            //return Response<IEnumerable<StoreManager>>.Success(HttpStatusCode.OK, storeManagers);
            return Response<StoreManagerTreeNode>.Success(HttpStatusCode.OK, tree);
        }

        private StoreManagerTreeNode BuildStoreManagerTree(List<StoreManager> storeManagers)
        {
            var managerDictionary = new Dictionary<long, StoreManagerTreeNode>();

            foreach (var manager in storeManagers)
            {
                var node = new StoreManagerTreeNode
                {
                    StoreManager = manager,
                    Subordinates = new List<StoreManagerTreeNode>()
                    //Id = manager.Id,
                    //StoreId = manager.StoreId,
                    //MemberId = manager.MemberId,
                    //ParentManagerId = manager.ParentManagerId
                };

                managerDictionary[manager.Id] = node;
            }

            foreach (var manager in storeManagers)
            {
                if (managerDictionary.TryGetValue(manager.Id, out var node) && node.StoreManager.ParentManagerId != 0 &&
                    managerDictionary.TryGetValue(node.StoreManager.ParentManagerId, out var parentManager))
                {
                    parentManager.Subordinates = parentManager.Subordinates ?? new List<StoreManagerTreeNode>();
                    parentManager.Subordinates.Add(node);
                }
            }


            return managerDictionary.Values.FirstOrDefault(manager => manager.StoreManager.ParentManagerId == 0);
        }

        public async Task<Response<IEnumerable<Store>>> GetManagedStoresByMemberId(long memberId)
        {
            var storeManagers = await _context.StoreManagers
                .Where(s => s.MemberId == memberId)
                .Include(m => m.Store)
                .ToListAsync();

            if (storeManagers.Count == 0)
            {
                return Response<IEnumerable<Store>>.Error(HttpStatusCode.NotFound, "");
            }

            var managedStores = storeManagers.Select(sm => sm.Store);

            return Response<IEnumerable<Store>>.Success(HttpStatusCode.OK, managedStores);
        }



        public async Task<Response<StoreManager>> PostStoreManager(OwnerManagerPost post, long appointerId)
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

            var appointer = _context.StoreManagers.FirstOrDefault(m => m.Id == appointerId);
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

        public async Task<Response<StoreManager>> PostStoreOwner(OwnerManagerPost post, long appointerId)
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

            var appointer = _context.StoreManagers.FirstOrDefault(m => m.Id == appointerId);
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


        public async Task<Response<bool>> DeleteStoreManager(long id, Member boss)
        {
            var storeManager = await _context.StoreManagers.FindAsync(id);
            var bossManager = _context.StoreManagers.FirstOrDefault(m => m.MemberId == boss.Id);
            if (storeManager == null || bossManager == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "");
            }
            if (storeManager.ParentManagerId != bossManager.Id)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "the manager is not the boss");
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
