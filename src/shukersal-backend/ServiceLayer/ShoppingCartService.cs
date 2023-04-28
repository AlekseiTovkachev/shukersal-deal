using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;

namespace shukersal_backend.ServiceLayer
{
    // TODO: Move logic to domain
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ShoppingCartService : ControllerBase
    {
        private readonly MarketDbContext _context;

        public ShoppingCartService(MarketDbContext context)
        {
            _context = context;
        }
        // GET: api/ShoppingCarts/memberId/5
        [HttpGet("memberId/{memberId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.MemberId == memberId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return shoppingCart;
        }

        // POST: api/ShoppingCarts/5/items
        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItemToCart(long id, [FromBody] ShoppingItem item)
        {
            // TODO: Implement addition based on store id (to the correct basket)
            return new StatusCodeResult(StatusCodes.Status501NotImplemented);
        }

        // DELETE: api/ShoppingCarts/5/items/1
        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(long id, long itemId)
        {
            // TODO: Implement removal of correct item, as well as removing of empty baskets
            return new StatusCodeResult(501); // Not implemented
        }
    }
}
