using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Objects
{
    public abstract class PurchaseRuleComponent
    {
        public PurchaseRule _model { get; set; }
        public PurchaseRuleComponent(PurchaseRule model) { _model = model; }
        public static PurchaseRuleComponent Build(PurchaseRule model)
        {
            if (model == null)
                return new PurchaseRuleComponentDefault(model);
            if (model.purchaseRuleType == PurchaseRuleType.AND)
                return new PurchaseRuleComponentAnd(model);
            if (model.purchaseRuleType == PurchaseRuleType.OR)
                return new PurchaseRuleComponentOr(model);
            if (model.purchaseRuleType == PurchaseRuleType.PRODUCT_AT_LEAST)
                return new PurchaseRuleComponentAtLeastProduct(model);
            if (model.purchaseRuleType == PurchaseRuleType.PRODUCT_LIMIT)
                return new PurchaseRuleComponentLimitProduct(model);
            return new PurchaseRuleComponentDefault(model);
        }
        public abstract bool Eval(ICollection<TransactionItem> items);
    }
    public class PurchaseRuleComponentDefault : PurchaseRuleComponent
    {
        public PurchaseRuleComponentDefault(PurchaseRule model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return true;
        }
    }
    public class PurchaseRuleComponentAnd : PurchaseRuleComponent
    {
        public PurchaseRuleComponentAnd(PurchaseRule model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm).Eval(items)).All(res => res);
        }
    }
    public class PurchaseRuleComponentOr : PurchaseRuleComponent
    {
        public PurchaseRuleComponentOr(PurchaseRule model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm).Eval(items)).Any(res => res);
        }
    }
    public class PurchaseRuleComponentAtLeastProduct : PurchaseRuleComponent
    {
        public PurchaseRuleComponentAtLeastProduct(PurchaseRule model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity >= _model.conditionLimit).Count() > 0;
        }
    }
    public class PurchaseRuleComponentLimitProduct : PurchaseRuleComponent
    {
        public PurchaseRuleComponentLimitProduct(PurchaseRule model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity < _model.conditionLimit).Count() > 0;
        }
    }
}
