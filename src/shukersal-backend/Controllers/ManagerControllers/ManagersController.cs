using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;

namespace shukersal_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreManagersController : ControllerBase
    {
        //private readonly ManagerContext _context;
        private readonly StoreContext _context;
        private readonly MemberContext _memberContext;
        private readonly StoreContext _storeContext;


        public StoreManagersController(ManagerContext context, MemberContext memberContext, StoreContext storeContext)
        {
            //_context = context;
            _context = storeContext;
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
                return NotFound();
            }
            if (_context.StorePermissions == null)
            {
                return NotFound();
            }
            bool isManagerOfStore = _context.StoreManagers.Any(sm => sm.MemberId == post.MemberId
                && sm.StoreId == post.StoreId);
            if (isManagerOfStore)
            {
                return NotFound();
            }
            var member = _memberContext.Members.FirstOrDefault(m => m.Id == post.MemberId);
            var store = _storeContext.Stores.FirstOrDefault(p => p.Id == post.StoreId);

            if (store == null || member == null)
            {
                return NotFound();
            }

            var appointer = _context.StoreManagers.FirstOrDefault(m => m.Id == post.AppointerId);
            var boss = _context.StoreManagers.FirstOrDefault(m => m.Id == post.BossId);
            if (appointer == null || boss == null)
            {
                return NotFound();
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
                return NotFound();
            }
            if (_context.StorePermissions == null)
            {
                return NotFound();
            }
            bool isManagerOfStore = _context.StoreManagers.Any(sm => sm.MemberId == post.MemberId
                && sm.StoreId == post.StoreId);
            if (isManagerOfStore)
            {
                return NotFound();
            }
            var member = _memberContext.Members.FirstOrDefault(m => m.Id == post.MemberId);
            var store = _storeContext.Stores.FirstOrDefault(p => p.Id == post.StoreId);

            if (store == null || member == null)
            {
                return NotFound();
            }

            var appointer = _context.StoreManagers.FirstOrDefault(m => m.Id == post.AppointerId);
            var boss = _context.StoreManagers.FirstOrDefault(m => m.Id == post.BossId);
            if (appointer == null || boss == null)
            {
                return NotFound();
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
        public async Task<IActionResult> AddPermissionToManager(long Id, [FromBody] PermissionType permission)
        {
            if (permission == PermissionType.Manager_permission)
            {
                return NotFound();
            }

            if (_context.StoreManagers == null)
            {
                return NotFound();
            }
            if (_context.StorePermissions == null)
            {
                return NotFound();
            }

            var manager = _context.StoreManagers.FirstOrDefault(sm => sm.Id == Id);

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
    }
}