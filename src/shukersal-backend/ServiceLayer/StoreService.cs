using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shukersal_backend.ServiceLayer
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    public class StoreService : ControllerBase
    {
        private readonly StoreController storeController;
        private readonly Member? currentMember;
        private readonly ILogger logger;

        public StoreService(MarketDbContext context, ILogger<StoreService> logger)
        {
            storeController = new StoreController(context);

            currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            this.logger = logger;
            logger.LogInformation("testing the log");
        }

        // GET: api/Store
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            var response = await storeController.GetStores();
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
            var response = await storeController.GetStore(id);
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
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateStore(storeData, currentMember);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStore(long id, StorePatch patch)
        {
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.UpdateStore(id, patch, currentMember);
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
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await storeController.DeleteStore(id, currentMember);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Action method for adding a product to a store
        [HttpPost("stores/{storeId}/products")]
        public async Task<IActionResult> AddProduct(long storeId, ProductPost product)
        {
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.AddProduct(storeId, product, currentMember);
                var res_product = response.Result;
                return Ok(res_product);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Action method for updating a product in a store
        [HttpPatch("stores/{storeId}/products/{productId}")]
        public async Task<IActionResult> UpdateProduct(long storeId, long productId, ProductPatch product)
        {
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.UpdateProduct(storeId, productId, product, currentMember);
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
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await storeController.DeleteProduct(storeId, productId, currentMember);
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            return NoContent();
        }

        //// GET: api/Products
        //[HttpGet("stores/Products")]
        //public async Task<ActionResult<IEnumerable<Store>>> GetAllProducts()
        //{
        //    var response = await storeController.GetAllProducts();
        //    if (!response.IsSuccess)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(response.Result);
        //}

        // GET: api/Store/Products
        [HttpGet("stores/{storeId}/products")]
        public async Task<ActionResult<Store>> GetStoreProducts(long storeId)
        {
            var response = await storeController.GetStoreProducts(storeId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Category
        [HttpGet("stores/categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var response = await storeController.GetCategories();
            return Ok(response.Result);
        }
    }
}
