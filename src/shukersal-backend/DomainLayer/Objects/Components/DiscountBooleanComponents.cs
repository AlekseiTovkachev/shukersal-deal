using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Objects
{
    public abstract class DiscountComponentBoolean
    {
        public DiscountRuleBoolean _model { get; set; }
        public MarketDbContext _context { get; set; }
        public DiscountComponentBoolean(DiscountRuleBoolean model, MarketDbContext context) { _model = model; _context = context; }
        public static DiscountComponentBoolean Build(DiscountRuleBoolean model, MarketDbContext context)
        {
            if (model == null)
                return new DiscountComponentBooleanDefault(model, context);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.AND)
                return new DiscountComponentBooleanAnd(model, context);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.OR)
                return new DiscountComponentBooleanOr(model, context);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_AT_LEAST)
                return new DiscountComponentBooleanAtLeastProduct(model, context);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_LIMIT)
                return new DiscountComponentBooleanLimitProduct(model, context);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.CATEGORY_AT_LEAST)
                return new DiscountComponentBooleanAtLeastProduct(model, context);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.CATEGORY_LIMIT)
                return new DiscountComponentBooleanLimitProduct(model, context);
            return new DiscountComponentBooleanDefault(model, context);
        }
        public abstract bool Eval(ICollection<TransactionItem> items);
    }
    public class DiscountComponentBooleanDefault : DiscountComponentBoolean
    {
        public DiscountComponentBooleanDefault(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return true;
        }
    }
    public class DiscountComponentBooleanAnd : DiscountComponentBoolean
    {
        public DiscountComponentBooleanAnd(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm, _context).Eval(items)).All(res => res);
        }
    }
    public class DiscountComponentBooleanOr : DiscountComponentBoolean
    {
        public DiscountComponentBooleanOr(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm, _context).Eval(items)).Any(res => res);
        }
    }
    public class DiscountComponentBooleanAtLeastProduct : DiscountComponentBoolean
    {
        public DiscountComponentBooleanAtLeastProduct(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity >= _model.conditionLimit).Count() > 0;
        }
    }
    public class DiscountComponentBooleanLimitProduct : DiscountComponentBoolean
    {
        public DiscountComponentBooleanLimitProduct(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity < _model.conditionLimit).Count() > 0;
        }
    }

    public class DiscountComponentBooleanAtLeastCategory : DiscountComponentBoolean
    {
        public DiscountComponentBooleanAtLeastCategory(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(i =>
                    _context.Products.Where(p => p.Id == i.ProductId).Count() != 0 &&
                    _context.Products.Where(p => p.Id == i.ProductId).First().Category.Name == _model.conditionString &&
                    i.Quantity >= _model.conditionLimit).Count() > 0;
        }
    }

    public class DiscountComponentBooleanLimitCategory : DiscountComponentBoolean
    {
        public DiscountComponentBooleanLimitCategory(DiscountRuleBoolean model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(i =>
                    _context.Products.Where(p => p.Id == i.ProductId).Count() != 0 &&
                    _context.Products.Where(p => p.Id == i.ProductId).First().Category.Name == _model.conditionString &&
                    i.Quantity < _model.conditionLimit).Count() > 0;
        }
    }
}
