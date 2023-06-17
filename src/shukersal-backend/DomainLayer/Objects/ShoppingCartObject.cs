using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.Models.ShoppingCartModels;
using NuGet.ContentModel;
using Microsoft.IdentityModel.Tokens;

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


        public async Task<Response<ShoppingBasket>> GetBasketByStoreId(long storeId)
        {
            var ShoppingBasket = await Context.ShoppingBaskets.Where(b => b.ShoppingCartId == Id && b.StoreId==storeId).FirstOrDefaultAsync();
            if (ShoppingBasket == null)
            {
                return Response<ShoppingBasket>.Error(HttpStatusCode.NotFound, "Basket doesn't exist.");
            }
            return Response<ShoppingBasket>.Success(HttpStatusCode.OK, ShoppingBasket);

        }

        public async Task<Response<ShoppingBasket>> GetBasket(long BasketId)
        {
            var ShoppingBasket=await Context.ShoppingBaskets.Where(b=>b.Id==BasketId).FirstOrDefaultAsync();
            if (ShoppingBasket == null)
            {
                return Response<ShoppingBasket>.Error(HttpStatusCode.NotFound, "Basket doesn't exist.");
            }
            return Response<ShoppingBasket>.Success(HttpStatusCode.OK, ShoppingBasket);
        }
        
        public async Task<Response<ShoppingItem>> GetShoppingItem(long shoppingItemId)
        {
            foreach(ShoppingBasketObject basket in ShoppingBaskets)
            {
                var item=await basket.GetShoppingItemFromBasket(shoppingItemId);
                if (item.Result != null) { return item; }
            }
            return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Cart does not contain this item.");

        }
        public async Task<Response<ShoppingItem>> GetShoppingItemByProductId(long productId)
        {
            foreach (ShoppingBasketObject basket in ShoppingBaskets)
            {
                var item = await basket.GetShoppingItemFromBasketByProductId(productId);
                if (item.Result != null) { return item; }
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

        public ShoppingCartObject(MarketDbContext context, ShoppingCart cart) 
        {
            Context = context;
            Id = cart.Id;
            MemberId = cart.MemberId;
            ShoppingBaskets=new List<ShoppingBasketObject>();
            cart.ShoppingBaskets.ToList().ForEach(basket => ShoppingBaskets.Add(new ShoppingBasketObject(context, basket)));

        }

        //new methods!
        public async Task<Response<ShoppingItem>> RemoveShoppingItem(long shoppingItemId)
        {
            var itemToRemove=await GetShoppingItem(shoppingItemId);
            if(itemToRemove==null || itemToRemove.Result==null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Cart does not contain this item.");
            }
            var basket = ShoppingBaskets.Where(b => b.Id == itemToRemove.Result.ShoppingBasketId).FirstOrDefault();
            if (basket == null)
            {
               return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Basket was not found.");
            }

            var respRemoval = await basket.RemoveItemFromBasket(shoppingItemId);
            if (respRemoval.IsSuccess)
            {
                var basketToRemove = await GetBasket(itemToRemove.Result.ShoppingBasketId);
                if(basketToRemove.IsSuccess && basketToRemove.Result!=null)
                {
                    Context.ShoppingBaskets.Remove(basketToRemove.Result);
                    Context.SaveChanges();
                    ShoppingBaskets.Remove(basket);
                }
                
            }

            return respRemoval;
        }

        public async Task<Response<ShoppingItem>> RemoveShoppingItemByProductId(long productId)
        {
            var itemToRemove = await GetShoppingItemByProductId(productId);
            if (itemToRemove == null || itemToRemove.Result == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Cart does not contain this item.");
            }
            var basket = ShoppingBaskets.Where(b => b.Id == itemToRemove.Result.ShoppingBasketId).FirstOrDefault();
            if (basket == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Basket was not found.");
            }

            var respRemoval = await basket.RemoveItemFromBasketByProductId(productId);
            if (respRemoval.IsSuccess)
            {
                var basketToRemove = await GetBasket(itemToRemove.Result.ShoppingBasketId);
                if (basketToRemove.IsSuccess && basketToRemove.Result != null)
                {
                    Context.ShoppingBaskets.Remove(basketToRemove.Result);
                    Context.SaveChanges();
                    ShoppingBaskets.Remove(basket);
                }

            }

            return respRemoval;
        }
        
        public async Task<Response<ShoppingItem>> AddShoppingItem(ShoppingItemPost shoppingItemPost)
        {
            var basketResp = await GetBasketByStoreId(shoppingItemPost.StoreId);
            ShoppingBasketObject basketObject;
            if(basketResp != null && basketResp.Result!=null)
            {
                basketObject=new ShoppingBasketObject(Context,basketResp.Result);
            }
            else
            {
                var basket = new ShoppingBasket
                {
                    ShoppingCartId = Id,
                    StoreId = shoppingItemPost.StoreId,
                    ShoppingItems = new List<ShoppingItem>(),
                };
                Context.ShoppingBaskets.Add(basket);
                Context.SaveChanges();
                basketObject=new ShoppingBasketObject(Context,basket);
            }
            return basketObject.AddItemToBasket(shoppingItemPost).Result;
        }

        public async Task<Response<ShoppingItem>> EditItemQuantity(long StoreId, long ProductId, int Quantity)
        {
            var basketResp = await GetBasketByStoreId(StoreId);
            if (basketResp != null && basketResp.Result != null)
            {
                var basketObj =ShoppingBaskets.Where(b => b.Id == basketResp.Result.Id).FirstOrDefault();
                if (basketObj==null) { return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Basket does not exist"); }

                return await basketObj.EditItemQuantity(ProductId, Quantity);

            }
            return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Basket does not exist");

        }
    }
}
