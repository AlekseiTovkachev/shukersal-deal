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
            if (shoppingCart == null)
            {
                return Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "User's shopping cart not found.");
            }
            return Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);

        }

        public async Task<Response<ShoppingCart>> GetShoppingCartById(long cartId, long loggedInMemberId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            if (shoppingCart == null)
            {
                return Response<ShoppingCart>.Error(HttpStatusCode.NotFound, "Shopping cart not found.");
            }
            else if (shoppingCart.MemberId != loggedInMemberId) { return Response<ShoppingCart>.Error(HttpStatusCode.Unauthorized, "Not the cart owner"); }
            return Response<ShoppingCart>.Success(HttpStatusCode.OK, shoppingCart);
        }

        public async Task<Response<ShoppingItem>> AddItemToCart(long cartId, ShoppingItemPost shoppingItemPost, long loggedInMemberId)
        {
            var resp = await GetShoppingCartById(cartId, loggedInMemberId);
            if (resp.Result != null && resp.IsSuccess)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem = await cart.AddShoppingItem(shoppingItemPost);
                if (ToAdditem.Result != null)
                {
                    return Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Response<ShoppingItem>.Error(resp.StatusCode, resp.ErrorMessage);
        }
        
        public async Task<Response<ShoppingItem>> RemoveItemFromCart(long cartId, long shoppingItemId, long loggedInMemberId)
        {
            var resp = await GetShoppingCartById(cartId, loggedInMemberId);
            if (resp.Result != null && resp.IsSuccess)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem = await cart.RemoveShoppingItem(shoppingItemId);
                if (ToAdditem.IsSuccess && ToAdditem.Result != null)
                {
                    return Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Response<ShoppingItem>.Error(resp.StatusCode, resp.ErrorMessage);
        }

        public async Task<Response<ShoppingItem>> RemoveItemFromCartByProductId(long cartId, long productId, long loggedInMemberId)
        {
            var resp = await GetShoppingCartById(cartId, loggedInMemberId);
            if (resp.Result != null && resp.IsSuccess)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, resp.Result);
                var ToAdditem = await cart.RemoveShoppingItemByProductId(productId);
                if (ToAdditem.IsSuccess && ToAdditem.Result != null)
                {
                    return Response<ShoppingItem>.Success(HttpStatusCode.OK, ToAdditem.Result);
                }
                return ToAdditem;
            }
            return Response<ShoppingItem>.Error(resp.StatusCode, resp.ErrorMessage);
        }

        public async Task<Response<ShoppingItem>> EditItemQuantity(long cartId, ShoppingItemPost item, long loggedInMemberId)
        {
            var Cartresp = await GetShoppingCartById(cartId, loggedInMemberId);
            if (Cartresp.Result != null && Cartresp.IsSuccess)
            {
                ShoppingCartObject cart = new ShoppingCartObject(_context, Cartresp.Result);
                var ToAdditem = await cart.EditItemQuantity(item.StoreId, item.ProductId, item.Quantity);
                return ToAdditem;

            }
            return Response<ShoppingItem>.Error(Cartresp.StatusCode, Cartresp.ErrorMessage);

        }
    }
}
