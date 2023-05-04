using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
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

namespace shukersal_backend.DomainLayer.Controllers
{
    public class TransactionController
    {
        private readonly MarketDbContext _context;
        private readonly PaymentProxy _paymentProvider;
        private readonly DeliveryProxy _deliveryProvider;
        private readonly MarketObject _marketObject;


        //Will be deleted when the relevant function are in market object:
        MemberService _memberService;
        ShoppingCartService _shoppingCartService;


        public TransactionController(MarketDbContext context)
        {
            _context = context;
            _paymentProvider = new PaymentProxy();
            _deliveryProvider = new DeliveryProxy();
            _marketObject = new MarketObject(context);
            //Will be deleted when the relevant function are in market object:
            _memberService = new MemberService(context);
            _shoppingCartService = new ShoppingCartService(context);
        }


        public DeliveryProxy getDeliveryService() { return _deliveryProvider; }

        public PaymentProxy getPaymentService() { return _paymentProvider; }

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
            //memberResp member = await _marketObject.GetMember(TransactionPost.MemberId);
            var memberResp = await _memberService.GetMember(TransactionPost.MemberId);
            if (memberResp == null || memberResp.Value == null)
            { return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Illegal user id"); }

            Member member = memberResp.Value;
            //var shoppingCartResp = await _marketObject.GetShoppingCartByUserId(TransactionPost.MemberId);
            var shoppingCartResp = await _shoppingCartService.GetShoppingCartByUserId(TransactionPost.MemberId);
            if (shoppingCartResp == null || shoppingCartResp.Value == null) { return Response<Transaction>.Error(HttpStatusCode.BadRequest, "No shopping cart found"); }
            ShoppingCart shoppingCart = shoppingCartResp.Value;
            if (shoppingCart.ShoppingBaskets.Count == 0) { return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Shopping cart is empty."); }



            var Transaction = new Transaction
            {
                MemberId = TransactionPost.MemberId,
                //Member_ =null,
                TransactionDate = TransactionPost.TransactionDate,
                TotalPrice = TransactionPost.TotalPrice,
                TransactionItems = new List<TransactionItem>(),
            };



            var TransactionBaskets = new Dictionary<long, List<TransactionItem>>();
            foreach (ShoppingBasket basket in shoppingCart.ShoppingBaskets)
            {

                TransactionBaskets.Add(basket.StoreId, new List<TransactionItem>());


                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    TransactionItem transactionItem = new TransactionItem(Transaction.Id, item);
                    TransactionBaskets[basket.StoreId].Add(transactionItem);
                }

                //var allAvailable= await _marketObject.CheckAvailabilityInStock(basket.StoreId, TransactionBaskets[basket.StoreId]);
                var allAvailable = await CheckAvailabilityInStock(basket.StoreId, TransactionBaskets[basket.StoreId]);
                if (!allAvailable.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, allAvailable.ErrorMessage);
                }

                //var allAvailable= await _marketObject.CheckTransactionPolicy(basket.StoreId, TransactionBaskets[basket.StoreId]);
                var compliesWithTransactionPolicy = await CheckTransactionPolicy(basket.StoreId, TransactionBaskets[basket.StoreId]);
                if (!compliesWithTransactionPolicy.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, compliesWithTransactionPolicy.ErrorMessage);
                }

                //var discountsApplied = await  _marketObject.ApplyDiscounts(basket.StoreId, TransactionBaskets[basket.StoreId], TransactionPost.MemberId);
                var discountsApplied = await ApplyDiscounts(basket.StoreId, TransactionBaskets[basket.StoreId], TransactionPost.MemberId);
                if (!discountsApplied.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, discountsApplied.ErrorMessage);
                }

            }
            Transaction.TotalPrice = TransactionBaskets.Aggregate(0.0, (total, nextBasket) => total + nextBasket.Value.Aggregate(0.0, (totalBasket, item) => totalBasket + item.FinalPrice * item.Quantity));

            TransactionPost.TotalPrice = Transaction.TotalPrice;


            //connction with external delivery service
            bool deliveryConfirmed = _deliveryProvider.ConfirmDelivery(new DeliveryDetails(TransactionPost, TransactionBaskets));
            if (!deliveryConfirmed)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Delivey declined");

            }
            //connction with external delivery service
            bool paymentConfirmed = _paymentProvider.ConfirmPayment(new PaymentDetails(TransactionPost));

            if (!paymentConfirmed)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Payment declined");

            }

            foreach (var (storeId, items) in TransactionBaskets)
            {
                Transaction.TransactionItems.AddRange(items);
                //var stockUpdated= await _marketObject.UpdateStock(storeId, items);
                var stockUpdated = await UpdateStock(storeId, items);
                if (!stockUpdated.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, stockUpdated.ErrorMessage);
                }
                //TODO: sendTransactionNotification(storeId);
            }


            //TODO: remove all baskets from shopping cart
            //await _marketObject.RemoveAllBaskets(member.Id);

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


        private bool TransactionExists(long id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Response<bool>> UpdateTransaction(long id, TransactionPost post)
        {

            var Transaction = await _context.Transactions.FindAsync(id);
            if (Transaction == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Transaction not found");
            }

            Transaction.TransactionDate = post.TransactionDate;
            Transaction.TotalPrice = post.TotalPrice;

            _context.Entry(Transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        private async Task<Response<bool>> CheckTransactionPolicy(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }

        private async Task<Response<bool>> ApplyDiscounts(long storeId, List<TransactionItem> TransactionItems, long memberId)
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


        private async Task<Response<bool>> CheckAvailabilityInStock(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            foreach (var TransactionItem in TransactionItems)
            {
                var product = await _context.Products.FindAsync(TransactionItem.ProductId);
                if (product == null) { return Response<bool>.Error(HttpStatusCode.NotFound, "Product does not exist"); }
                // if (product.UnitsInStock < TransactionItem.Quantity) { return Response<bool>.Error(HttpStatusCode.BadRequest, "Product's qunatity is unavailable in store"); }
            }

            return Response<bool>.Success(HttpStatusCode.NoContent, true);

        }


        private async Task<Response<bool>> UpdateStock(long storeId, List<TransactionItem> TransactionItems)
        {
            var shop = await _context.Stores.FindAsync(storeId);
            if (shop == null)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }

            foreach (var TransactionItem in TransactionItems)
            {
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
