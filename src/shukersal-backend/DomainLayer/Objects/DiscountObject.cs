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
        public async Task<Response<DiscountRule>> CreateDiscount(DiscountRulePost post, Store s)
        {
            ICollection<DiscountRule>? componenets = null;
            if (post.discountType == DiscountType.ADDITIONAL || post.discountType == DiscountType.MAX)
                componenets= new List<DiscountRule>();
            var discount = new DiscountRule
            {
                Id = post.Id,
                store = s,
                Discount = post.Discount,
                discountType = post.discountType,
                Components = componenets,
                PurchaseRule = post.PurchaseRule,
                discountOn = post.discountOn,
                discountOnString = post.discountOnString
            };
            _context.DiscountRules.Add(discount);
            await _context.SaveChangesAsync();
            return Response<DiscountRule>.Success(HttpStatusCode.OK, discount);
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
                    PurchaseRule = post.PurchaseRule,
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
                 discountRule.PurchaseRule != null &&
                _purchaseRuleObject.Evaluate( discountRule.PurchaseRule, items))
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
    }
}
