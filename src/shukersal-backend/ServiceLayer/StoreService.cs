﻿using Microsoft.AspNetCore.Cors;
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
        private readonly ILogger<ControllerBase> logger;
        private readonly MarketDbContext context;

        public StoreService(MarketDbContext context, ILogger<ControllerBase> logger)
        {
            storeController = new StoreController(context);
            this.context = context;
            currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            this.logger = logger;
            logger.LogInformation("testing the log");
        }

        // GET: api/Store
        [HttpGet]
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

        // GET: api/Store/5
        [HttpGet("{id}")]
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

        // POST: api/Store
        [HttpPost]
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

        // PUT: api/Store/5
        [HttpPatch("{id}")]
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

        // DELETE: api/Store/5
        [HttpDelete("{id}")]
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

        // Action method for updating a product in a store
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

        // Action method for deleting a product from a store
        [HttpDelete("stores/{storeId}/products")]
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
            logger.LogInformation("GetStoreProducts method called with for storeID: {storeId}", storeId);
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
            logger.LogInformation("GetCategories method called");
            var response = await storeController.GetCategories();
            return Ok(response.Result);
        }

        // GET: api/Store/5
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
        public async Task<IActionResult> AddChildDiscount(long compositeId, DiscountRulePost post)
        {
            logger.LogInformation("AddDiscount method called with for storeID: {storeId}", post.StoreId);
            var currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var response = await storeController.CreateChildDiscount(compositeId, post, currentMember);
                var res_disc = response.Result;
                return Ok(res_disc);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("discounts/{storeId}")]
        public async Task<IActionResult> GetDiscounts(long storeId)
        {
            logger.LogInformation("GetDiscounts method called with for storeID: {storeId}", storeId);

            var response = await storeController.GetDiscounts(storeId);
            var res_disc = response.Result;
            return Ok(res_disc);

        }
    }
}
