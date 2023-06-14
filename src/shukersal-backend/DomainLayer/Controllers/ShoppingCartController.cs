using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
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

        public async Task<Response<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.MemberId == memberId);
            if(shoppingCart == null)
            {
                return Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "User's shopping cart not found.");
            }
            return Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);

        }

        public async Task<Response<ShoppingCart>> GetShoppingCartById(long cartId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (shoppingCart == null)
            {
                return Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
            }
            return Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);
        }

        public async Task<Response<ShoppingItem>> AddItemToCart(long cartId,ShoppingItemPost shoppingItemPost)
        {
            var resp = await GetShoppingCartById(cartId);
            if (resp.Result != null)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem = await cart.AddShoppingItem(shoppingItemPost);
                if (ToAdditem.Result != null)
                {
                    return Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
        }

        public async Task<Response<ShoppingItem>> RemoveItemFromCart(long cartId, long shoppingItemId)
        {
            var resp = await GetShoppingCartById(cartId);
            if (resp.Result != null)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem = await cart.RemoveShoppingItem(shoppingItemId);
                if (ToAdditem.IsSuccess && ToAdditem.Result != null)
                {
                    return Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
        }

        public async Task<Response<ShoppingItem>> EditItemQuantity(long cartId, ShoppingItemPost item)
        {
            var Cartresp = await GetShoppingCartById(cartId);
            if (Cartresp.Result != null)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, Cartresp.Result);
                var ToAdditem = await cart.EditItemQuantity(item.StoreId,item.ProductId,item.Quantity);
                return ToAdditem;
                /*
                if (ToAdditem!=null && ToAdditem.Result != null)
                {
                    return Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                //if(ToAdditem.IsSuccess)
                */
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");

        }
    }
}
