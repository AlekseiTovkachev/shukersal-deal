using Microsoft.EntityFrameworkCore;
using System.Net;
using shukersal_backend.Models;
using shukersal_backend.Utility;


namespace shukersal_backend.DomainLayer.Objects
{
    public class DiscountObject
    {
        private MarketDbContext _context;
        private PurchaseRuleObject _purchaseRuleObject;

        public DiscountObject(MarketDbContext context)
        {
            _context = context;
            _purchaseRuleObject = new PurchaseRuleObject(context);
        }
        public async Task<Response<bool>> CreateDiscount(DiscountRulePost post, Store s)
        {
            ICollection<DiscountRule>? componenets = null;
            if (post.discountType == DiscountType.ADDITIONAL || post.discountType == DiscountType.MAX)
                componenets= new List<DiscountRule>();
            _context.DiscountRules.Add(new DiscountRule {
                Id = post.Id,
                store = s,
                Discount = post.Discount,
                discountType = post.discountType,
                Components = componenets,
                discountRuleBoolean = post.discountRuleBoolean,
                discountOn = post.discountOn,
                discountOnString = post.discountOnString
            });
            await _context.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.OK, true);
        }
        public async Task<Response<bool>> CreateChildDiscount(long compositeId, DiscountRulePost post)
        {
            var composite = await _context.DiscountRules.FirstOrDefaultAsync(dr => dr.Id == compositeId);
            if (composite != null && (composite.discountType == DiscountType.ADDITIONAL || composite.discountType == DiscountType.MAX))
            {
                ICollection<DiscountRule>? componenets = null;
                if (post.discountType == DiscountType.ADDITIONAL || post.discountType == DiscountType.MAX)
                    componenets = new List<DiscountRule>();
                var component = new DiscountRule
                {
                    Id = post.Id,
                    store = composite.store,
                    Discount = post.Discount,
                    discountType = post.discountType,
                    Components = componenets,
                    discountRuleBoolean = post.discountRuleBoolean,
                    discountOn = post.discountOn,
                    discountOnString = post.discountOnString
                };
                _context.DiscountRules.Add(component);
                composite.Components?.Add(component);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(HttpStatusCode.OK, true);
            }
            return Response<bool>.Success(HttpStatusCode.NotFound, false);
        }
        public double CalculateDiscount(DiscountRule discountRule, ICollection<ShoppingItem> items)
        {
            if (discountRule == null)
                return 0;
            else if (discountRule.discountType == DiscountType.SIMPLE)
                return calculateDiscountOnProducts(discountRule, items);

            else if (discountRule.discountType == DiscountType.CONDITIONAL &&
                 discountRule.discountRuleBoolean != null &&
                 Evaluate(discountRule.discountRuleBoolean, items))
                return calculateDiscountOnProducts(discountRule, items);

            else if (discountRule.discountType == DiscountType.MAX && discountRule.Components != null)
                return discountRule.Components.
                    Select(cm => CalculateDiscount(cm, items)).
                    DefaultIfEmpty(0).
                    Max();

            else if (discountRule.discountType == DiscountType.ADDITIONAL && discountRule.Components != null)
                return discountRule.Components.
                    Select(cm => CalculateDiscount(cm, items)).
                    DefaultIfEmpty(0).
                    Sum();

            return 0;
        }

        private double calculateDiscountOnProducts(DiscountRule discountRule, ICollection<ShoppingItem> items)
        {
            if (discountRule.discountOn == DiscountOn.STORE)
                return items.Select(i => i.Product.Price)
                    .Sum() * discountRule.Discount / 100;

            else if (discountRule.discountOn == DiscountOn.CATEGORY)
                return items.Where(i => i.Product.Category.Name == discountRule.discountOnString)
                    .Select(i => i.Product.Price)
                    .Sum() * discountRule.Discount / 100;

            else if (discountRule.discountOn == DiscountOn.PRODUCT)
                return items.Where(i => i.Product.Name == discountRule.discountOnString)
                    .Select(i => i.Product.Price)
                    .Sum() * discountRule.Discount / 100;
            return 0;
        }
        public async Task<Response<ICollection<DiscountRule>>> GetDiscounts(long storeId)
        {
            var discounts = await _context.DiscountRules
                .Where(dr => dr.store.Id == storeId)
                .ToListAsync();
            return Response<ICollection<DiscountRule>>.Success(HttpStatusCode.OK, discounts);
        }






        public async Task<Response<bool>> CreateDiscountRuleBoolean(DiscountRuleBoolean post, Store s)
        {
            ICollection<DiscountRuleBoolean>? componenets = null;
            if (post.discountRuleBooleanType == DiscountRuleBooleanType.OR || post.discountRuleBooleanType == DiscountRuleBooleanType.AND || post.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION)
                componenets = new List<DiscountRuleBoolean>();
            _context.DiscountRuleBooleans.Add(new DiscountRuleBoolean
            {
                Id = post.Id,
                store = s,
                discountRuleBooleanType = post.discountRuleBooleanType,
                Components = componenets,
                conditionString = post.conditionString,
                conditionLimit = post.conditionLimit,
                minHour = post.minHour,
                maxHour = post.maxHour,

            });
            await _context.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.OK, true);
        }
        public async Task<Response<bool>> CreateChildDiscountRuleBoolean(long compositeId, DiscountRuleBoolean post)
        {
            var composite = await _context.DiscountRuleBooleans.FirstOrDefaultAsync(dr => dr.Id == compositeId);
            if (composite != null && (composite.discountRuleBooleanType == DiscountRuleBooleanType.OR || composite.discountRuleBooleanType == DiscountRuleBooleanType.AND || composite.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION))
            {
                ICollection<DiscountRuleBoolean>? componenets = null;
                if (post.discountRuleBooleanType == DiscountRuleBooleanType.OR || post.discountRuleBooleanType == DiscountRuleBooleanType.AND || post.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION)
                    componenets = new List<DiscountRuleBoolean>();
                var component = new DiscountRuleBoolean
                {
                    Id = post.Id,
                    store = composite.store,
                    discountRuleBooleanType = post.discountRuleBooleanType,
                    Components = componenets,
                    conditionString = post.conditionString,
                    conditionLimit = post.conditionLimit,
                    minHour = post.minHour,
                    maxHour = post.maxHour,
                };
                _context.DiscountRuleBooleans.Add(component);
                composite.Components?.Add(component);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(HttpStatusCode.OK, true);
            }
            return Response<bool>.Success(HttpStatusCode.NotFound, false);
        }
        public bool Evaluate(DiscountRuleBoolean purchaseRule, ICollection<ShoppingItem> items)
        {
            if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.AND && purchaseRule.Components != null)
                return purchaseRule.Components.Select(cm => Evaluate(cm, items)).All(res => res);

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.OR && purchaseRule.Components != null)
                return purchaseRule.Components.Select(cm => Evaluate(cm, items)).Any(res => res);

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION && purchaseRule.Components != null)
                return purchaseRule.Components.Count == 2 &&
                    (!Evaluate(purchaseRule.Components.ElementAt(0), items)
                    || Evaluate(purchaseRule.Components.ElementAt(1), items));

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_AT_LEAST)
                return items.Where(
                    i => i.Product.Name == purchaseRule.conditionString &&
                    i.Quantity >= purchaseRule.conditionLimit).Count() > 0;

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_LIMIT)
                return items.Where(
                    i => i.Product.Name == purchaseRule.conditionString &&
                    i.Quantity > purchaseRule.conditionLimit).Count() == 0;

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.CATEGORY_AT_LEAST)
                return items.Where(
                    i => i.Product.Category.Name == purchaseRule.conditionString)
                    .Sum(i => i.Quantity) >= purchaseRule.conditionLimit;

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.CATEGORY_LIMIT)
                return items.Where(
                    i => i.Product.Category.Name == purchaseRule.conditionString)
                    .Sum(i => i.Quantity) <= purchaseRule.conditionLimit;
            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.TIME_HOUR_AT_DAY)
                return purchaseRule.minHour <= DateTime.Now.Hour && DateTime.Now.Hour < purchaseRule.maxHour;

            //else if (purchaseRule.purchaseRuleType == PurchaseRuleType.TIME_DAY_AT_WEEK)
            //    return purchaseRule.weekDays[(int)DateTime.Now.DayOfWeek];
            return true;
        }

        public async Task<Response<ICollection<PurchaseRule>>> GetDiscountRuleBooleans(long storeId)
        {
            var purchaseRules = await _context.PurchaseRules
                .Where(dr => dr.store.Id == storeId)
                .ToListAsync();
            return Response<ICollection<PurchaseRule>>.Success(HttpStatusCode.OK, purchaseRules);
        }
    }
}
