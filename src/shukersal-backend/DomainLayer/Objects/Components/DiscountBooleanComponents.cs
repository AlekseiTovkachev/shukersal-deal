using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Objects
{
    public abstract class DiscountComponentBoolean
    {
        public DiscountRuleBoolean _model { get; set; }
        public DiscountComponentBoolean(DiscountRuleBoolean model) { _model = model; }
        public static DiscountComponentBoolean Build(DiscountRuleBoolean model)
        {
            if (model == null)
                return new DiscountComponentBooleanDefault(model);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.AND)
                return new DiscountComponentBooleanAnd(model);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.OR)
                return new DiscountComponentBooleanOr(model);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_AT_LEAST)
                return new DiscountComponentBooleanAtLeastProduct(model);
            if (model.discountRuleBooleanType == DiscountRuleBooleanType.PRODUCT_LIMIT)
                return new DiscountComponentBooleanLimitProduct(model);
            return new DiscountComponentBooleanDefault(model);
        }
        public abstract bool Eval(ICollection<TransactionItem> items);
    }
    public class DiscountComponentBooleanDefault : DiscountComponentBoolean
    {
        public DiscountComponentBooleanDefault(DiscountRuleBoolean model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return true;
        }
    }
    public class DiscountComponentBooleanAnd : DiscountComponentBoolean
    {
        public DiscountComponentBooleanAnd(DiscountRuleBoolean model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm).Eval(items)).All(res => res);
        }
    }
    public class DiscountComponentBooleanOr : DiscountComponentBoolean
    {
        public DiscountComponentBooleanOr(DiscountRuleBoolean model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return _model.Components.Select(cm => Build(cm).Eval(items)).Any(res => res);
        }
    }
    public class DiscountComponentBooleanAtLeastProduct : DiscountComponentBoolean
    {
        public DiscountComponentBooleanAtLeastProduct(DiscountRuleBoolean model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity >= _model.conditionLimit).Count() > 0;
        }
    }
    public class DiscountComponentBooleanLimitProduct : DiscountComponentBoolean
    {
        public DiscountComponentBooleanLimitProduct(DiscountRuleBoolean model) : base(model) { }
        public override bool Eval(ICollection<TransactionItem> items)
        {
            return items.Where(
                    i => i.ProductName == _model.conditionString &&
                    i.Quantity < _model.conditionLimit).Count() > 0;
        }
    }
}
