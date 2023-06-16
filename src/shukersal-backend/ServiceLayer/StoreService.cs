using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.StoreModels;
using shukersal_backend.Utility;
using System.Data;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shukersal_backend.ServiceLayer
{
    [ApiController]
    [Route("api/")]
    [EnableCors("AllowOrigin")]
    public class StoreService : ControllerBase
    {
        private readonly StoreController storeController;
        private readonly ILogger<ControllerBase> logger;
        private readonly MarketDbContext context;

        public StoreService(MarketDbContext context, ILogger<StoreService> logger)
        {
            storeController = new StoreController(context);
            this.context = context;
            this.logger = logger;
            logger.LogInformation("testing the log");
        }

        // GET: api/stores
        [HttpGet("stores")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            logger.LogInformation("GetStores method called.");
            var response = await storeController.GetStores();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/stores/5
        [HttpGet("stores/{id}")]
        public async Task<ActionResult<Store>> GetStore(long id)
        {
            logger.LogInformation("GetStore method called with ID: {id}", id);
            var response = await storeController.GetStore(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // POST: api/stores
        [HttpPost("stores")]
        public async Task<ActionResult<Store>> CreateStore(StorePost storeData)
        {
            logger.LogInformation("CreateStore method called.");
            var member = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (member == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController
                    .CreateStore(storeData, member);
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

        // PUT: api/store/5
        [HttpPatch("stores/{id}")]
        public async Task<IActionResult> UpdateStore(long id, StorePatch patch)
        {
            logger.LogInformation("UpdateStore method called with ID: {id}", id);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
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

        // DELETE: api/store/5
        [HttpDelete("stores/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
        public async Task<IActionResult> DeleteStore(long id)
        {
            logger.LogInformation("DeleteStore method called with ID: {id}", id);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
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
            logger.LogInformation("AddProduct method called with for storeID: {storeId}", storeId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
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

        [HttpGet("stores/{storeId}/products")]
        public async Task<ActionResult<Store>> GetStoreProducts(long storeId)
        {
            logger.LogInformation("GetStoreProducts method called with for storeID: {storeId}", storeId);
            var response = await storeController.GetStoreProducts(storeId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            logger.LogInformation("GetProduct method called with for productId: {id}", id);
            var response = await storeController.GetProduct(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/products
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllProducts()
        {
            var response = await storeController.GetAllProducts();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        [HttpPatch("stores/{storeId}/products/{productId}")]
        public async Task<IActionResult> UpdateProduct(long storeId, long productId, ProductPatch product)
        {
            logger.LogInformation("AddProduct method called with for storeID: {storeId} and productId: {productId}", storeId, productId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
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

        [HttpDelete("stores/{storeId}/products/{productId}")]
        public async Task<IActionResult> DeleteProduct(long storeId, long productId)
        {
            logger.LogInformation("DeleteProduct method called with for storeID: {storeId} and productId: {productId}", storeId, productId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
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

        // GET: api/Category
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            logger.LogInformation("GetCategories method called");
            var response = await storeController.GetCategories();
            return Ok(response.Result);
        }

        /*
         * Discounts 
         */

        [HttpPost("discounts")]
        public async Task<IActionResult> AddDiscount(DiscountRulePost post)
        {
            logger.LogInformation("AddDiscount method called with for storeID: {storeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateDiscount(post, currentMember);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("discounts/{id}")]
        public async Task<IActionResult> AddChildDiscount(long id, DiscountRulePost post)
        {
            logger.LogInformation("AddChildDiscount method called with for storeID: {storeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateChildDiscount(id, post, currentMember);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("discounts/boolean/new/{id}")]
        public async Task<IActionResult> AddDiscountBoolean(long id, DiscountRuleBooleanPost post)
        {
            logger.LogInformation("DiscountRuleBooleanPost method called with for storeID: {storeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateDiscountRuleBoolean(post, currentMember, id);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("discounts/boolean/child/{id}")]
        public async Task<IActionResult> AddChildDiscountBoolean(long id, DiscountRuleBooleanPost post)
        {
            logger.LogInformation("AddChildDiscountBoolean method called with for storeID: {storeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateChildDiscountRuleBoolean(id, post, currentMember);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("discounts/all/{storeId}")]
        public async Task<IActionResult> GetDiscounts(long storeId)
        {
            logger.LogInformation("GetDiscounts method called with for storeID: {storeId}", storeId);

            var response = await storeController.GetDiscounts(storeId);
            var res_disc = response.Result;
            return Ok(res_disc);

        }

        [HttpGet("store/{storeId}/discounts/applied")]
        public async Task<IActionResult> GetAppliedDiscount(long storeId)
        {
            logger.LogInformation("GetAppliedDiscount method called with for storeID: {storeId}", storeId);

            var response = await storeController.GetAppliedDiscount(storeId);
            var res_disc = response.Result;
            return Ok(res_disc);
        }

        [HttpPatch("stores/{storeId}/discounts/{discountId}")]
        public async Task<IActionResult> SelectDiscount(long storeId, long discountId)
        {
            logger.LogInformation("SelectDiscount method called with for storeID: {storeId}, discountId : {discountId}", storeId, discountId);

            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await storeController.SelectDiscount(storeId, discountId, currentMember);
            var res_disc = response.Result;
            return Ok(res_disc);
        }

        [HttpGet("stores/{storeId}/purchaserules/all")]
        public async Task<IActionResult> GetPurchaseRules(long storeId)
        {
            logger.LogInformation("GetPurchaseRules method called with for storeID: {storeId}", storeId);

            var response = await storeController.GetPurchaseRules(storeId);
            var res_disc = response.Result;
            return Ok(res_disc);

        }

        [HttpGet("stores/{storeId}/purchaserules/applied")]
        public async Task<IActionResult> GetAppliedPurchaseRule(long storeId)
        {
            logger.LogInformation("GetAppliedPurchaseRule method called with for storeID: {storeId}", storeId);

            var response = await storeController.GetAppliedPurchaseRule(storeId);
            var res_disc = response.Result;
            return Ok(res_disc);
        }

        [HttpPatch("stores/{storeId}/purchaserules/{discountId}")]
        public async Task<IActionResult> SelectPurchaseRule(long storeId, long discountId)
        {
            logger.LogInformation("SelectPurchaseRule method called with for storeID: {storeId}, discountId : {discountId}", storeId, discountId);

            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var response = await storeController.SelectPurchaseRule(storeId, discountId, currentMember);
            var res_disc = response.Result;
            return Ok(res_disc);
        }

        [HttpPost("purchaserules")]
        public async Task<IActionResult> AddPurchaseRule(PurchaseRulePost post)
        {
            logger.LogInformation("AddPurchaseRule method called with for storeID: {storeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreatePurchaseRule(post, currentMember);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("purchaserules/{id}")]
        public async Task<IActionResult> AddChildPurchaseRule(long id, PurchaseRulePost post)
        {
            logger.LogInformation("AddChildPurchaseRule method called with for compositeId: {compositeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateChildPurchaseRule(id, post, currentMember);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
