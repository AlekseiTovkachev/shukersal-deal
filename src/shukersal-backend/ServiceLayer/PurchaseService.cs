using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;

namespace shukersal_backend.ServiceLayer
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PurchaseService : ControllerBase
    {
        private readonly PurchaseController purchaseController;


        public PurchaseService(MarketDbContext context)
        {
            purchaseController = new PurchaseController(context);
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Purchase>>> GetPurchases()
        {
            var response = await purchaseController.GetPurchases();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Purchases/5
        [HttpGet("Purchaseid")]
        public async Task<ActionResult<Models.Purchase>> GetPurchase(long PurchaseId)
        {
            var response = await purchaseController.GetPurchase(PurchaseId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }



        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Purchase>> PurchaseAShoppingCart(PurchasePost purchasePost)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseController.PurchaseAShoppingCart(purchasePost);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
                }
                var purchase = response.Result;
                return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, purchase);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Purchases/5
        [HttpDelete("purchaseid/{purchaseId}")]
        public async Task<IActionResult> DeletePurchase(long purchaseId)
        {
            var response = await purchaseController.DeletePurchase(purchaseId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }


        // PUT: api/Purchase/5
        [HttpPut("purchaseid/{purchaseid}")]
        public async Task<IActionResult> UpdatePurchase(long purchaseid, PurchasePost post)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseController.UpdatePurchase(purchaseid, post);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

        // GET: api/Purchases/memberId/5
        [HttpGet("memberId/{memberId}")]
        public async Task<ActionResult<Models.Purchase>> BrowesePurchaseHistory(long memberId)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseController.BrowesePurchaseHistory(memberId);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

        // GET: api/Purchases/storeId/5
        [HttpGet("storeId/{storeId}")]
        public async Task<ActionResult<Models.Purchase>> BroweseShopPurchaseHistory(long storeId)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseController.BroweseShopPurchaseHistory(storeId);
                if (!response.IsSuccess)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(ModelState);
                    }
                }
                return Ok(response.Result);

            }
            return BadRequest();
        }

    }
}
