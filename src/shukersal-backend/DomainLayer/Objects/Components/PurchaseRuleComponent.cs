using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Objects
{
    public abstract class PurchaseRuleComponent
    {
        public PurchaseRule _model { get; set; }
        public MarketDbContext _context { get; set; }
        public PurchaseRuleComponent(PurchaseRule model, MarketDbContext context) { _model = model; _context = context; }
        public static PurchaseRuleComponent Build(PurchaseRule model, MarketDbContext context)
        {
            if (model == null)
                return new PurchaseRuleComponentDefault(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.AND)
                return new PurchaseRuleComponentAnd(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.OR)
                return new PurchaseRuleComponentOr(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.CONDITION)
                return new PurchaseRuleComponentCondition(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.PRODUCT_AT_LEAST)
                return new PurchaseRuleComponentAtLeastProduct(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.PRODUCT_LIMIT)
                return new PurchaseRuleComponentLimitProduct(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.CATEGORY_AT_LEAST)
                return new PurchaseRuleComponentAtLeastCategory(model, context);
            if (model.purchaseRuleType == PurchaseRuleType.CATEGORY_LIMIT)
                return new PurchaseRuleComponentLimitCategory(model, context);
            return new PurchaseRuleComponentDefault(model, context);
        }
        public abstract bool Eval(ICollection<TransactionItem> items);
    }
    public class PurchaseRuleComponentDefault : PurchaseRuleComponent
    {
        public PurchaseRuleComponentDefault(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return true;
        }
    }
    public class PurchaseRuleComponentAnd : PurchaseRuleComponent
    {
        public PurchaseRuleComponentAnd(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm, _context).Eval(items)).All(res => res);
        }
    }
    public class PurchaseRuleComponentOr : PurchaseRuleComponent
    {
        public PurchaseRuleComponentOr(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm, _context).Eval(items)).Any(res => res);
        }
    }
    public class PurchaseRuleComponentAtLeastProduct : PurchaseRuleComponent
    {
        public PurchaseRuleComponentAtLeastProduct(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity >= _model.conditionLimit).Count() > 0;
        }
    }
    public class PurchaseRuleComponentLimitProduct : PurchaseRuleComponent
    {
        public PurchaseRuleComponentLimitProduct(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity < _model.conditionLimit).Count() > 0;
        }
    }

    public class PurchaseRuleComponentAtLeastCategory : PurchaseRuleComponent
    {
        public PurchaseRuleComponentAtLeastCategory(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(i =>
                    _context.Products.Where(p => p.Id == i.ProductId).Count() != 0 &&
                    _context.Products.Where(p => p.Id == i.ProductId).First().Category.Name == _model.conditionString &&
                    i.Quantity >= _model.conditionLimit).Count() > 0;
        }
    }

    public class PurchaseRuleComponentLimitCategory : PurchaseRuleComponent
    {
        public PurchaseRuleComponentLimitCategory(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(i =>
                    _context.Products.Where(p => p.Id == i.ProductId).Count() != 0 &&
                    _context.Products.Where(p => p.Id == i.ProductId).First().Category.Name == _model.conditionString &&
                    i.Quantity < _model.conditionLimit).Count() > 0;
        }
    }

    public class PurchaseRuleComponentCondition : PurchaseRuleComponent
    {
        public PurchaseRuleComponentCondition(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return
                _model.Components == null ||
                _model.Components.Count != 2 ||
                !Build(_model.Components.ElementAt(0), _context).Eval(items) ||
                Build(_model.Components.ElementAt(0), _context).Eval(items);
        }
    }

    public class PurchaseRuleTimeAtDay : PurchaseRuleComponent
    {
        public PurchaseRuleTimeAtDay(PurchaseRule model, MarketDbContext context) : base(model, context) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.minHour <= DateTime.Now.Hour && DateTime.Now.Hour < _model.maxHour;
        }
    }
}
