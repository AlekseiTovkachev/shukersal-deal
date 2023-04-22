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
using shukersal_backend.Domain;
using shukersal_backend.Models;

namespace shukersal_backend.Controllers.PurchaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseService purchaseService;


        public PurchasesController(MarketDbContext context)
        {
            purchaseService = new PurchaseService(context);
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
            var response = await purchaseService.GetPurchases();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Purchases/5
        [HttpGet("Purchaseid")]
        public async Task<ActionResult<Purchase>> GetPurchase(long PurchaseId)
        {
            var response = await purchaseService.GetPurchase(PurchaseId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

      

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PurchaseAShoppingCart(PurchasePost purchasePost)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseService.PurchaseAShoppingCart(purchasePost);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
                }
                var purchase = response.Result;
                return CreatedAtAction(nameof(GetPurchase), new { id=purchase.Id }, purchase);
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
            var response = await purchaseService.DeletePurchase(purchaseId);
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
                var response = await purchaseService.UpdatePurchase(purchaseid, post);
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
        public async Task<ActionResult<Purchase>> BrowesePurchaseHistory(long memberId)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseService.BrowesePurchaseHistory(memberId);
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
        public async Task<ActionResult<Purchase>> BroweseShopPurchaseHistory(long storeId)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseService.BroweseShopPurchaseHistory(storeId);
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
