using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class ShoppingCartController : AbstractController
    {
        private MarketObject _marketObject;

        public ShoppingCartController(MarketDbContext context) : base(context)
        {
            _marketObject = new MarketObject(context);
        }

        public async Task<Utility.Response<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.MemberId == memberId);
            if(shoppingCart == null)
            {
                return Utility.Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "User's shopping cart not found.");
            }
            return Utility.Response<ShoppingCart>.Success(HttpStatusCode.OK,shoppingCart);

        }

        public async Task<Utility.Response<ShoppingCart>> GetShoppingCartById(long cartId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (shoppingCart == null)
            {
                return Utility.Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
            }
            return Utility.Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);
        }

        public async Task<Utility.Response<ShoppingItem>> AddItemToCart(long cartId, ShoppingItem item) {
            var resp = await GetShoppingCartById(cartId);
            if (resp.Result != null) 
            {
                ShoppingCartObject cart = new ShoppingCartObject (_context,resp.Result);
                var ToAdditem =await cart.AddShoppingItem(item);
                if (ToAdditem.Result != null)
                {
                    return Utility.Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
                return Utility.Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
        }

        public async Task<Utility.Response<ShoppingItem>> RemoveItemFromCart(long cartId, ShoppingItem item) 
        {
            var resp = await GetShoppingCartById(cartId);
            if (resp.Result != null)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem =await cart.RemoveShoppingItem(item.Id);
                if (ToAdditem.Result != null)
                {
                    return Utility.Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Utility.Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");

        }

        public async Task<Utility.Response<ShoppingItem>> EditItemQuantity(long cartId, ShoppingItem item)
        {
            var resp = await GetShoppingCartById(cartId);
            if (resp.Result != null)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem = await cart.EditItemQuantity(item.Id, item.Quantity);
                if (ToAdditem.Result != null)
                {
                    return Utility.Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Utility.Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");

        }

    }
}
