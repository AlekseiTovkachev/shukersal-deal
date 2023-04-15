using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.Controllers.PurchaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseContext _context;
        private readonly StoreContext _storeContext;
        private readonly MemberContext _memberContext;


        public PurchasesController(PurchaseContext context, StoreContext storeContext, MemberContext memberContext)
        {
            _context = context;
            _storeContext = storeContext;
            _memberContext = memberContext;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            return await _context.Purchases.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(long id)
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(long id, Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PurchaseAShoppingCart(PurchasePost purchasePost)
        {
          if (_context.Purchases == null)
          {
              return Problem("Entity set 'PurchaseContext.Purchases'  is null.");
          }

            if (_context.PurchaseItems == null)
            {
                return Problem("Entity set 'PurchaseContext.PurchaseItems'  is null.");
            }


            if (ModelState.IsValid)
            {

                var member = await _memberContext.Members.FindAsync(purchasePost.MemberId);
                if (member == null)
                {
                    return Problem("Illegal user id");
                }

                var purchase = new Purchase(purchasePost.MemberId,member,purchasePost.PurchaseDate);
                ShoppingCart cart = member.ShoppingCart;

                decimal totalPrice = 0;
                foreach(ShoppingBasket basket in cart.ShoppingBaskets)
                {


                    foreach (ShoppingItem shoppingItem in basket.ShoppingItems)
                    {

                        //TO DO: update final price
                        //TO DO: check purchase policy
                        //TO DO: apply discount

                        var item = new PurchaseItem(purchase.Id,purchase,shoppingItem.Id,shoppingItem.Product,shoppingItem.Quantity,shoppingItem.Product.Price);
                        purchase.PurchaseItems.Add(item);

                        totalPrice = totalPrice + item.Price;
                    }

                }

                foreach(PurchaseItem purchaseItem in purchase.PurchaseItems)
                {
                    _context.PurchaseItems.Add(purchaseItem);
                    await _context.SaveChangesAsync();
                }

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchase);

            }
            else { return BadRequest(ModelState); }
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(long id)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(long id)
        {
            return (_context.Purchases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }


}
