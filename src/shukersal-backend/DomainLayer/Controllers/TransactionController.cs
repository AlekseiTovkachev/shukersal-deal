using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.ContentModel;
using NuGet.Packaging;
using shukersal_backend.DomainLayer.ExternalServices;
using shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService;
using shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Utility;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class TransactionController
    {
        private readonly MarketDbContext _context;
        private readonly PaymentProxy _paymentProvider;
        private readonly DeliveryProxy _deliveryProvider;

        private readonly MarketObject _marketObject;
        private readonly StoreObject _storeObject;

        //private readonly ShoppingCartObject _shoppingCartObject; 
        //private readonly MemberObject _memberObject;


        public TransactionController(MarketDbContext context)
        {
            _context = context;
            _paymentProvider = new PaymentProxy();
            _deliveryProvider = new DeliveryProxy();
            _marketObject = new MarketObject(context);
            _storeObject = new StoreObject(context, _marketObject);

        }


        public DeliveryProxy getDeliveryProxy() { return _deliveryProvider; }

        public PaymentProxy getPaymentProxy() { return _paymentProvider; }

        public async Task<Response<IEnumerable<Transaction>>> GetTransactions()
        {
            var Transactions = await _context.Transactions.Include(s => s.TransactionItems).ToListAsync();
            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, Transactions);
        }

        public async Task<Response<Transaction>> GetTransaction(long Transactionid)
        {
            var Transaction = await _context.Transactions
                .Include(s => s.TransactionItems)
                .FirstOrDefaultAsync(s => s.Id == Transactionid);

            if (Transaction == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Not found");
            }
            return Response<Transaction>.Success(HttpStatusCode.OK, Transaction);
        }
    
 
       
        public async Task<Response<Transaction>> PurchaseAShoppingCart(TransactionPost TransactionPost)
        {

            var member = await _marketObject.GetMember(TransactionPost.MemberId);
            bool isGuest = member.Result == null;

            if (TransactionPost.TransactionItems.IsNullOrEmpty())
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Shopping cart is empty.");
            }


            var Transaction = new Transaction
            {
                MemberId = TransactionPost.MemberId,
                TransactionDate = TransactionPost.TransactionDate,
                TransactionItems= new List<TransactionItem>(),
                
            
            };

            Dictionary<long,List<TransactionItem>> TransactionBaskets=new Dictionary<long,List<TransactionItem>>();

            foreach (TransactionItem item in TransactionPost.TransactionItems)
            {
                if (!TransactionBaskets.ContainsKey(item.StoreId))
                {
                    TransactionBaskets.Add(item.StoreId, new List<TransactionItem>());
                }
                item.TransactionId = Transaction.Id;
                TransactionBaskets[item.StoreId].Add(item);
            }


            foreach(KeyValuePair<long,List<TransactionItem>> basket in TransactionBaskets)
            {

                var allAvailable = await CheckAvailabilityInStock(basket.Key, TransactionBaskets[basket.Key]);
                if (!allAvailable.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, allAvailable.ErrorMessage);
                }
                
               //var allAvailable= await _marketObject.CheckPurchasePolicy(basket.key, TransactionBaskets[basket.key]);

               var compliesWithTransactionPolicy = await CheckPurchasePolicy(basket.Key, TransactionBaskets[basket.Key]);
                if (!compliesWithTransactionPolicy.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, compliesWithTransactionPolicy.ErrorMessage);
                }

                //var discountsApplied = await  _marketObject.ApplyDiscounts(basket.StoreId, TransactionBaskets[basket.StoreId], TransactionPost.MemberId);
                var discountsApplied = await ApplyDiscounts(basket.Key, TransactionBaskets[basket.Key], TransactionPost.MemberId);
                if (!discountsApplied.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, discountsApplied.ErrorMessage);
                }
            }


            Transaction.TotalPrice = TransactionBaskets.Aggregate(0.0, (total, nextBasket) => total + nextBasket.Value.Aggregate(0.0, (totalBasket, item) => totalBasket + item.FinalPrice * item.Quantity));
            TransactionPost.BillingDetails.TotalPrice = TransactionPost.TotalPrice=Transaction.TotalPrice;
            TransactionBaskets.Values.ToList().ForEach(basket => Transaction.TransactionItems.AddRange(basket));

            //connction with external delivery service
            bool deliveryConfirmed = _deliveryProvider.ConfirmDelivery(TransactionPost.DeliveryDetails,Transaction.TransactionItems.ToList());
            if (!deliveryConfirmed)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Delivey declined");

            }
            //connction with external delivery service
            bool paymentConfirmed = _paymentProvider.ConfirmPayment(TransactionPost.BillingDetails);

            if (!paymentConfirmed)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Payment declined");

            }

            foreach (var (storeId, items) in TransactionBaskets)
            {
                //var stockUpdated= await _marketObject.UpdateStock(storeId, items);
                var stockUpdated = await UpdateStock(storeId, items);
                if (!stockUpdated.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, stockUpdated.ErrorMessage);
                }
                //TODO: sendTransactionNotification(storeId);
            }

            if (member.Id)
            {
               await _ShoppingCartObject.EmptyCart(member.Id);
            }


            _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();
            return Response<Transaction>.Success(HttpStatusCode.Created, Transaction);
        }


        public async Task<Response<IEnumerable<Transaction>>> BrowseTransactionHistory(long memberId)
        {
            var Transactions = await _context.Transactions
              .Include(s => s.TransactionItems).Where(s => s.MemberId == memberId).ToListAsync();
            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, Transactions);
        }

        public async Task<Response<IEnumerable<Transaction>>> BrowseShopTransactionHistory(long shopId)
        {
            var storeResp = await _marketObject.GetStore(shopId);

            if (storeResp.Result == null) {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.BadRequest,storeResp.ErrorMessage); }


            var purchasesId = new List<long>();
            var items = await _context.TransactionItems.ToListAsync();
            var group = items.GroupBy(it => new { it.StoreId, it.TransactionId });
            foreach (var it in group)
            {
                if (it.Key.StoreId == shopId)
                {
                    purchasesId.Add(it.Key.TransactionId);
                }
            }
            purchasesId.Distinct();

            List<Transaction> purchaseHistory = await _context.Transactions.Include(i => i.TransactionItems).Where(p => purchasesId.Contains(p.Id)).ToListAsync();
            foreach(Transaction t in purchaseHistory){
                List<TransactionItem> toRemove = new List<TransactionItem>(t.TransactionItems.Where(i=>i.StoreId!=shopId));
                foreach(TransactionItem item in toRemove)
                {
                    t.TransactionItems.Remove(item);
                }
            }

            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, purchaseHistory);
        }


        public async Task<Response<bool>> DeleteTransaction(long id)
        {
            if (_context.Transactions == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.Transactions'  is null.");
            }
            var Transaction = await _context.Transactions.FindAsync(id);
            if (Transaction == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Transaction not found");
            }

            foreach (var item in _context.TransactionItems)
            {
                if (item.TransactionId == id) { _context.TransactionItems.Remove(item); }
            }
            _context.Transactions.Remove(Transaction);
            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }

        private async Task<Response<bool>> CheckPurchasePolicy(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await _marketObject.GetStore(storeId);
            if (shop.Result==null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }

        private async Task<Response<bool>> ApplyDiscounts(long storeId, List<TransactionItem> TransactionItems, long memberId)
        {
            var shop = await _marketObject.GetStore(storeId);
            if (shop.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            var member = await _memberObject.GetMember(memberId);
            
            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


        private async Task<Response<bool>> CheckAvailabilityInStock(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await _marketObject.GetStore(storeId);
            if (shop.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            foreach (var TransactionItem in TransactionItems)
            {
                var product = await _storeObject.getProduct(TransactionItem.ProductId);
                if (product.Result == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
                if (product.Result.UnitsInStock < TransactionItem.Quantity) { return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store"); }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


        private async Task<Response<bool>> UpdateStock(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await _marketObject.GetStore(storeId);
            if (shop.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            foreach (var TransactionItem in TransactionItems)
            {

                var updated=_marketObject.UpdateProduct(storeId, TransactionItem.ProductId);
                var product = await _context.Products.FindAsync(TransactionItem.ProductId);
                if (product == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
                if (product.UnitsInStock < TransactionItem.Quantity)
                {
                    return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store");
                }
                else
                {
                    product.UnitsInStock = product.UnitsInStock - TransactionItem.Quantity;
                }
            }
            await _context.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


    }
}
