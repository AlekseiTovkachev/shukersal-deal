using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Objects
{
    public class ShoppingBasketObject
    {
        public MarketDbContext context;
        //private ShoppingBasket basket;
        public long Id;

        public long ShoppingCartId;

        public long StoreId { get; set; }

    public ICollection<ShoppingItem> ShoppingItems;

        public ShoppingBasketObject(MarketDbContext context, ShoppingBasket basket)
        {
            this.context = context;
            Id= basket.Id;
            ShoppingCartId= basket.ShoppingCartId;
            StoreId= basket.StoreId;
            ShoppingItems=new List<ShoppingItem>(basket.ShoppingItems);
        }

        public async Task<Response<ShoppingItem>> GetItemFromBasket(long itemId)
        {
            var item= await context.ShoppingItems.Where(i=>i.Id==itemId).FirstOrDefaultAsync();
            if (item == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Item Is not in basket.");
            }
            return Response<ShoppingItem>.Success(HttpStatusCode.OK, item);
        }

        public async Task<Response<ShoppingItem>> AddItemToBasket(ShoppingItem item)
        {
            if (item.Quantity <= 0)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Item's quantity must be positive.");
            }
            if (GetItemFromBasket(item.ProductId)!=null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Item Is already in basket.");
            }
            ShoppingItems.Add(item);
            context.ShoppingItems.Add(item);
            await context.SaveChangesAsync();
            return Response<ShoppingItem>.Success(HttpStatusCode.OK,item);
        }

        public async Task<Response<ShoppingItem>> RemoveItemFromBasket(long itemId)
        {
            var itemToRemove = await GetItemFromBasket(itemId);
            if (itemToRemove.Result!=null)
            {
                ShoppingItems.Remove(itemToRemove.Result);
                context.ShoppingItems.Remove(itemToRemove.Result);
               await context.SaveChangesAsync();
            }
            return itemToRemove;
        }
        public async Task<Response<ShoppingItem>> EditItemQuantity(long itemId,int quantity)
        {
            var itemToEdit = await GetItemFromBasket(itemId);
            if(itemToEdit.Result == null)
            {
                return itemToEdit;
            }

            if (quantity <= 0)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Item's quantity must be positive.");

            }
            itemToEdit.Result.Quantity = quantity;
            context.Entry(itemToEdit).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return itemToEdit;

        }

    }
}