using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Models.StoreModels;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Objects
{
    public class PurchaseRuleObject
    {
        private MarketDbContext _context;

        public PurchaseRuleObject(MarketDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> CreatePurchaseRule(PurchaseRulePost post, Store s)
        {
            ICollection<PurchaseRule>? componenets = null;
            if (post.purchaseRuleType == PurchaseRuleType.OR || post.purchaseRuleType == PurchaseRuleType.AND || post.purchaseRuleType == PurchaseRuleType.CONDITION)
                componenets = new List<PurchaseRule>();
            var pr = new PurchaseRule
            {
                //Id = post.Id,
                purchaseRuleType = post.purchaseRuleType,
                Components = componenets,
                conditionString = post.conditionString,
                conditionLimit = post.conditionLimit,
                minHour = post.minHour,
                maxHour = post.maxHour,

            };
            s.PurchaseRules.Add(pr);
            _context.PurchaseRules.Add(pr);
            await _context.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.OK, true);
        }
        public async Task<Response<bool>> CreateChildPurchaseRule(long compositeId, PurchaseRulePost post)
        {
            var composite = await _context.PurchaseRules.FirstOrDefaultAsync(dr => dr.Id == compositeId);
            if (composite != null && (composite.purchaseRuleType == PurchaseRuleType.OR || composite.purchaseRuleType == PurchaseRuleType.AND || composite.purchaseRuleType == PurchaseRuleType.CONDITION))
            {
                ICollection<PurchaseRule>? componenets = null;
                if (post.purchaseRuleType == PurchaseRuleType.OR || post.purchaseRuleType == PurchaseRuleType.AND || post.purchaseRuleType == PurchaseRuleType.CONDITION)
                    componenets = new List<PurchaseRule>();
                var component = new PurchaseRule
                {
                    //Id = post.Id,
                    purchaseRuleType = post.purchaseRuleType,
                    Components = componenets,
                    conditionString = post.conditionString,
                    conditionLimit = post.conditionLimit,
                    minHour = post.minHour,
                    maxHour = post.maxHour,
                };
                _context.PurchaseRules.Add(component);
                composite.Components?.Add(component);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(HttpStatusCode.OK, true);
            }
            return Response<bool>.Success(HttpStatusCode.NotFound, false);
        }
        public bool Evaluate(PurchaseRule purchaseRule, ICollection<TransactionItem> items)
        {
            return PurchaseRuleComponent.Build(purchaseRule, _context).Eval(items);
        }

        public async Task<Response<ICollection<PurchaseRule>>> GetPurchaseRules(long storeId)
        {
            var store = await _context.Stores.Where(s => s.Id == storeId)
                .Include(s => s.PurchaseRules)
                    .ThenInclude(pr => pr.Components)
                .FirstOrDefaultAsync();
            if (store != null)
                return Response<ICollection<PurchaseRule>>.Success(HttpStatusCode.OK, store.PurchaseRules);
            return Response<ICollection<PurchaseRule>>.Error(HttpStatusCode.NotFound, "store doesnt exist");
        }

        public async Task<Response<PurchaseRule>> GetAppliedPurchaseRule(long storeId)
        {
            var store = await _context.Stores.Where(s => s.Id == storeId)
                .Include(s => s.AppliedPurchaseRule)
                    .ThenInclude(pr => pr.Components)
                .FirstOrDefaultAsync();
            if (store != null && store.AppliedPurchaseRule != null)
                return Response<PurchaseRule>.Success(HttpStatusCode.OK, store.AppliedPurchaseRule);
            return Response<PurchaseRule>.Error(HttpStatusCode.NotFound, "store doesnt exist");
        }

        public async Task<Response<PurchaseRule>> SelectPurchaseRule(Store s, long PurchaseRuleId)
        {
            var r = s.PurchaseRules.Where(dr => dr.Id == PurchaseRuleId);
            if (r == null || r.Count() == 0)
                return Response<PurchaseRule>.Error(HttpStatusCode.NotFound, "purchase rule doesnt exist");
            s.AppliedPurchaseRule = r.FirstOrDefault();
            await _context.SaveChangesAsync();
            return Response<PurchaseRule>.Success(HttpStatusCode.OK, r.FirstOrDefault());
        }
    }
}