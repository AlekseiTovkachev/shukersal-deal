using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Objects
{
    public class TransactionObject
    {
        private MarketDbContext _context;
        private MarketObject _market;

        public TransactionObject(MarketDbContext context, MarketObject market)
        {
            _context = context;
            _market = market;
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
                if (!(_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault())
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

        public async Task<Response<IEnumerable<TransactionItem>>> GetTransactionItems(long TransactionId)
        {
            var transactionExist = _market.TransactionExists(TransactionId);


            if (!transactionExist)
            {
                return Response<IEnumerable<TransactionItem>>.Error(HttpStatusCode.NotFound,"Transaction was not found.");
            }

            var transactionItems = await _context.TransactionItems
                .Where(i => i.TransactionId == TransactionId)
                .ToListAsync();

            return Response<IEnumerable<TransactionItem>>.Success(HttpStatusCode.OK, transactionItems);
        }




    }
}
