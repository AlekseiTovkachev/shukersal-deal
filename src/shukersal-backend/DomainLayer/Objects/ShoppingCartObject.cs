using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace shukersal_backend.DomainLayer.Objects
{
    public class ShoppingCartObject
    {
        public MarketDbContext Context;
        //public ShoppingBasketObject basketObject;

        
        public long Id;
        public long MemberId;
        //public List<ShoppingBasketObject> ShoppingBasketObjects;
        public List<ShoppingBasketObject> ShoppingBaskets;


        public ShoppingCartObject(MarketDbContext context, long Id, long MemberId, List<ShoppingBasket> baskets)
        {   Context = context;
            this.Id=Id;
            this.MemberId=MemberId;
            
            ShoppingBaskets = new List<ShoppingBasketObject>();
            baskets.ForEach(basket => ShoppingBaskets.Add(new ShoppingBasketObject(context, basket)));
        }

        public Response<ShoppingBasketObject> GetStoreBasket(long storeId)
        {
            ShoppingBasketObject? ShoppingBasket= ShoppingBaskets.Find(basket=>basket.StoreId==storeId);
            if (ShoppingBasket == null)
            {
                return Response<ShoppingBasketObject>.Error(HttpStatusCode.NotFound, "Store basket doesn't exist.");
            }
            return Response<ShoppingBasketObject>.Success(HttpStatusCode.OK, ShoppingBasket);
        }

        public async Task<Response<ShoppingBasketObject>> GetBasket(long BasketId)
        {
            var ShoppingBasket=await Context.ShoppingBaskets.Where(b=>b.Id==BasketId).FirstOrDefaultAsync();
            if (ShoppingBasket == null)
            {
                return Response<ShoppingBasketObject>.Error(HttpStatusCode.NotFound, "Basket doesn't exist.");
            }
            return Response<ShoppingBasketObject>.Success(HttpStatusCode.OK, new ShoppingBasketObject(Context,ShoppingBasket));
        }

        public async Task<Response<ShoppingItem>> GetShoppingItem(long shoppingItemId)
        {
            foreach(ShoppingBasketObject basket in ShoppingBaskets)
            {
                var item=await basket.GetItemFromBasket(shoppingItemId);
                if (item.Result != null) { return item; }
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Cart does not contain this item.");

        }

        public async Task<Response<ShoppingItem>> RemoveShoppingItem(long shoppingItemId)
        {
            foreach (ShoppingBasketObject basket in ShoppingBaskets)
            {
                var item =await basket.RemoveItemFromBasket(shoppingItemId);
                if (item.Result != null) 
                {
                    return item;
                }
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Cart does not contain this item.");

        }

        public async Task<Response<ShoppingItem>> EditItemQuantity(long itemId, int quantity)
        {
            foreach (ShoppingBasketObject basket in ShoppingBaskets)
            {
                var item = await basket.EditItemQuantity(itemId,quantity);
                if (item.Result != null)
                {
                    return item;
                }
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Cart does not contain this item.");

        }

        public async Task<Response<IEnumerable<ShoppingBasketObject>>> EmptyCart()
        {
            var toRemove = await Context.ShoppingBaskets.Where(basket => basket.ShoppingCartId == Id).ToListAsync();
            Context.ShoppingBaskets.RemoveRange(toRemove);
            await Context.SaveChangesAsync();
            ShoppingBaskets.Clear();
            
            return Response<IEnumerable<ShoppingBasketObject>>.Success(HttpStatusCode.OK, ShoppingBaskets);

        }

        public async Task<Response<ShoppingItem>> AddShoppingItem(ShoppingItem shoppingItem)
        {
            var basket = await GetBasket(shoppingItem.ShoppingBasketId);
            if (basket.Result != null)
            {
               return basket.Result.AddItemToBasket(shoppingItem).Result;
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Basket does not exist");
           
        }


        public ShoppingCartObject(MarketDbContext context, ShoppingCart cart) 
        {
            Context = context;
            Id = cart.Id;
            MemberId = cart.MemberId;
            ShoppingBaskets=new List<ShoppingBasketObject>();
            cart.ShoppingBaskets.ToList().ForEach(basket => ShoppingBaskets.Add(new ShoppingBasketObject(context, basket)));

        }


        public async Task<Response<ShoppingItem>> EditItemQuantity(long itemId, long basketId, int quantity)
        {
            var basket = await GetBasket(basketId);
            if (basket.Result != null)
            {
                return await basket.Result.EditItemQuantity(itemId,quantity);
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Basket does not exist");

        }



    }
}
