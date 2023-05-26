using Microsoft.EntityFrameworkCore;
using System.Net;
using shukersal_backend.Models;
using shukersal_backend.Utility;


namespace shukersal_backend.DomainLayer.Objects
{
    public class DiscountObject
    {
        private MarketDbContext _context;

        public DiscountObject(MarketDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> CreateDiscount(DiscountRulePost post, Store s)
        {
            ICollection<DiscountRule>? componenets = null;
            if (post.discountType == DiscountType.ADDITIONAL || post.discountType == DiscountType.MAX)
                componenets = new List<DiscountRule>();
            var dr = new DiscountRule
            {
                Id = post.Id,
                Discount = post.Discount,
                discountType = post.discountType,
                Components = componenets,
                discountRuleBoolean = post.discountRuleBoolean,
                discountOn = post.discountOn,
                discountOnString = post.discountOnString
            };
            _context.DiscountRules.Add(dr);
            s.DiscountRules.Add(dr);
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
        public async Task<double> CalculateDiscount(DiscountRule discountRule, ICollection<TransactionItem> transactionItems)
        {
            var res = calculateDiscount(discountRule, transactionItems);
            foreach (var r in res)
                r.transcationItem.FinalPrice = r.transcationItem.FullPrice - r.discount;
            await _context.SaveChangesAsync();
            return res.Select(r => r.transcationItem.FullPrice).Sum();

        }
        private IEnumerable<TranscationCalculation> calculateDiscount(DiscountRule discountRule, ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            if (discountRule == null)
            {
                //do nothing
            }
            else if (discountRule.discountType == DiscountType.SIMPLE)
                return calculateDiscountOnProducts(discountRule, items);

            else if (discountRule.discountType == DiscountType.CONDITIONAL &&
                 discountRule.discountRuleBoolean != null &&
                 Evaluate(discountRule.discountRuleBoolean, items))
                return calculateDiscountOnProducts(discountRule, items);

            else if (discountRule.discountType == DiscountType.MAX && discountRule.Components != null)
            {
                var tcList = discountRule.Components.Select(cm => calculateDiscount(cm, items));
                return tcList.Where(tc => tc.Select(i => i.discount).Sum()
                     == tcList.Select(
                        t => t.Select(i => i.discount).Sum())
                    .Max()).FirstOrDefault();

            }

            else if (discountRule.discountType == DiscountType.ADDITIONAL && discountRule.Components != null)
            {
                var tcList = discountRule.Components.Select(cm => calculateDiscount(cm, items));
                return transcations.Select(ta => new TranscationCalculation
                {
                    transcationItem = ta.transcationItem,
                    discount = tcList.Select(
                        t => t.Where(i => i.transcationItem == ta.transcationItem).FirstOrDefault().discount)
                    .Sum()
                });
            }
            return transcations;
        }

        private IEnumerable<TranscationCalculation> calculateDiscountOnProducts(DiscountRule discountRule, ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            if (discountRule.discountOn == DiscountOn.STORE)
                return transcations.Select(t => new TranscationCalculation {
                    transcationItem = t.transcationItem,
                    discount = t.transcationItem.FullPrice * discountRule.Discount / 100
                });


            else if (discountRule.discountOn == DiscountOn.CATEGORY)
                return transcations.Select(t => new TranscationCalculation
                {
                    transcationItem = t.transcationItem,
                    discount = t.transcationItem.FullPrice * discountRule.Discount / 100
                });

            else if (discountRule.discountOn == DiscountOn.PRODUCT)
                return transcations.Select(t =>
                    t.transcationItem.ProductName == discountRule.discountOnString ?
                        new TranscationCalculation
                        {
                            transcationItem = t.transcationItem,
                            discount = t.transcationItem.FullPrice * discountRule.Discount / 100
                        }
                    : t
                );
            return transcations;
        }
        /*public async Task<Response<ICollection<DiscountRule>>> GetDiscounts(long storeId)
        {
            
            var discounts = await _context.DiscountRules
                .Where(dr => dr.store.Id == storeId)
                .ToListAsync();
            return Response<ICollection<DiscountRule>>.Success(HttpStatusCode.OK, discounts);
        }*/

        public async Task<Response<DiscountRule>> SelectDiscount(Store s, long DiscountRuleId)
        {
            var r = s.DiscountRules.Where(dr => dr.Id == DiscountRuleId);
            if (r == null || r.Count() == 0)
                return Response<DiscountRule>.Error(HttpStatusCode.NotFound,"discount doesnt exist");
            s.AppliedDiscountRule = r.FirstOrDefault();
            await _context.SaveChangesAsync();
            return Response<DiscountRule>.Success(HttpStatusCode.OK, r.FirstOrDefault());
        }
        public async Task<Response<bool>> CreateDiscountRuleBoolean(DiscountRuleBoolean post, Store s, DiscountRule discountRule)
        {
            ICollection<DiscountRuleBoolean>? componenets = null;
            if (post.discountRuleBooleanType == DiscountRuleBooleanType.OR || post.discountRuleBooleanType == DiscountRuleBooleanType.AND || post.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION)
                componenets = new List<DiscountRuleBoolean>();
            var drb = new DiscountRuleBoolean
            {
                Id = post.Id,
                discountRuleBooleanType = post.discountRuleBooleanType,
                Components = componenets,
                conditionString = post.conditionString,
                conditionLimit = post.conditionLimit,
                minHour = post.minHour,
                maxHour = post.maxHour,

            };
            discountRule.discountRuleBoolean = drb;
            _context.DiscountRuleBooleans.Add(drb);
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
        
        public bool Evaluate(DiscountRuleBoolean purchaseRule, ICollection<TransactionItem> items)
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
                    i => i.ProductName == purchaseRule.conditionString &&
                    i.Quantity >= purchaseRule.conditionLimit).Count() > 0;

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_LIMIT)
                return items.Where(
                    i => i.ProductName == purchaseRule.conditionString &&
                    i.Quantity > purchaseRule.conditionLimit).Count() == 0;

            /*else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.CATEGORY_AT_LEAST)
                return items.Where(
                    i => i.Product.Category.Name == purchaseRule.conditionString)
                    .Sum(i => i.Quantity) >= purchaseRule.conditionLimit;

            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.CATEGORY_LIMIT)
                return items.Where(
                    i => i.Product.Category.Name == purchaseRule.conditionString)
                    .Sum(i => i.Quantity) <= purchaseRule.conditionLimit;*/
            else if (purchaseRule.discountRuleBooleanType == DiscountRuleBooleanType.TIME_HOUR_AT_DAY)
                return purchaseRule.minHour <= DateTime.Now.Hour && DateTime.Now.Hour < purchaseRule.maxHour;

            //else if (purchaseRule.purchaseRuleType == PurchaseRuleType.TIME_DAY_AT_WEEK)
            //    return purchaseRule.weekDays[(int)DateTime.Now.DayOfWeek];
            return true;
        }

        /*public async Task<Response<ICollection<PurchaseRule>>> GetDiscountRuleBooleans(long storeId)
        {
            var purchaseRules = await _context.PurchaseRules
                .Where(dr => dr.store.Id == storeId)
                .ToListAsync();
            return Response<ICollection<PurchaseRule>>.Success(HttpStatusCode.OK, purchaseRules);
        }*/
    }

    public class TranscationCalculation
    {
        public TransactionItem transcationItem { get; set; }
        public double discount { get; set; }
    }
}
