using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;

namespace shukersal_backend.ServiceLayer
{
    // TODO: Move logic to domain
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class StoreManagerService : ControllerBase
    {
        //private readonly ManagerContext _context;
        //private readonly MarketDbContext _context;
        private readonly StoreManagerController _controller;
        private readonly Member? currentMember;

        public StoreManagerService(MarketDbContext context)
        {
            //_context = context;
            //currentMember = ServiceUtilities.GetCurrentMember(context, httpContext);
            _controller = new StoreManagerController(context);
        }

        // GET: api/StoreManagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            var response = await _controller.GetStoreManagers();
            return Ok(response.Result);
        }

        // GET: api/StoreManagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreManager>> GetStoreManager(long id)
        {
            var response = await _controller.GetStoreManager(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // POST: api/StoreManagers
        [HttpPost("api/storemanager/createstoremanager")]
        public async Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {
            var response = await _controller.PostStoreManager(post);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpPost("api/storemanager/createstoreowner")]
        public async Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        {
            var response = await _controller.PostStoreOwner(post);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // PUT: api/StoreManagers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoreManager(long id, StoreManager storeManager)
        {
            var response = await _controller.PutStoreManager(id, storeManager);
            if (!response.IsSuccess)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                return NotFound();
            }
            return Ok(response.Result);
        }

        // DELETE: api/StoreManagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreManager(long id)
        {
            var response = await _controller.DeleteStoreManager(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }


        // Add a permission to a shop manager
        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AddPermissionToManager(long id, [FromBody] PermissionType permission)
        {
            var response = await _controller.AddPermissionToManager(id, permission);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // Remove a permission from a shop manager
        [HttpDelete("{id}/permissions")]
        public async Task<IActionResult> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {
            var response = await _controller.RemovePermissionFromManager(id, permission);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }
    }
}