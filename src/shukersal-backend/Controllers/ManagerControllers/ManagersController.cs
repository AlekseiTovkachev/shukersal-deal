using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.Controllers.ManagersController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        

        private readonly ManagerContext _context;
        private readonly MemberContext _memberContext;
        private readonly StoreContext _storeContext;

        private long nextPermissionId;
        private long nextManagerId;

        public ManagersController(ManagerContext context, MemberContext memberContext, StoreContext storeContext)
        {

            _context = context;
            _memberContext = memberContext;
            _storeContext = storeContext;
            nextPermissionId = 0; //TODO: READ FROM DB THE MAX VALUE AT VER3
            nextManagerId = 0; //TODO: READ FROM DB THE MAX VALUE AT VER3
        }

        [HttpPost]
        public async Task<IActionResult> AddOwner(OwnerManagerPost post)
        {
            var appointer = await _memberContext.Members.FindAsync(post.AppointerId);
            var boss = await _memberContext.Members.FindAsync(post.BossId);
            var member = await _memberContext.Members.FindAsync(post.MemberId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);
            nextPermissionId++;
            nextManagerId++;
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            if (ModelState.IsValid)
            {
                var parent = SearchManager(store, boss);

                //parent no found error
                if (parent == null)
                    return Problem("TODO");

                //not already manager, needed to avoid loops
                if (SearchManager(store, member) != null)
                    return Problem("TODO");

                //permission test
                if (!CheckPermission(SearchManager(store, appointer), _context.APPOINT_OWNER_PERMISSION))
                    return Problem("TODO");


                var owner = new StoreManager(nextManagerId, member, store, parent);
                var permission = new StorePermission(nextPermissionId, owner, _context.MANAGER_PERMISSION);

                // Add the shopping cart and member to the database
                _context.StoreManagers.Add(owner);
                _context.StorePermissions.Add(permission);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddManager(OwnerManagerPost post)
        {
            var appointer = await _memberContext.Members.FindAsync(post.AppointerId);
            var boss = await _memberContext.Members.FindAsync(post.BossId);
            var member = await _memberContext.Members.FindAsync(post.MemberId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);
            nextPermissionId++;
            nextManagerId++;
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            if (ModelState.IsValid)
            {
                var parent = SearchManager(store, boss);

                //parent no found error
                if (parent == null)
                    return Problem("TODO");

                //not already manager, needed to avoid loops
                if (SearchManager(store, member) != null)
                    return Problem("TODO");

                //permission test
                if (!CheckPermission(SearchManager(store, appointer), _context.APPOINT_MANAGER_PERMISSION))
                    return Problem("TODO");

                var manager = new StoreManager(nextManagerId, member, store, parent);
                var permission1 = new StorePermission(nextPermissionId, manager, _context.REPLY_PERMISSION);
                nextPermissionId++;
                var permission2 = new StorePermission(nextPermissionId, manager, _context.GET_HISTORY_PERMISSION);

                // Add the shopping cart and member to the database
                _context.StoreManagers.Add(manager);
                _context.StorePermissions.Add(permission1);
                _context.StorePermissions.Add(permission2);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFounder(FounderPost post)
        {
            var member = await _memberContext.Members.FindAsync(post.MemberId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);
            nextPermissionId++;
            nextManagerId++;
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            if (ModelState.IsValid)
            {
                var founder = new StoreManager(nextManagerId, member, store);
                var permission = new StorePermission(nextPermissionId, founder, _context.MANAGER_PERMISSION);

                // Add the shopping cart and member to the database
                _context.StoreManagers.Add(founder);
                _context.StorePermissions.Add(permission);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GivePermission(PermissionsPost post)
        {
            var appointer = await _memberContext.Members.FindAsync(post.AppointerId);
            var target = await _memberContext.Members.FindAsync(post.TargetId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);
            nextPermissionId++;
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            if (ModelState.IsValid)
            {
                var appointerManager = SearchManager(store, appointer);
                var targetManager = SearchManager(store, target);

                //member no found error
                if (appointerManager == null)
                    return Problem("TODO");
                if (targetManager == null)
                    return Problem("TODO");

                //permission test
                if (!CheckPermission(appointerManager, _context.EDIT_MANAGER_PERMISSIONS_PERMISSION))
                    return Problem("TODO");

                if (!CheckPermission(targetManager, post.PermissionType))
                    return Problem("TODO");

                var permission = new StorePermission(nextPermissionId, targetManager, post.PermissionType);

                _context.StorePermissions.Add(permission);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemovePermission(PermissionsPost post)
        {
            var appointer = await _memberContext.Members.FindAsync(post.AppointerId);
            var target = await _memberContext.Members.FindAsync(post.TargetId);
            var store = await _storeContext.Stores.FindAsync(post.StoreId);
            if (post.PermissionType == _context.MANAGER_PERMISSION)
                return Problem("TODO");
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            if (ModelState.IsValid)
            {
                var appointerManager = SearchManager(store, appointer);
                var targetManager = SearchManager(store, target);

                //member no found error
                if (appointerManager == null)
                    return Problem("TODO");
                if (targetManager == null)
                    return Problem("TODO");

                //permission test
                if (!CheckPermission(appointerManager, _context.EDIT_MANAGER_PERMISSIONS_PERMISSION))
                    return Problem("TODO");

                if (CheckPermission(targetManager, post.PermissionType))
                    return Problem("TODO");

                foreach (StorePermission permission in targetManager.StorePermissions)
                    if (permission.PermissionType == post.PermissionType)
                        _context.StorePermissions.Remove(permission);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{storeId, memberId}")]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetManagersPermissions(long StoreId, long MemberId)
        {
            var member = await _memberContext.Members.FindAsync(MemberId);
            var store = await _storeContext.Stores.FindAsync(StoreId);
            if (_context.StoreManagers == null)
            {
                return Problem("Entity set 'ManagerContext.StoreManagers'  is null.");
            }
            if (_context.StorePermissions == null)
            {
                return Problem("Entity set 'MAnagerContext.StorePermissions'  is null.");
            }
            if (ModelState.IsValid)
            {
                if (!CheckPermission(SearchManager(store, member), _context.GET_MANAGER_INFO_PERMISSION))
                    return Problem("TODO");
                var storeManagers = await _context.StoreManagers.Where(m => m.Store == store).Include(m => m.StorePermissions).ToListAsync();

                return storeManagers;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private StoreManager? SearchManager(Store store, Member member)
        {
            foreach (StoreManager manager in _context.StoreManagers)
                if (manager.Member == member && manager.Store == store)
                    return manager;
            return null;
        }

        private bool CheckPermission(StoreManager? manager, int permissionType)
        {
            if (manager == null)
                return false;
            foreach (StorePermission permission in manager.StorePermissions)
                if (permission.PermissionType == _context.MANAGER_PERMISSION || permission.PermissionType == permissionType)
                    return true;
            return false;
        }

    }
}
