using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Utility;

namespace shukersal_backend.ServiceLayer
{
    // TODO: Move logic to domain
    [Route("api/shoppingcarts")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ShoppingCartService : ControllerBase
    {
        //private readonly MarketDbContext _context;
        private readonly ShoppingCartController _shoppingCartController;
        private readonly Member? currentMember;
        private readonly ILogger<ControllerBase> logger;

        public ShoppingCartService(MarketDbContext context, ILogger<ControllerBase> logger)
        {
            _shoppingCartController = new ShoppingCartController(context);
            currentMember = ServiceUtilities.GetCurrentMember(context, HttpContext);
            this.logger = logger;

        }
        // GET: api/ShoppingCarts/memberId/5
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            logger.LogInformation("GetShoppingCartByUserId Called with {memberId}", memberId);
            if (currentMember == null) { Unauthorized(); }

            var response = await _shoppingCartController.GetShoppingCartByUserId(memberId);
            if (response.Result == null)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/ShoppingCarts/memberId/5
        [HttpGet("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByCartId(long cartId)
        {
            logger.LogInformation("GetShoppingCartByCartId Called with {cartId}", cartId);
            if (currentMember == null) { Unauthorized(); }

            var response = await _shoppingCartController.GetShoppingCartById(cartId);
            if (response.Result == null)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // POST: api/ShoppingCarts/5/items
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult<ShoppingItem>> AddItemToCart(long cartId, ShoppingItem item)
        {
            logger.LogInformation("AddItemToCart Called with cart id:{cartId}, item id:{Id}, product id:{ProductId}, quantity:{Quantity}", cartId, item.Id, item.ProductId, item.Quantity);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var itemResp = await _shoppingCartController.AddItemToCart(cartId, item);
            if (itemResp.Result != null && itemResp.IsSuccess)
            {
                return itemResp.Result;
            }
            return BadRequest(itemResp.ErrorMessage);
        }

        [HttpDelete("{cartid}/items")]
        public async Task<ActionResult<ShoppingItem>> RemoveItemFromCart(long cartId, ShoppingItem item)
        {
            logger.LogInformation("RemoveItemFromCart Called with cart id:{cartId}, item id:{Id}, product id:{ProductId}, quantity:{Quantity}", cartId, item.Id, item.ProductId, item.Quantity);
            if (currentMember == null)
            {
                return Unauthorized();
            }
            var itemResp = await _shoppingCartController.RemoveItemFromCart(cartId, item);
            if (itemResp.Result != null && itemResp.IsSuccess)
            {
                return itemResp.Result;
            }
            return BadRequest(itemResp.ErrorMessage);
        }

        [HttpPut("{cartId}/items")]
        public async Task<ActionResult<ShoppingItem>> EditItemQuantity(long cartId, ShoppingItem item)
        {
            logger.LogInformation("PutStoreManager Called with cart id:{cartId}, item id:{Id}, product id:{ProductId}, quantity:{Quantity}", cartId, item.Id, item.ProductId, item.Quantity);
            var response = await _shoppingCartController.EditItemQuantity(cartId, item);
            if (!response.IsSuccess)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                return NotFound();
            }
            return Ok(response.Result);
        }
    }
}
