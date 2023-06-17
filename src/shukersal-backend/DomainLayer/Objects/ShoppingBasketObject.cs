using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.DomainLayer.Objects
{
    public class ShoppingBasketObject
    {
        public MarketDbContext context;
        //private ShoppingBasket basket;
        public long Id;

        public long ShoppingCartId;

        public long StoreId { get; set; }

        public List<ShoppingItem> ShoppingItems;

        public ShoppingBasketObject(MarketDbContext context, ShoppingBasket basket)
        {
            this.context = context;
            Id= basket.Id;
            ShoppingCartId= basket.ShoppingCartId;
            StoreId= basket.StoreId;
            ShoppingItems=new List<ShoppingItem>(basket.ShoppingItems);
        }

        public async Task<Response<ShoppingItem>> GetShoppingItemFromBasket(long shoppingItemId)
        {
            var item= await context.ShoppingItems.Where(i=>i.Id == shoppingItemId).FirstOrDefaultAsync();
            if (item == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Item Is not in basket.");
            }
            return Response<ShoppingItem>.Success(HttpStatusCode.OK, item);
        }

        public async Task<Response<ShoppingItem>> GetShoppingItemFromBasketByProductId(long productId)
        {
            var item = await context.ShoppingItems.Where(i => i.ProductId == productId).FirstOrDefaultAsync();
            if (item == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "product Is not in basket.");
            }
            return Response<ShoppingItem>.Success(HttpStatusCode.OK, item);
        }

        /*
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
        */
        public async Task<Response<ShoppingItem>> RemoveItemFromBasket(long itemId)
        {
            var itemToRemove = await GetShoppingItemFromBasket(itemId);
            if (itemToRemove.Result!=null)
            {
                ShoppingItems.RemoveAll(i=>i.Id==itemId);
                context.ShoppingItems.Remove(itemToRemove.Result);
                await context.SaveChangesAsync();
            }
            return itemToRemove;
        }

        public async Task<Response<ShoppingItem>> RemoveItemFromBasketByProductId(long productId)
        {
            var itemToRemove = await GetShoppingItemFromBasketByProductId(productId);
            if (itemToRemove.Result != null)
            {
                ShoppingItems.RemoveAll(i => i.ProductId == productId);
                context.ShoppingItems.Remove(itemToRemove.Result);
                await context.SaveChangesAsync();
            }
            return itemToRemove;
        }
        /*
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
        */

        public async Task<Response<ShoppingItem>> AddItemToBasket(ShoppingItemPost shoppingItemPost)
        {
            if (shoppingItemPost.Quantity <= 0)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Item's quantity must be positive.");
            }
            if (GetShoppingItemFromBasketByProductId(shoppingItemPost.ProductId).Result.StatusCode==HttpStatusCode.OK)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Item Is already in basket.");
            }

            var product=await context.Products.FindAsync(shoppingItemPost.ProductId);
            if (product == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Product does not exist.");
            }
            ShoppingItem item = new ShoppingItem
            {
                ShoppingBasketId = Id,
                Product = product,
                Quantity = shoppingItemPost.Quantity,
            };
            ShoppingItems.Add(item);
            context.ShoppingItems.Add(item);
            await context.SaveChangesAsync();
            return Response<ShoppingItem>.Success(HttpStatusCode.OK, item);
        }

        public async Task<Response<ShoppingItem>> EditItemQuantity(long productId, int quantity)
        {
            var itemToEdit = await context.ShoppingItems.Where(i => i.ProductId == productId).FirstOrDefaultAsync();
            if (itemToEdit == null)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.NotFound, "Item was not found in the shopping basket.");
                
            }

            if (quantity <= 0)
            {
                return Response<ShoppingItem>.Error(HttpStatusCode.BadRequest, "Item's quantity must be positive.");

            }
            itemToEdit.Quantity = quantity;
            context.Entry(itemToEdit).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Response<ShoppingItem>.Success(HttpStatusCode.OK, itemToEdit);

        }
    }
}