using Microsoft.EntityFrameworkCore;
using System.Net;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using shukersal_backend.Models.StoreModels;

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
            _context.PurchaseRules.Add(new PurchaseRule
            {
                Id = post.Id,
                store = s,
                purchaseRuleType = post.purchaseRuleType,
                Components = componenets,
                conditionString = post.conditionString,
                conditionLimit =    post.conditionLimit,
                minHour = post.minHour,
                maxHour = post.maxHour,
                
            });
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
                    Id = post.Id,
                    store = composite.store,
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
        public bool Evaluate(PurchaseRule purchaseRule, ICollection<ShoppingItem> items)
        {
            if (purchaseRule.purchaseRuleType == PurchaseRuleType.AND && purchaseRule.Components != null)
                return purchaseRule.Components.Select(cm => Evaluate(cm, items)).All(res => res);

            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.OR && purchaseRule.Components != null)
                return purchaseRule.Components.Select(cm => Evaluate(cm, items)).Any(res => res);

            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.CONDITION && purchaseRule.Components != null)
                return purchaseRule.Components.Count == 2 &&
                    (!Evaluate(purchaseRule.Components.ElementAt(0), items)
                    || Evaluate(purchaseRule.Components.ElementAt(1), items));

            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.PRODUCT_AT_LEAST)
                return items.Where(
                    i => i.Product.Name == purchaseRule.conditionString &&
                    i.Quantity >= purchaseRule.conditionLimit).Count() > 0;

            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.PRODUCT_LIMIT)
                return items.Where(
                    i => i.Product.Name == purchaseRule.conditionString &&
                    i.Quantity > purchaseRule.conditionLimit).Count() == 0;

            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.CATEGORY_AT_LEAST)
                return items.Where(
                    i => i.Product.Category.Name == purchaseRule.conditionString)
                    .Sum(i => i.Quantity) >= purchaseRule.conditionLimit;

            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.CATEGORY_LIMIT)
                return items.Where(
                    i => i.Product.Category.Name == purchaseRule.conditionString)
                    .Sum(i => i.Quantity) <= purchaseRule.conditionLimit;
            else if (purchaseRule.purchaseRuleType == PurchaseRuleType.TIME_HOUR_AT_DAY)
                return purchaseRule.minHour <= DateTime.Now.Hour && DateTime.Now.Hour < purchaseRule.maxHour;

            //else if (purchaseRule.purchaseRuleType == PurchaseRuleType.TIME_DAY_AT_WEEK)
            //    return purchaseRule.weekDays[(int)DateTime.Now.DayOfWeek];
            return true;
        }

        public async Task<Response<ICollection<PurchaseRule>>> GetPurchaseRules(long storeId)
        {
            var purchaseRules = await _context.PurchaseRules
                .Where(dr => dr.store.Id == storeId)
                .ToListAsync();
            return Response<ICollection<PurchaseRule>>.Success(HttpStatusCode.OK, purchaseRules);
        }
    }
}
