using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.ServiceLayer
{
    // TODO: Move logic to domain
    [Route("api/storemanagers")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class StoreManagerService : ControllerBase
    {
        //private readonly ManagerContext _context;
        private readonly MarketDbContext _context;
        private readonly StoreManagerController _controller;
        private readonly Member? currentMember;
        private readonly ILogger<ControllerBase> logger;

        public StoreManagerService(MarketDbContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            this.logger = logger;
            //currentMember = ServiceUtilities.GetCurrentMember(context, httpContext);
            _controller = new StoreManagerController(context);
        }

        // GET: api/StoreManagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            logger.LogInformation("GetStoreManagers method called");
            var response = await _controller.GetStoreManagers();
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreManager>> GetStoreManager(long id)
        {
            logger.LogInformation("GetStoreManager with id = {id} method called", id);
            var response = await _controller.GetStoreManager(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagersByMemberId(long memberId)
        {
            logger.LogInformation("GetStoreManagersByMemberId with id = {memberId} method called", memberId);
            var response = await _controller.GetStoreManagersByMemberId(memberId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpGet("stores/{storeId}/managers")]
        public async Task<ActionResult<StoreManagerTreeNode>> GetStoreManagersByStoreId(long storeId)
        {
            logger.LogInformation("GetStoreManagersByStoreId with id = {storeId} method called", storeId);
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await _controller.GetStoreManagersByStoreId(storeId, currentMember);
            if (!response.IsSuccess)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpGet("member/{memberId}/stores")]
        public async Task<ActionResult<IEnumerable<Store>>> GetManagedStoresByMemberId(long memberId)
        {
            logger.LogInformation("GetStoresByMemberId with id = {memberId} method called", memberId);
            var response = await _controller.GetManagedStoresByMemberId(memberId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpPost]
        public async Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {
            logger.LogInformation("PostStoreManager method called");
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (post.Owner)
            {
                var response = await _controller.PostStoreOwner(post, currentMember);
                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                return Ok(response.Result);
            }
            else
            {
                var response = await _controller.PostStoreManager(post, currentMember);
                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                return Ok(response.Result);
            }
        }

        //[HttpPost("api/storemanager/createstoreowner")]
        //public async Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        //{
        //    logger.LogInformation("PostStoreOwner method called");
        //    var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
        //    if (currentMember == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var response = await _controller.PostStoreOwner(post, currentMember);
        //    if (!response.IsSuccess)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(response.Result);
        //}

        // PUT: api/storeManagers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoreManager(long id, StoreManager storeManager)
        {
            logger.LogInformation("PutStoreManager method called with id = {id}", id);
            var response = await _controller.PutStoreManager(id, storeManager);
            if (!response.IsSuccess)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
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
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await _controller.DeleteStoreManager(id, currentMember);
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
            logger.LogInformation("AddPermission method called for managerid = {id}", id);
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await _controller.AddPermissionToManager(id, permission, currentMember);
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
            logger.LogInformation("RemovePermission method called for managerid = {id}", id);
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await _controller.RemovePermissionFromManager(id, permission);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }
    }
}