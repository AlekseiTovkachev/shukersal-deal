using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace shukersal_backend.Domain
{
    public class PurchaseService
    {
        private readonly MarketDbContext _context;

        public PurchaseService(MarketDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<Purchase>>> GetPurchases()
        {
            if (_context.Purchases == null)
            {
                return Response<IEnumerable<Purchase>>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.Purchases'  is null.");
            }
            var purchases = await _context.Purchases
               // .Include(s => s.PurchaseItems).Include(m=>m.Member_).ToListAsync();
               .Include(s => s.PurchaseItems).ToListAsync();
            return Response<IEnumerable<Purchase>>.Success(HttpStatusCode.OK, purchases);
        }

        public async Task<Response<Purchase>> GetPurchase(long Purchaseid)
        {
            if (_context.Purchases == null)
            {
                return Response<Purchase>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.Purchases'  is null.");
            }
            var purchase = await _context.Purchases
                .Include(s => s.PurchaseItems)
                //.Include(m=>m.Member_)
                .FirstOrDefaultAsync(s => s.Id == Purchaseid);
            
            if (purchase == null)
            {
                return Response<Purchase>.Error(HttpStatusCode.NotFound, "Not found");
            }
            return Response<Purchase>.Success(HttpStatusCode.OK, purchase);
        }

        public async Task<Response<Purchase>> PurchaseAShoppingCart(PurchasePost purchasePost) {

            if (_context.Purchases == null)
            {
                return Response<Purchase>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.Purchases'  is null.");
            }
            if (_context.PurchaseItems == null)
            {
                return Response<Purchase>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.PurchaseItems'  is null.");
            }


            var purchase = new Purchase
            {
                Member_Id_ = purchasePost.Member__ID,
                //Member_ =null,
                PurchaseDate = purchasePost.PurchaseDate,
                TotalPrice = purchasePost.TotalPrice,
                PurchaseItems =new List<PurchaseItem>(),
            };

            var member = await _context.Members.FindAsync(purchasePost.Member__ID);
            if (member == null)
            {
                return Response<Purchase>.Error(HttpStatusCode.BadRequest, "Illegal user id");
            }
            //purchase.Member_ = member;

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.MemberId == purchasePost.Member__ID);

            if (shoppingCart == null)
            {
                return Response<Purchase>.Error(HttpStatusCode.BadRequest, "No shopping cart.");
            }

            if(shoppingCart.ShoppingBaskets.Count == 0)
            {
                return Response<Purchase>.Error(HttpStatusCode.BadRequest, "Shopping cart is empty.");
            }

            var purchaseBaskets = new Dictionary<long, List<PurchaseItem>>();
            foreach(ShoppingBasket basket in shoppingCart.ShoppingBaskets)
            {

                purchaseBaskets.Add(basket.StoreId, new List<PurchaseItem>());
                
                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    var purchaseItem = new PurchaseItem
                    {
                        PurchaseId= purchase.Id,
                       // Purchase= purchase,
                        ProductId = item.ProductId,
                        StoreId = item.Product.StoreId,
                        ProductName = item.Product.Name,
                        ProductDescription = item.Product.Description,
                        Quantity = item.Quantity,
                        FullPrice = item.Product.Price,
                        FinalPrice = item.Product.Price,
                    };
                    purchaseBaskets[basket.StoreId].Add(purchaseItem);
                }

                
                var allAvailable = await CheckAvailabilityInStock(basket.StoreId, purchaseBaskets[basket.StoreId]);
                if (!allAvailable.Result)
                {
                    return Response<Purchase>.Error(HttpStatusCode.BadRequest, allAvailable.ErrorMessage);
                }

                var compliesWithPurchasePolicy= await CheckPurchasePolicy(basket.StoreId,purchaseBaskets[basket.StoreId]);
                if (!compliesWithPurchasePolicy.Result)
                {
                    return Response<Purchase>.Error(HttpStatusCode.BadRequest, compliesWithPurchasePolicy.ErrorMessage);
                }

                var discountsApplied = await ApplyDiscounts(basket.StoreId, purchaseBaskets[basket.StoreId], purchasePost.Member__ID);
                if (!discountsApplied.Result)
                {
                    return Response<Purchase>.Error(HttpStatusCode.BadRequest, discountsApplied.ErrorMessage);
                }

            }
            purchase.TotalPrice = purchaseBaskets.Aggregate(0.0, (total, nextBasket) => total + nextBasket.Value.Aggregate(0.0, (totalBasket, item) => totalBasket + item.FinalPrice*item.Quantity));
            

            //connction with external delivery service
            //TODO: confirmDelivery(purchaseItems,~delivery details~);

            //connction with external delivery service
            //TODO: confirmPayment(amount,~payment details~);

       
            foreach (var (storeId,items) in purchaseBaskets)
            {
                purchase.PurchaseItems.AddRange(items);
                var stockUpdated=await UpdateStock(storeId,items);
                if (!stockUpdated.Result)
                {
                    return Response<Purchase>.Error(HttpStatusCode.BadRequest, stockUpdated.ErrorMessage);
                }
                //TODO: sendPurchaseNotification(storeId);
            }

            
            //TODO: remove all baskets from shopping cart


            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return Response<Purchase>.Success(HttpStatusCode.Created, purchase);
        }


        public async Task<Response<IEnumerable<Purchase>>> BrowesePurchaseHistory(long memberId) {
            if (_context.Purchases == null)
            {
                return Response<IEnumerable<Purchase>>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.Purchases'  is null.");
            }

            var purchases = await _context.Purchases
              .Include(s => s.PurchaseItems).Where(s => s.Member_Id_ == memberId).ToListAsync();
            return Response<IEnumerable<Purchase>>.Success(HttpStatusCode.OK, purchases);
        }

        public async Task<Response<IEnumerable<Purchase>>> BroweseShopPurchaseHistory(long shopId)
        {
            if (_context.Purchases == null)
            {
                return Response<IEnumerable<Purchase>>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.Purchases'  is null.");
            }

            var purchasesId =new List<long>();
            var items = await _context.PurchaseItems.ToListAsync();
            var group = items.GroupBy(it => new {it.StoreId,it.PurchaseId}) ;
            foreach(var it in group)
            {
                if (it.Key.StoreId == shopId)
                {
                    purchasesId.Add(it.Key.PurchaseId);
                }
            }
            purchasesId.Distinct();

            List<Purchase> purchaseHistory = await _context.Purchases.Include(i => i.PurchaseItems).Where(p => purchasesId.Contains(p.Id)).ToListAsync();

            return Response<IEnumerable<Purchase>>.Success(HttpStatusCode.OK, purchaseHistory);
             
        }


        private bool PurchaseExists(long id)
        {
            return (_context.Purchases?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Response<bool>> UpdatePurchase(long id, PurchasePost post)
        {

            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Purchase not found");
            }
            
            purchase.PurchaseDate = post.PurchaseDate;
            purchase.TotalPrice = post.TotalPrice;

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
                {
                    return Response<bool>.Error(HttpStatusCode.NotFound, "not found");
                }
                else
                {
                    throw;
                }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        public async Task<Response<bool>> DeletePurchase(long id)
        {
            if (_context.Purchases == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Entity set 'PurchaseContext.Purchases'  is null.");
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Purchase not found");
            }

            foreach (var item in _context.PurchaseItems) {
                if(item.PurchaseId == id) { _context.PurchaseItems.Remove(item); }
            }
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        private async Task<Response<bool>> CheckPurchasePolicy(long storeId,List<PurchaseItem> purchaseItems)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }

        private async Task<Response<bool>> ApplyDiscounts(long storeId, List<PurchaseItem> purchaseItems,long memberId)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal member id");
            }
            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


        private async Task<Response<bool>> CheckAvailabilityInStock(long storeId, List<PurchaseItem> purchaseItems)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            foreach(var purchaseItem in purchaseItems)
            {
                var product= await _context.Products.FindAsync(purchaseItem.ProductId);
                if (product == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
               // if (product.UnitsInStock < purchaseItem.Quantity) { return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store"); }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


        private async Task<Response<bool>> UpdateStock(long storeId, List<PurchaseItem> purchaseItems)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            foreach (var purchaseItem in purchaseItems)
            {
                var product = await _context.Products.FindAsync(purchaseItem.ProductId);
                if (product == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
                if (product.UnitsInStock < purchaseItem.Quantity) 
                { 
                    return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store"); 
                }
                else 
                { 
                    product.UnitsInStock = product.UnitsInStock - purchaseItem.Quantity; 
                }
            }
            await _context.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


    }
}
