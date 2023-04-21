using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;

namespace shukersal_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreManagersController : ControllerBase
    {
        private readonly ManagerContext _context;
        private readonly MemberContext _memberContext;
        private readonly StoreContext _storeContext;

        public StoreManagersController(ManagerContext context, MemberContext memberContext, StoreContext storeContext)
        {
            _context = context;
            _memberContext = memberContext;
            _storeContext = storeContext;
        }

        // GET: api/StoreManagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            return await _context.StoreManagers.ToListAsync();
        }

        // GET: api/StoreManagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreManager>> GetStoreManager(long id)
        {
            //addition for testing
            var storeManager = await _context.StoreManagers
                .Include(m => m.StorePermissions)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (storeManager == null)
            {
                return NotFound();
            }

            return storeManager;
        }

        // POST: api/StoreManagers
        [HttpPost("api/storemanager/createstoremanager")]
        public async Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'ManagerContext.StorePermissions'  is null.");
            }
            bool isManagerOfStore = _context.StoreManagers.Any(sm => sm.MemberId == post.MemberId
                && sm.StoreId == post.StoreId);
            if (isManagerOfStore)
            {
                return Problem("The member already manages this store");
            }
            var member = await _memberContext.Members.FindAsync(post.MemberId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);

            if (store == null || member == null)
            {
                return Problem("Illegal store id or member id");
            }

            var appointer = await _context.StoreManagers.FindAsync(post.AppointerId);
            var boss = await _context.StoreManagers.FindAsync(post.BossId);
            if (appointer == null || boss == null)
            {
                return Problem("Illegal appointer id or boss id");
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

            return CreatedAtAction(nameof(GetStoreManager), new { id = storeManager.Id }, storeManager);
        }

        [HttpPost("api/storemanager/createstoreowner")]
        public async Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        {
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'ManagerContext.StorePermissions'  is null.");
            }
            bool isManagerOfStore = _context.StoreManagers.Any(sm => sm.MemberId == post.MemberId
                && sm.StoreId == post.StoreId);
            if (isManagerOfStore)
            {
                return Problem("The member already manages this store");
            }
            var member = await _memberContext.Members.FindAsync(post.MemberId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);

            if (store == null || member == null)
            {
                return Problem("Illegal store id or member id");
            }

            var appointer = await _context.StoreManagers.FindAsync(post.AppointerId);
            var boss = await _context.StoreManagers.FindAsync(post.BossId);
            if (appointer == null || boss == null)
            {
                return Problem("Illegal appointer id or boss id");
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

            return CreatedAtAction(nameof(GetStoreManager), new { id = storeManager.Id }, storeManager);
        }

        // PUT: api/StoreManagers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoreManager(long id, StoreManager storeManager)
        {
            if (id != storeManager.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/StoreManagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreManager(long id)
        {
            var storeManager = await _context.StoreManagers.FindAsync(id);
            if (storeManager == null)
            {
                return NotFound();
            }

            _context.StoreManagers.Remove(storeManager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoreManagerExists(long id)
        {
            return _context.StoreManagers.Any(e => e.Id == id);
        }

        // Add a permission to a shop manager
        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AddPermissionToManager(long id, [FromBody] PermissionType permission)
        {
            if (permission == PermissionType.Manager_permission)
            {
                return Problem("TODO");
            }

            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }

            var manager = await _context.StoreManagers.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            manager.StorePermissions.Add(new StorePermission
            {
                StoreManager = manager,
                StoreManagerId = manager.Id,
                PermissionType = permission
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Remove a permission from a shop manager
        [HttpDelete("{id}/permissions")]
        public async Task<IActionResult> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            var manager = await _context.StoreManagers.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            var permissionToRemove = manager.StorePermissions.FirstOrDefault(p => p.PermissionType == permission);

            if (permissionToRemove == null)
            {
                return NotFound();
            }

            manager.StorePermissions.Remove(permissionToRemove);
            await _context.SaveChangesAsync();

            return Ok();
        }


        //[HttpPost]
        //public async Task<IActionResult> GivePermission(PermissionsPost post)
        //{
        //    var appointer = await _memberContext.Members.FindAsync(post.AppointerId);
        //    var target = await _memberContext.Members.FindAsync(post.TargetId);
        //    var store = await _storeContext.Stores.FindAsync(post.StoreId);
        //    nextPermissionId++;
        //    if (_context.StoreManagers == null)
        //    {
        //        return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
        //    }
        //    if (_context.StorePermissions == null)
        //    {
        //        return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var appointerManager = SearchManager(store, appointer);
        //        var targetManager = SearchManager(store, target);

        //        //member no found error
        //        if (appointerManager == null)
        //            return Problem("TODO");
        //        if (targetManager == null)
        //            return Problem("TODO");

        //        //permission test
        //        if (!CheckPermission(appointerManager, _context.EDIT_MANAGER_PERMISSIONS_PERMISSION))
        //            return Problem("TODO");

        //        if (!CheckPermission(targetManager, post.PermissionType))
        //            return Problem("TODO");

        //        var permission = new StorePermission(nextPermissionId, targetManager, post.PermissionType);

        //        _context.StorePermissions.Add(permission);

        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> RemovePermission(PermissionsPost post)
        //{
        //    var appointer = await _memberContext.Members.FindAsync(post.AppointerId);
        //    var target = await _memberContext.Members.FindAsync(post.TargetId);
        //    var store = await _storeContext.Stores.FindAsync(post.StoreId);
        //    if (post.PermissionType == _context.MANAGER_PERMISSION)
        //        return Problem("TODO");
        //    if (_context.StoreManagers == null)
        //    {
        //        return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
        //    }
        //    if (_context.StorePermissions == null)
        //    {
        //        return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var appointerManager = SearchManager(store, appointer);
        //        var targetManager = SearchManager(store, target);

        //        //member no found error
        //        if (appointerManager == null)
        //            return Problem("TODO");
        //        if (targetManager == null)
        //            return Problem("TODO");

        //        //permission test
        //        if (!CheckPermission(appointerManager, _context.EDIT_MANAGER_PERMISSIONS_PERMISSION))
        //            return Problem("TODO");

        //        if (CheckPermission(targetManager, post.PermissionType))
        //            return Problem("TODO");

        //        foreach (StorePermission permission in targetManager.StorePermissions)
        //            if (permission.PermissionType == post.PermissionType)
        //                _context.StorePermissions.Remove(permission);

        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        //[HttpGet("{storeId, memberId}")]
        //public async Task<ActionResult<IEnumerable<StoreManager>>> GetManagersPermissions(long StoreId, long MemberId)
        //{
        //    var member = await _memberContext.Members.FindAsync(MemberId);
        //    var store = await _storeContext.Stores.FindAsync(StoreId);
        //    if (_context.StoreManagers == null)
        //    {
        //        return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
        //    }
        //    if (_context.StorePermissions == null)
        //    {
        //        return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        if (!CheckPermission(SearchManager(store, member), _context.GET_MANAGER_INFO_PERMISSION))
        //            return Problem("TODO");
        //        var storeManagers = await _context.StoreManagers.Where(m => m.Store == store).Include(m => m.StorePermissions).ToListAsync();

        //        return storeManagers;
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

        //private StoreManager? SearchManager(Store store, Member member)
        //{
        //    foreach (StoreManager manager in _context.StoreManagers)
        //        if (manager.Member == member && manager.Store == store)
        //            return manager;
        //    return null;
        //}

        //private bool CheckPermission(StoreManager? manager, int permissionType)
        //{
        //    if (manager == null)
        //        return false;
        //    foreach (StorePermission permission in manager.StorePermissions)
        //        if (permission.PermissionType == _context.MANAGER_PERMISSION || permission.PermissionType == permissionType)
        //            return true;
        //    return false;
        //}
    }

}