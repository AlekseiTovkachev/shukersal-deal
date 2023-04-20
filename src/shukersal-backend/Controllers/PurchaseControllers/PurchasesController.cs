using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using shukersal_backend.Domain;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.Controllers.PurchaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseService purchaseService;


        public PurchasesController(PurchaseContext context, StoreContext storeContext, MemberContext memberContext, ShoppingCartContext shoppingCartContext)
        {
            purchaseService=new PurchaseService(context, storeContext, memberContext, shoppingCartContext);
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
        [HttpGet("{Purchaseid}")]
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
                    return BadRequest(ModelState);
                }
                var purchase = response.Result;
                return CreatedAtAction("GetPurchase", new { id = purchasePost.Id }, purchase);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        // DELETE: api/Purchases/5
        [HttpDelete("{purchaseId}")]
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
        [HttpPut("{purchaseid}")]
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
                return NoContent();

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
                return NoContent();

            }
            return BadRequest();
        }

        // GET: api/Purchases/storeId/5
        [HttpGet("storeId/{storeId}")]
        public async Task<ActionResult<Purchase>> BroweseShopPurchaseHistory(long storeId)
        {
            if (ModelState.IsValid)
            {
                var response = await purchaseService.BrowesePurchaseHistory(storeId);
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









    }

        

}
