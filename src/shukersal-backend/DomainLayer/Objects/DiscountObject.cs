using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;


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
                //Id = post.Id,
                Discount = post.Discount,
                discountType = post.discountType,
                Components = componenets,
                discountRuleBoolean = null,
                discountOn = post.discountOn,
                discountOnString = post.discountOnString,
                StoreId = s.Id,
                ParentId = null
            };

            s.DiscountRules.Add(dr);
            _context.DiscountRules.Add(dr);

            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.OK, true);
        }
        public async Task<Response<bool>> CreateChildDiscount(long compositeId, DiscountRulePost post)
        {
            var composite = await _context.DiscountRules
                .Where(dr => dr.Id == compositeId)
                .Include(dr => dr.Components)
                .FirstOrDefaultAsync();
            //var composite = await _context.DiscountRules.FirstOrDefaultAsync(dr => dr.Id == compositeId);
            if (composite != null && composite.discountType >= DiscountType.XOR_MAX && composite.discountType <= DiscountType.ADDITIONAL)
            {
                ICollection<DiscountRule>? componenets = null;
                if (post.discountType == DiscountType.ADDITIONAL || post.discountType == DiscountType.MAX)
                    componenets = new List<DiscountRule>();
                var component = new DiscountRule
                {
                    //Id = post.Id,
                    Discount = post.Discount,
                    discountType = post.discountType,
                    Components = componenets,
                    discountRuleBoolean = null,
                    discountOn = post.discountOn,
                    discountOnString = post.discountOnString,
                    ParentId = compositeId,
                    StoreId = composite.StoreId
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
            var res = DiscountComponent.Build(discountRule, _context).Calculate(transactionItems);
            foreach (var r in res)
                r.transcationItem.FinalPrice = r.transcationItem.FullPrice - r.discount;
            await _context.SaveChangesAsync();
            return res.Select(r => r.transcationItem.FinalPrice).Sum();

        }

        public async Task<Response<ICollection<DiscountRule>>> GetDiscounts(long storeId)
        {
            var discounts = await _context.DiscountRules
                .Where(d => d.StoreId == storeId)
                .Include(dr => dr.Components)
                .ToListAsync();
            var booleans = await _context.DiscountRuleBooleans
                .Include(dr => dr.Components)
                .ToListAsync();

            //var dcnt = await _context.DiscountRules.Include(dr => dr.Components).ToListAsync();
            //var dcnt2 = await _context.DiscountRuleBooleans.Include(dr => dr.Components).ToListAsync();
            var store = await _context.Stores.Where(s => s.Id == storeId)
                .Include(s => s.DiscountRules)
                .FirstOrDefaultAsync();

            if (store == null)
                return Response<ICollection<DiscountRule>>.Error(HttpStatusCode.NotFound, "store doesnt exist");

            var trees = store.DiscountRules.Where(dr => dr.ParentId == null).ToList();

            return Response<ICollection<DiscountRule>>.Success(HttpStatusCode.OK, trees);
        }

        public async Task<Response<DiscountRule>> GetAppliedDiscount(long storeId)
        {
            var discounts = await _context.DiscountRules
                .Where(d => d.StoreId == storeId)
                .Include(dr => dr.Components)
                .ToListAsync();
            var booleans = await _context.DiscountRuleBooleans
                .Include(dr => dr.Components)
                .ToListAsync();
            //var dcnt = await _context.DiscountRules.Include(dr => dr.Components).ToListAsync();
            //var dcnt2 = await _context.DiscountRuleBooleans.Include(dr => dr.Components).ToListAsync();
            var store = await _context.Stores.Where(s => s.Id == storeId)
                .Include(s => s.AppliedDiscountRule)
                .FirstOrDefaultAsync();

            if (store != null && store.AppliedDiscountRule != null)
                return Response<DiscountRule>.Success(HttpStatusCode.OK, store.AppliedDiscountRule);
            return Response<DiscountRule>.Error(HttpStatusCode.NotFound, "store doesnt exist");
        }

        public async Task<Response<DiscountRule>> SelectDiscount(Store s, long DiscountRuleId)
        {
            // only the root discount can be chosen
            var r = s.DiscountRules.Where(dr => dr.Id == DiscountRuleId && dr.ParentId == null);
            if (r == null || r.Count() == 0)
                return Response<DiscountRule>.Error(HttpStatusCode.NotFound, "discount doesnt exist");
            s.AppliedDiscountRule = r.FirstOrDefault();
            await _context.SaveChangesAsync();
            return Response<DiscountRule>.Success(HttpStatusCode.OK, r.FirstOrDefault());
        }
        public async Task<Response<bool>> CreateDiscountRuleBoolean(DiscountRuleBooleanPost post, DiscountRule discountRule)
        {
            if (discountRule.discountType != DiscountType.CONDITIONAL)
            {
                return Response<bool>.Error(HttpStatusCode.BadRequest, "Only conditional discount can have DiscountRuleBoolean");
            }
            ICollection<DiscountRuleBoolean>? componenets = null;
            if (post.discountRuleBooleanType == DiscountRuleBooleanType.OR || post.discountRuleBooleanType == DiscountRuleBooleanType.AND || post.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION)
                componenets = new List<DiscountRuleBoolean>();
            var drb = new DiscountRuleBoolean
            {
                //Id = post.Id,
                discountRuleBooleanType = post.discountRuleBooleanType,
                Components = componenets,
                conditionString = post.conditionString,
                conditionLimit = post.conditionLimit,
                minHour = post.minHour,
                maxHour = post.maxHour,
                IsRoot = true,
                RootDiscountId = discountRule.Id
            };
            discountRule.discountRuleBoolean = drb;
            _context.DiscountRuleBooleans.Add(drb);
            await _context.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.OK, true);
        }
        public async Task<Response<bool>> CreateChildDiscountRuleBoolean(long compositeId, DiscountRuleBooleanPost post)
        {
            var composite = await _context.DiscountRuleBooleans
                .Where(dr => dr.Id == compositeId)
                .Include(dr => dr.Components)
                .FirstOrDefaultAsync();
            //var composite = await _context.DiscountRuleBooleans.FirstOrDefaultAsync(dr => dr.Id == compositeId);
            if (composite != null && (composite.discountRuleBooleanType == DiscountRuleBooleanType.OR || composite.discountRuleBooleanType == DiscountRuleBooleanType.AND || composite.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION))
            {
                ICollection<DiscountRuleBoolean>? components = null;
                if (post.discountRuleBooleanType == DiscountRuleBooleanType.OR || post.discountRuleBooleanType == DiscountRuleBooleanType.AND || post.discountRuleBooleanType == DiscountRuleBooleanType.CONDITION)
                    components = new List<DiscountRuleBoolean>();
                var component = new DiscountRuleBoolean
                {
                    //Id = post.Id,
                    discountRuleBooleanType = post.discountRuleBooleanType,
                    Components = components,
                    conditionString = post.conditionString,
                    conditionLimit = post.conditionLimit,
                    minHour = post.minHour,
                    maxHour = post.maxHour,
                    IsRoot = false,
                    //RootDiscountId = composite.RootDiscountId
                };
                _context.DiscountRuleBooleans.Add(component);
                composite.Components?.Add(component);
                await _context.SaveChangesAsync();
                return Response<bool>.Success(HttpStatusCode.OK, true);
            }
            return Response<bool>.Success(HttpStatusCode.NotFound, false);
        }

    }

    public class TranscationCalculation
    {
        public TransactionItem transcationItem { get; set; }
        public double discount { get; set; }
    }
}
