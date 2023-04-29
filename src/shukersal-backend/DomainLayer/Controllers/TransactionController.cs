using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class TransactionController
    {
        private readonly MarketDbContext _context;

        public TransactionController(MarketDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<Transaction>>> GetTransactions()
        {
            if (_context.Transactions == null)
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.Transactions'  is null.");
            }
            var Transactions = await _context.Transactions
               // .Include(s => s.TransactionItems).Include(m=>m.Member_).ToListAsync();
               .Include(s => s.TransactionItems).ToListAsync();
            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, Transactions);
        }

        public async Task<Response<Transaction>> GetTransaction(long Transactionid)
        {
            if (_context.Transactions == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.Transactions'  is null.");
            }
            var Transaction = await _context.Transactions
                .Include(s => s.TransactionItems)
                //.Include(m=>m.Member_)
                .FirstOrDefaultAsync(s => s.Id == Transactionid);

            if (Transaction == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Not found");
            }
            return Response<Transaction>.Success(HttpStatusCode.OK, Transaction);
        }

        public async Task<Response<Transaction>> TransactionAShoppingCart(TransactionPost TransactionPost)
        {

            if (_context.Transactions == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.Transactions'  is null.");
            }
            if (_context.TransactionItems == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.TransactionItems'  is null.");
            }


            var Transaction = new Transaction
            {
                Member_Id_ = TransactionPost.Member__ID,
                //Member_ =null,
                TransactionDate = TransactionPost.TransactionDate,
                TotalPrice = TransactionPost.TotalPrice,
                TransactionItems = new List<TransactionItem>(),
            };

            var member = await _context.Members.FindAsync(TransactionPost.Member__ID);
            if (member == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Illegal user id");
            }
            //Transaction.Member_ = member;

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ShoppingBaskets)
                .ThenInclude(b => b.ShoppingItems)
                .FirstOrDefaultAsync(c => c.MemberId == TransactionPost.Member__ID);

            if (shoppingCart == null)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "No shopping cart.");
            }

            if (shoppingCart.ShoppingBaskets.Count == 0)
            {
                return Response<Transaction>.Error(HttpStatusCode.BadRequest, "Shopping cart is empty.");
            }

            var TransactionBaskets = new Dictionary<long, List<TransactionItem>>();
            foreach (ShoppingBasket basket in shoppingCart.ShoppingBaskets)
            {

                TransactionBaskets.Add(basket.StoreId, new List<TransactionItem>());

                foreach (ShoppingItem item in basket.ShoppingItems)
                {
                    var TransactionItem = new TransactionItem
                    {
                        TransactionId = Transaction.Id,
                        // Transaction= Transaction,
                        ProductId = item.ProductId,
                        StoreId = item.Product.StoreId,
                        ProductName = item.Product.Name,
                        ProductDescription = item.Product.Description,
                        Quantity = item.Quantity,
                        FullPrice = item.Product.Price,
                        FinalPrice = item.Product.Price,
                    };
                    TransactionBaskets[basket.StoreId].Add(TransactionItem);
                }


                var allAvailable = await CheckAvailabilityInStock(basket.StoreId, TransactionBaskets[basket.StoreId]);
                if (!allAvailable.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, allAvailable.ErrorMessage);
                }

                var compliesWithTransactionPolicy = await CheckTransactionPolicy(basket.StoreId, TransactionBaskets[basket.StoreId]);
                if (!compliesWithTransactionPolicy.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, compliesWithTransactionPolicy.ErrorMessage);
                }

                var discountsApplied = await ApplyDiscounts(basket.StoreId, TransactionBaskets[basket.StoreId], TransactionPost.Member__ID);
                if (!discountsApplied.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, discountsApplied.ErrorMessage);
                }

            }
            Transaction.TotalPrice = TransactionBaskets.Aggregate(0.0, (total, nextBasket) => total + nextBasket.Value.Aggregate(0.0, (totalBasket, item) => totalBasket + item.FinalPrice * item.Quantity));


            //connction with external delivery service
            //TODO: confirmDelivery(TransactionItems,~delivery details~);

            //connction with external delivery service
            //TODO: confirmPayment(amount,~payment details~);


            foreach (var (storeId, items) in TransactionBaskets)
            {
                Transaction.TransactionItems.AddRange(items);
                var stockUpdated = await UpdateStock(storeId, items);
                if (!stockUpdated.Result)
                {
                    return Response<Transaction>.Error(HttpStatusCode.BadRequest, stockUpdated.ErrorMessage);
                }
                //TODO: sendTransactionNotification(storeId);
            }


            //TODO: remove all baskets from shopping cart


            _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();
            return Response<Transaction>.Success(HttpStatusCode.Created, Transaction);
        }


        public async Task<Response<IEnumerable<Transaction>>> BroweseTransactionHistory(long memberId)
        {
            if (_context.Transactions == null)
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.Transactions'  is null.");
            }

            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.BadRequest, "Illegal user id");
            }

            var Transactions = await _context.Transactions
              //  .Include(s => s.TransactionItems).Include(m=>m.Member_).Where(s=>s.Member_Id_ ==memberId).ToListAsync();
              .Include(s => s.TransactionItems).Where(s => s.Member_Id_ == memberId).ToListAsync();
            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, Transactions);
        }

        public async Task<Response<IEnumerable<Transaction>>> BroweseShopTransactionHistory(long shopId)
        {
            if (_context.Transactions == null)
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.NotFound, "Entity set 'TransactionContext.Transactions'  is null.");
            }

            var shop = await _context.Stores.FindAsync(shopId);
            if (shop == null)
            {
                return Response<IEnumerable<Transaction>>.Error(HttpStatusCode.BadRequest, "Illegal shop id");
            }


            var Transactions = await _context.Transactions
                //.Include(s => s.TransactionItems).Include(m=>m.Member_).Where(p=> p.TransactionItems.Any(i => i.StoreId == shopId)).ToListAsync();
                .Include(s => s.TransactionItems).Where(p => p.TransactionItems.Any(i => i.StoreId == shopId)).ToListAsync();

            var TransactionHistory = new List<Transaction>();
            foreach (var Transaction in Transactions)
            {
                Transaction p = new Transaction
                {
                    Id = Transaction.Id,
                    Member_Id_ = Transaction.Member_Id_,
                    // Member_ = Transaction.Member_,
                    TransactionDate = Transaction.TransactionDate,
                    TotalPrice = Transaction.TotalPrice,
                    TransactionItems = Transaction.TransactionItems.Where(i => i.StoreId == shopId).ToList(),
                };
                TransactionHistory.Add(p);
            }
            return Response<IEnumerable<Transaction>>.Success(HttpStatusCode.OK, TransactionHistory);
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
