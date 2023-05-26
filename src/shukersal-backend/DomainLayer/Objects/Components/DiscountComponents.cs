using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Objects
{
    public abstract class DiscountComponent
    {
        public DiscountRule _model {get; set;}
        public DiscountComponent(DiscountRule model) { _model = model; }
        public static DiscountComponent Build (DiscountRule model) {
            if (model == null)
                return new DiscountComponentDefault(model);
            if (model.discountType == DiscountType.SIMPLE)
                return new DiscountComponentSimple(model);
            if (model.discountType == DiscountType.CONDITIONAL)
                return new DiscountComponentConditional(model);
            if (model.discountType == DiscountType.ADDITIONAL)
                return new DiscountComponentAdditional(model);
            if (model.discountType == DiscountType.MAX)
                return new DiscountComponentMax(model);
            return new DiscountComponentDefault(model);
        }
        public abstract IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items);
    }
    public class DiscountComponentDefault : DiscountComponent
    {
        public DiscountComponentDefault(DiscountRule model) : base(model) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            return transcations;
        }
    }
    public class DiscountComponentSimple : DiscountComponent
    {
        public DiscountComponentSimple(DiscountRule model) : base(model) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            if (_model.discountOn == DiscountOn.STORE)
                return transcations.Select(t => new TranscationCalculation
                {
                    transcationItem = t.transcationItem,
                    discount = t.transcationItem.FullPrice * _model.Discount / 100
                });


            else if (_model.discountOn == DiscountOn.CATEGORY)
                return transcations.Select(t => new TranscationCalculation
                {
                    transcationItem = t.transcationItem,
                    discount = t.transcationItem.FullPrice * _model.Discount / 100
                });

            else if (_model.discountOn == DiscountOn.PRODUCT)
                return transcations.Select(t =>
                    t.transcationItem.ProductName == _model.discountOnString ?
                        new TranscationCalculation
                        {
                            transcationItem = t.transcationItem,
                            discount = t.transcationItem.FullPrice * _model.Discount / 100
                        }
                    : t
                );
            return transcations;
        }
    }

    public class DiscountComponentConditional : DiscountComponent
    {
        public DiscountComponentConditional(DiscountRule model) : base(model) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            if (DiscountComponentBoolean.Build(_model.discountRuleBoolean).Eval(items))
            {
                if (_model.discountOn == DiscountOn.STORE)
                    return transcations.Select(t => new TranscationCalculation
                    {
                        transcationItem = t.transcationItem,
                        discount = t.transcationItem.FullPrice * _model.Discount / 100
                    });


                else if (_model.discountOn == DiscountOn.CATEGORY)
                    return transcations.Select(t => new TranscationCalculation
                    {
                        transcationItem = t.transcationItem,
                        discount = t.transcationItem.FullPrice * _model.Discount / 100
                    });

                else if (_model.discountOn == DiscountOn.PRODUCT)
                    return transcations.Select(t =>
                        t.transcationItem.ProductName == _model.discountOnString ?
                            new TranscationCalculation
                            {
                                transcationItem = t.transcationItem,
                                discount = t.transcationItem.FullPrice * _model.Discount / 100
                            }
                        : t
                    );
            }
            return transcations;

        }
    }

    public class DiscountComponentAdditional : DiscountComponent
    {
        public DiscountComponentAdditional(DiscountRule model) : base(model) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            var tcList = _model.Components.Select(cm => Build(cm).Calculate(items));
            return transcations.Select(ta => new TranscationCalculation
            {
                transcationItem = ta.transcationItem,
                discount = tcList.Select(
                    t => t.Where(i => i.transcationItem == ta.transcationItem).FirstOrDefault().discount)
                .Sum()
            });
        }
    }
    public class DiscountComponentMax : DiscountComponent
    {
        public DiscountComponentMax(DiscountRule model) : base(model) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            var tcList = _model.Components.Select(cm => Build(cm).Calculate(items));
            return tcList.Where(tc => tc.Select(i => i.discount).Sum()
                 == tcList.Select(
                    t => t.Select(i => i.discount).Sum())
                .Max()).FirstOrDefault();
        }
    }
}
