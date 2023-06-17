using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using shukersal_backend.DomainLayer.notifications;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.ExternalServices.ExternalDeliveryService;
using shukersal_backend.ExternalServices.ExternalPaymentService;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
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
        private readonly StoreManagerObject _managerObject;
        private ShoppingCartObject _shoppingCartObject;
        private readonly NotificationController _notificationController;

        public TransactionController(MarketDbContext context, NotificationController notificationController)
        {
            _context = context;
            _marketObject = new MarketObject(context);
            _notificationController = notificationController;
            _memberObject = new MemberObject(context);
            _transactionObject = new TransactionObject(context,_marketObject);
            _managerObject = new StoreManagerObject(_context);
            _storeObject = new StoreObject(context, _marketObject, _managerObject);
            _shoppingCartObject = null;
            _marketObject.SetDeliveryProvider("https://php-server-try.000webhostapp.com/");
            _marketObject.SetPaymentProvider("https://php-server-try.000webhostapp.com/");
        }

        //for testings
        public TransactionController(MarketDbContext context, MarketObject marketObject,TransactionObject transactionObject, StoreManagerObject managerObject, MemberObject memberObject, StoreObject storeObject)
        {
            _context = context;
            _marketObject = marketObject;
            _memberObject = memberObject;
            _transactionObject = transactionObject;
            _managerObject = managerObject;
            _storeObject = storeObject;
            _marketObject.SetDeliveryProvider("https://php-server-try.000webhostapp.com/");
            _marketObject.SetPaymentProvider("https://php-server-try.000webhostapp.com/");
            _shoppingCartObject = null;

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

        public async Task<Response<Transaction>> GetTransaction(long Transactionid,long loggedInMemId)
        {
            var Transaction = await _context.Transactions
                .Include(s => s.TransactionItems)
                .FirstOrDefaultAsync(s => s.Id == Transactionid);
            var member=await _memberObject.GetMember(loggedInMemId);
            if (Transaction == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Not found");
            }

            if(!member.IsSuccess || member.Result == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Member not found");
            }
            if (Transaction.MemberId!= loggedInMemId && member.Result.Role!= "Administrator")
            {
                return Response<Transaction>.Error(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            return Response<Transaction>.Success(HttpStatusCode.OK, Transaction);
        }


        private async Task<Response<Dictionary<long, List<TransactionItem>>>> PrepareTransactionBaskets(Transaction transaction,List<ShoppingBasketObject> basketObjects)
        {
            Dictionary<long, List<TransactionItem>> TransactionBaskets = new Dictionary<long, List<TransactionItem>>();
            foreach (var shoppingBasket in basketObjects)
            {
                foreach (var item in shoppingBasket.ShoppingItems)
                {
                    if (!TransactionBaskets.ContainsKey(shoppingBasket.StoreId))
                    {
                        TransactionBaskets.Add(shoppingBasket.StoreId, new List<TransactionItem>());
                    }

                    var productResp = await _storeObject.GetProduct(item.ProductId);
                    if (!productResp.IsSuccess || productResp.Result == null)
                    {
                        return Response<Dictionary<long, List<TransactionItem>>>.Error(HttpStatusCode.NotFound, "Product does not exist");
                    }
                    var product = productResp.Result;

                    var transactionItem = new TransactionItem
                    {
                        TransactionId = transaction.Id,
                        ProductId = item.ProductId,
                        StoreId = shoppingBasket.StoreId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        Quantity = item.Quantity,
                        FullPrice = product.Price,
                    };

                    await _context.TransactionItems.AddAsync(transactionItem);
                    TransactionBaskets[shoppingBasket.StoreId].Add(transactionItem);
                    transaction.TransactionItems.Add(transactionItem);
                }

            }
            return Response<Dictionary<long, List<TransactionItem>>>.Success(HttpStatusCode.OK,TransactionBaskets);
        }

        private async Task<Response<Dictionary<long, List<TransactionItem>>>> PrepareTransactionBaskets(Transaction transaction, ICollection<TransactionItemPost> items)
        {
            Dictionary<long, List<TransactionItem>> TransactionBaskets = new Dictionary<long, List<TransactionItem>>();
            foreach (var item in items)
            {
                if (!TransactionBaskets.ContainsKey(item.StoreId))
                {
                    TransactionBaskets.Add(item.StoreId, new List<TransactionItem>());
                }

                var productResp = await _storeObject.GetProduct(item.ProductId);
                if (!productResp.IsSuccess || productResp.Result == null)
                {
                    return Response<Dictionary<long, List<TransactionItem>>>.Error(HttpStatusCode.NotFound, "Product does not exist");
                }
                var product = productResp.Result;

                var transactionItem = new TransactionItem
                {
                    TransactionId = transaction.Id,
                    ProductId = item.ProductId,
                    StoreId = item.StoreId,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    Quantity = item.Quantity,
                    FullPrice = product.Price,
                };

                await _context.TransactionItems.AddAsync(transactionItem);
                TransactionBaskets[item.StoreId].Add(transactionItem);
                transaction.TransactionItems.Add(transactionItem);
            }
            return Response<Dictionary<long, List<TransactionItem>>>.Success(HttpStatusCode.OK, TransactionBaskets);
        }



        private async Task<Response<Transaction>> ProcessTransaction(Transaction transaction, Dictionary<long, List<TransactionItem>> TransactionBaskets, DeliveryDetails deliveryDetails, PaymentDetails paymentDetails)
        {
            var allValid = await CheckRulesAndApplyDiscounts(TransactionBaskets, transaction.MemberId);
            if (!allValid.IsSuccess || allValid.Result == null) { return Response<Transaction>.Error(HttpStatusCode.BadRequest, allValid.ErrorMessage); }
            transaction.TotalPrice = TransactionBaskets.Aggregate(0.0, (total, nextBasket) => total + nextBasket.Value.Aggregate(0.0, (totalBasket, item) => totalBasket + item.FinalPrice * item.Quantity));
            var gotExternalServicesConfirmation = await _marketObject.confirmDeliveryAndPayment(deliveryDetails, transaction.TransactionItems.ToList(), paymentDetails);
            if (!gotExternalServicesConfirmation.IsSuccess)
            {
                await Rollback(transaction.TransactionItems);
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, gotExternalServicesConfirmation.ErrorMessage);
            }

            foreach (var (storeId, items) in TransactionBaskets)
            {
                //TODO: sendTransactionNotification(storeId);
            }
            
            return Response<Transaction>.Success(HttpStatusCode.OK, transaction);
        }

        public async Task<Response<Transaction>> CreateMemberTransaction(TransactionPost transactionPost)
        {
            var member = _context.Members.Where(m => m.Id == transactionPost.MemberId).FirstOrDefault();

            if (member == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Couldn't find member");
            }

            var cartRes =await _marketObject.GetShoppingCartByUserId(member.Id);
            if (!cartRes.IsSuccess || cartRes.Result == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Cart was not found");
            }
            ShoppingCartObject cart = new ShoppingCartObject(_context, cartRes.Result);
            if (cart.ShoppingBaskets.IsNullOrEmpty())
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Shopping cart is empty!");
            }

            var transaction = new Transaction
            {
                IsMember = transactionPost.IsMember,
                MemberId = transactionPost.MemberId,
                TransactionDate = transactionPost.TransactionDate,
                TransactionItems = new List<TransactionItem>(),
            };

            await _context.Transactions.AddAsync(transaction);

            var TransactionBasketsResp = await PrepareTransactionBaskets(transaction, cart.ShoppingBaskets);
            if(!TransactionBasketsResp.IsSuccess || TransactionBasketsResp.Result == null)
            {
                _context.Transactions.Remove(transaction);
                return Response<Transaction>.Error(TransactionBasketsResp.StatusCode, TransactionBasketsResp.ErrorMessage);
            }
            var TransactionBaskets = TransactionBasketsResp.Result;

            var transactionProcessed = await ProcessTransaction(transaction, TransactionBaskets, transactionPost.DeliveryDetails, transactionPost.BillingDetails);
            if(!transactionProcessed.IsSuccess || transactionProcessed.Result==null) 
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return Response<Transaction>.Error(transactionProcessed.StatusCode, transactionProcessed.ErrorMessage);
            }
            
            await _marketObject.EmptyCart(member.Id);
            await _context.SaveChangesAsync();
            return Response<Transaction>.Success(HttpStatusCode.Created, transaction);
        }

        public async Task<Response<Transaction>> CreateGuestTransaction(TransactionPost transactionPost)
        {
            if (transactionPost.TransactionItems.IsNullOrEmpty())
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Shopping cart is empty!!");
            }

            var transaction = new Transaction
            {
                IsMember = transactionPost.IsMember,
                MemberId = transactionPost.MemberId,
                TransactionDate = transactionPost.TransactionDate,
                TransactionItems = new List<TransactionItem>(),
            };

            await _context.Transactions.AddAsync(transaction);

            var TransactionBasketsResp = await PrepareTransactionBaskets(transaction, transactionPost.TransactionItems);
            if (!TransactionBasketsResp.IsSuccess || TransactionBasketsResp.Result == null)
            {
                _context.Transactions.Remove(transaction);
                return Response<Transaction>.Error(TransactionBasketsResp.StatusCode, TransactionBasketsResp.ErrorMessage);
            }
            var TransactionBaskets = TransactionBasketsResp.Result;

            var transactionProcessed = await ProcessTransaction(transaction, TransactionBaskets, transactionPost.DeliveryDetails, transactionPost.BillingDetails);
            if (!transactionProcessed.IsSuccess || transactionProcessed.Result == null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return Response<Transaction>.Error(transactionProcessed.StatusCode, transactionProcessed.ErrorMessage);
            }

            await _context.SaveChangesAsync();
            return Response<Transaction>.Success(HttpStatusCode.Created, transaction);

        }

        //TODO: MAY NEED TO CHANGE THE RETURN VALUE
        public async Task<Response<Transaction>> PurchaseAShoppingCart(TransactionPost transactionPost)
        {
            Response<Transaction> response;
            if (transactionPost.IsMember)
            {
                response = await CreateMemberTransaction(transactionPost);
            }
            else
            {
                response = await CreateGuestTransaction(transactionPost);
            }

            if(response.IsSuccess)
            {
                SendTransactionNotification(response.Result);
            }
            return response;
        }
        //TODO: im here
        private async Task<Response<string>> SendTransactionNotification(Transaction transaction)
        {
            // Get transaction items related to the transaction
            var transactionItems = await _context.TransactionItems
                .Where(ti => ti.TransactionId == transaction.Id)
                .ToListAsync();

            // Get store ids from transaction items
            var storeIds = transactionItems.Select(ti => ti.StoreId).Distinct();

            // Get store managers related to the store ids
            var storeManagers = await _context.Set<StoreManager>()
                .Where(m => storeIds.Contains(m.StoreId))
                .ToListAsync();

            // Prepare a list to store pairs of store manager's memberId and transaction items
            List<Tuple<long, string>> storeManagerTransactionItemsPairs = new List<Tuple<long, string>>();

            // Loop through each store manager
            foreach (var storeManager in storeManagers)
            {
                // Find transaction items related to the store manager's store
                var items = transactionItems
                    .Where(ti => ti.StoreId == storeManager.StoreId)
                    .Select(ti => ti.ProductName);

                // Create a string containing all the product names for the store manager
                var productNames = "Items: " + string.Join(", ", items);

                // Add to the list as a pair
                storeManagerTransactionItemsPairs.Add(Tuple.Create(storeManager.MemberId, productNames));
            }
            var response = _notificationController.SendBulkNotifications(storeManagerTransactionItemsPairs, NotificationType.ProductPurchased);
            return response.Result;
        }



        private async Task Rollback(ICollection<TransactionItem> transactionItems)
        {
            foreach (TransactionItem item in transactionItems)
            {
                var productToRollBack = await _context.Products.FindAsync(item.ProductId); ;
                if (productToRollBack != null)
                {
                    productToRollBack.UnitsInStock = productToRollBack.UnitsInStock + item.Quantity;
                    await _context.SaveChangesAsync();
                }
            }

        } 


        public async Task<Response<IEnumerable<Transaction>>> BrowseTransactionHistory(long memberId, long loggedInMemId)
        {

            var member = await _memberObject.GetMember(loggedInMemId);

            if (!member.IsSuccess || member.Result == null)
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.NotFound, "Member not found");
            }
            if (memberId != loggedInMemId && member.Result.Role != "Administrator")
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            var Transactions = await _context.Transactions
            .Include(s => s.TransactionItems).Where(s => s.MemberId == memberId).ToListAsync();


            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, Transactions);
        }

        public async Task<Response<IEnumerable<Transaction>>> BrowseShopTransactionHistory(long shopId, long loggedInMemId)
        {
            var storeResp = await _marketObject.GetStore(shopId);

            if (storeResp.Result == null) {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.BadRequest,storeResp.ErrorMessage);
            }

            //var _managerObject = new StoreManagerObject(_context);
            var hasPermission = await _managerObject.CheckPermission(shopId, loggedInMemId, PermissionType.Get_history_permission);
            var member = (await _memberObject.GetMember(loggedInMemId)).Result;
            if (!hasPermission && (member == null || member.Role != "Administrator"))
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.Unauthorized, "Unauthorized");
            }

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
            await d.CalculateDiscount((await d.GetAppliedDiscount(storeId)).Result, TransactionItems);
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
