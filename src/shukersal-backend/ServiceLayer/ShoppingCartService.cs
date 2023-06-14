using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.ShoppingCartModels;
using shukersal_backend.Utility;

namespace shukersal_backend.ServiceLayer
{
    // TODO: Move logic to domain
    [Route("api/shoppingcarts")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ShoppingCartService : ControllerBase
    {
        private readonly MarketDbContext _context;
        private readonly ShoppingCartController _shoppingCartController;
        private readonly ILogger<ControllerBase> logger;

        public ShoppingCartService(MarketDbContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _shoppingCartController = new ShoppingCartController(context);
            this.logger = logger;

        }
        // GET: api/ShoppingCarts/memberId/5
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            logger.LogInformation("GetShoppingCartByUserId Called with {memberId}", memberId);
            var currentMember= ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null || currentMember.Id!=memberId) {return Unauthorized(); }
            var response = await _shoppingCartController.GetShoppingCartByUserId(memberId);
            if (response.Result == null)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Result);
        }

        // GET: api/ShoppingCarts/memberId/5
        [HttpGet("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByCartId(long cartId)
        {
            logger.LogInformation("GetShoppingCartByCartId Called with {cartId}", cartId);
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null) { return Unauthorized(); }

            var response = await _shoppingCartController.GetShoppingCartById(cartId,currentMember.Id);
            if (response.Result == null || !response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Result);
        }


        // POST: api/ShoppingCarts/5/items
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult<ShoppingItem>> AddItemToCart(long cartId,ShoppingItemPost shoppingItemPost)
        {
            logger.LogInformation("AddItemToCart Called with cart id:{cartId}, product id:{ProductId}, store id:{StoreId}, quantity:{Quantity}", cartId, shoppingItemPost.ProductId,shoppingItemPost.StoreId, shoppingItemPost.Quantity);
           
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null) { return Unauthorized();}
           
            var itemResp = await _shoppingCartController.AddItemToCart(cartId,shoppingItemPost,currentMember.Id);
            if (itemResp.Result != null && itemResp.IsSuccess)
            {
                return itemResp.Result;
            }
            return BadRequest(itemResp.ErrorMessage);
        }

        [HttpDelete("{cartId}/items")]
        public async Task<ActionResult<ShoppingItem>> RemoveItemFromCart(long cartId,long shoppingItemId)
        {
            logger.LogInformation("RemoveItemFromCart Called with cart id:{cartId}, item id:{Id}", cartId, shoppingItemId);
             var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
             if (currentMember == null){return Unauthorized();}
            var itemResp = await _shoppingCartController.RemoveItemFromCart(cartId, shoppingItemId,currentMember.Id);
            if (itemResp.Result != null && itemResp.IsSuccess)
            {
                return itemResp.Result;
            }
            return BadRequest(itemResp.ErrorMessage);
        }

        [HttpPut("{cartId}/items")]
        public async Task<ActionResult<ShoppingItem>> EditItemQuantity(long cartId,ShoppingItemPost item)
        {
            logger.LogInformation("PutStoreManager Called with cart id:{cartId}, product id:{ProductId}, quantity:{Quantity}", cartId, item.ProductId, item.Quantity);
            var currentMember = ServiceUtilities.GetCurrentMember(_context, HttpContext);
            if (currentMember == null){return Unauthorized();}
            var response = await _shoppingCartController.EditItemQuantity(cartId, item,currentMember.Id);

            if (response.Result != null && response.IsSuccess)
            {
                return response.Result;
            }
            return BadRequest(response.ErrorMessage);
        }

    }

}
