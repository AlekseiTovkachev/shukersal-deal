using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.ExternalServices.ExternalDeliveryService;
using shukersal_backend.ExternalServices.ExternalPaymentService;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class TransactionController
    {
        private readonly MarketDbContext _context;
        
        private readonly TransactionObject _transactionObject;

        private readonly MarketObject _marketObject;
        private readonly StoreObject _storeObject;

        private readonly MemberObject _memberObject;


        public TransactionController(MarketDbContext context)
        {
            _context = context;
            _marketObject = new MarketObject(context);
            _storeObject = new StoreObject(context, _marketObject, new StoreManagerObject());
            _memberObject= new MemberObject(context);
            _transactionObject = new TransactionObject(context,_marketObject);
            _marketObject.SetDeliveryProvider("https://php-server-try.000webhostapp.com/");
            _marketObject.SetPaymentProvider("https://php-server-try.000webhostapp.com/");
        }

        public void SetRealDeliveryAdapter(string url)
        {
            _marketObject.SetDeliveryProvider("");
        }
        public void SetRealpaymentAdapter(string url)
        {
            _marketObject.SetPaymentProvider("");
        }
        public DeliveryProxy getDeliveryProxy() { return _marketObject.getDeliveryProxy(); }

        public PaymentProxy getPaymentProxy() { return _marketObject.getPaymentProxy(); }

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
            long memberId=0;
            var member = await _memberObject.GetMember(TransactionPost.MemberId);
            if (member.Result != null)
            {
                memberId = member.Result.Id;
            }

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

            var allValid = await CheckRulesAndApplyDiscounts(TransactionBaskets,memberId);

            if (!allValid.IsSuccess|| allValid.Result == null) { return Response<Transaction>.Error(HttpStatusCode.BadRequest, allValid.ErrorMessage); }

            Transaction.TotalPrice = TransactionBaskets.Aggregate(0.0, (total, nextBasket) => total + nextBasket.Value.Aggregate(0.0, (totalBasket, item) => totalBasket + item.FinalPrice * item.Quantity));
            TransactionPost.BillingDetails.TotalPrice = TransactionPost.TotalPrice=Transaction.TotalPrice;
            TransactionBaskets.Values.ToList().ForEach(basket => Transaction.TransactionItems.AddRange(basket));
            
            var gotExternalServicesConfirmation = _marketObject.confirmDeliveryAndPayment(TransactionPost.DeliveryDetails, Transaction.TransactionItems.ToList(),TransactionPost.BillingDetails);
            if(!gotExternalServicesConfirmation.Result.IsSuccess) {
                //rollback
                foreach (TransactionItem item in Transaction.TransactionItems)
                {  
                    var productToRollBack = await _context.Products.FindAsync(item.ProductId); ;
                    if (productToRollBack != null)
                    {
                        productToRollBack.UnitsInStock = productToRollBack.UnitsInStock + item.Quantity;
                        await _context.SaveChangesAsync();
                    }
                }
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, gotExternalServicesConfirmation.Result.ErrorMessage); 
            }

            
            foreach (var (storeId, items) in TransactionBaskets)
            {
                //TODO: sendTransactionNotification(storeId);
            }

            if (member.Result!=null)
            {
               await _marketObject.EmptyCart(member.Result.Id);
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
                List<TransactionItem> toRemove = t.TransactionItems.Where(i=>i.StoreId!=shopId).ToList();
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

        

        private async Task<Response<bool>> ApplyDiscounts(long storeId, List<TransactionItem> TransactionItems, long memberId)
        {
            var shop = await _marketObject.GetStore(storeId);
            if (shop.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, shop.ErrorMessage);
            }

            var member = await _memberObject.GetMember(memberId);
            var d = new DiscountObject(_context);
            await d.CalculateDiscount(shop.Result.AppliedDiscountRule, TransactionItems);
            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


        public async Task<Response<bool>> UpdateTransaction(long Transactionid, TransactionPost post) {
            return await _transactionObject.UpdateTransaction(Transactionid, post);
        }


        public async Task<Response<Dictionary<long, List<TransactionItem>>>> CheckRulesAndApplyDiscounts(Dictionary<long, List<TransactionItem>> TransactionBaskets ,long memberId )
        {
            foreach (KeyValuePair<long, List<TransactionItem>> basket in TransactionBaskets)
            {
                var compliesWithTransactionPolicy = await _marketObject.CheckPurchasePolicy(basket.Key, TransactionBaskets[basket.Key]);
                if (!compliesWithTransactionPolicy.Result)
                {
                    return Response<Dictionary<long, List<TransactionItem>>>.Error(HttpStatusCode.BadRequest, "Purchase rule vaiolation");
                }

                var discountsApplied = await ApplyDiscounts(basket.Key, TransactionBaskets[basket.Key], memberId);
                if (!discountsApplied.Result)
                {
                    return Response<Dictionary<long, List<TransactionItem>>>.Error(HttpStatusCode.BadRequest, discountsApplied.ErrorMessage);
                }

                var allAvailable = await _marketObject.UpdateStock(basket.Key, TransactionBaskets[basket.Key]);
                if (!allAvailable.Result)
                {
                    return Response<Dictionary<long, List<TransactionItem>>>.Error(HttpStatusCode.BadRequest, allAvailable.ErrorMessage);
                }
            }
            return Response<Dictionary<long, List<TransactionItem>>>.Success(HttpStatusCode.BadRequest, TransactionBaskets);
        }
        

    }
}
