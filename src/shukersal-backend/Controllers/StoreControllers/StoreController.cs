using Microsoft.AspNetCore.Mvc;
using shukersal_backend.Domain;
using shukersal_backend.Models;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shukersal_backend.Controllers.StoreControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly StoreService storeService;

        public StoreController(StoreContext context, ManagerContext managerContext, MemberContext memberContext)
        {
            storeService = new StoreService(context, managerContext, memberContext);
        }

        // GET: api/Store
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            var response = await storeService.GetStores();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Store/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(long id)
        {
            var response = await storeService.GetStore(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // POST: api/Store
        [HttpPost]
        public async Task<ActionResult<Store>> CreateStore(StorePost storeData)
        {
            if (ModelState.IsValid)
            {
                var response = await storeService.CreateStore(storeData);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(ModelState);
                }
                var store = response.Result;
                return CreatedAtAction("GetStore", new { id = store.Id }, store);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Store/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(long id, StorePost post)
        {
            if (ModelState.IsValid)
            {
                var response = await storeService.UpdateStore(id, post);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return NoContent();

            }
            return BadRequest();
        }

        // DELETE: api/Store/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            var response = await storeService.DeleteStore(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Action method for adding a product to a store
        [HttpPost("stores/{storeId}/products")]
        public async Task<IActionResult> AddProduct(long storeId, Product product)
        {
            if (ModelState.IsValid)
            {
                var response = await storeService.AddProduct(storeId, product);
                var res_product = response.Result;
                return Ok(res_product);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Action method for updating a product in a store
        [HttpPut("stores/{storeId}/products")]
        public async Task<IActionResult> UpdateProduct(long storeId, long productId, Product product)
        {
            if (ModelState.IsValid)
            {
                var response = await storeService.UpdateProduct(storeId, productId, product);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                return Ok(response.Result);
            }
            return BadRequest();
        }

        // Action method for deleting a product from a store
        [HttpDelete("stores/{storeId}/products")]
        public async Task<IActionResult> DeleteProduct(long storeId, long productId)
        {
            var response = await storeService.DeleteProduct(storeId, productId);
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
