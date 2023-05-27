using Microsoft.EntityFrameworkCore;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;

namespace shukersal_backend.DomainLayer.Objects
{
    public abstract class DiscountComponent
    {
        public DiscountRule _model {get; set;}
        public MarketDbContext _context { get; set;}
        public DiscountComponent(DiscountRule model, MarketDbContext context) { _model = model; _context = context; }
        public static DiscountComponent Build (DiscountRule model, MarketDbContext context) {
            if (model == null)
                return new DiscountComponentDefault(model, context);
            if (model.discountType == DiscountType.SIMPLE)
                return new DiscountComponentSimple(model, context);
            if (model.discountType == DiscountType.CONDITIONAL)
                return new DiscountComponentConditional(model, context);
            if (model.discountType == DiscountType.XOR_MIN)
                return new DiscountComponentSimple(model, context);
            if (model.discountType == DiscountType.XOR_MAX)
                return new DiscountComponentSimple(model, context);
            if (model.discountType == DiscountType.XOR_PRIORRITY)
                return new DiscountComponentSimple(model, context);
            if (model.discountType == DiscountType.ADDITIONAL)
                return new DiscountComponentAdditional(model, context);
            if (model.discountType == DiscountType.MAX)
                return new DiscountComponentMax(model, context);
            return new DiscountComponentDefault(model, context);
        }
        public abstract IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items);
    }
    public class DiscountComponentDefault : DiscountComponent
    {
        public DiscountComponentDefault(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
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
        public DiscountComponentSimple(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
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
            {
                return transcations.Select(t =>
                    _context.Products.Where(p => p.Id == t.transcationItem.ProductId).Count() != 0 &&
                    _context.Products.Where(p => p.Id == t.transcationItem.ProductId).First().Category.Name == _model.discountOnString ?
                        new TranscationCalculation
                        {
                            transcationItem = t.transcationItem,
                            discount = t.transcationItem.FullPrice * _model.Discount / 100
                        }
                    : t
                );
            }

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
        public DiscountComponentConditional(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            if (DiscountComponentBoolean.Build(_model.discountRuleBoolean, _context).Eval(items))
            {
                if (_model.discountOn == DiscountOn.STORE)
                    return transcations.Select(t => new TranscationCalculation
                    {
                        transcationItem = t.transcationItem,
                        discount = t.transcationItem.FullPrice * _model.Discount / 100
                    });


                else if (_model.discountOn == DiscountOn.CATEGORY)
                    return transcations.Select(t =>
                        _context.Products.Where(p => p.Id == t.transcationItem.ProductId).Count() != 0 &&
                        _context.Products.Where(p => p.Id == t.transcationItem.ProductId).First().Category.Name == _model.discountOnString ?
                            new TranscationCalculation
                            {
                                transcationItem = t.transcationItem,
                                discount = t.transcationItem.FullPrice * _model.Discount / 100
                            }
                        : t
                    );

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
        public DiscountComponentAdditional(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
            foreach (TransactionItem t in items)
            {
                transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
            }
            var tcList = _model.Components.Select(cm => Build(cm, _context).Calculate(items));
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
        public DiscountComponentMax(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            var tcList = _model.Components.Select(cm => Build(cm, _context).Calculate(items));
            return tcList.Where(tc => tc.Select(i => i.discount).Sum()
                 == tcList.Select(
                    t => t.Select(i => i.discount).Sum())
                .Max()).FirstOrDefault();
        }
    }

    public class DiscountComponentXorMin : DiscountComponent
    {
        public DiscountComponentXorMin(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            if (_model.Components == null || _model.Components.Count != 2)
            {
                List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
                foreach (TransactionItem t in items)
                {
                    transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
                }
                return transcations;
            }
            var c1 = Build(_model.Components.ElementAt(0), _context).Calculate(items);
            var c2 = Build(_model.Components.ElementAt(1), _context).Calculate(items);
            return c1.Select(i => i.discount).Sum() < c2.Select(i => i.discount).Sum() ? c1 : c2;
        }
    }

    public class DiscountComponentXorMax : DiscountComponent
    {
        public DiscountComponentXorMax(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            if (_model.Components == null || _model.Components.Count != 2)
            {
                List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
                foreach (TransactionItem t in items)
                {
                    transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
                }
                return transcations;
            }
            var c1 = Build(_model.Components.ElementAt(0), _context).Calculate(items);
            var c2 = Build(_model.Components.ElementAt(1), _context).Calculate(items);
            return c1.Select(i => i.discount).Sum() > c2.Select(i => i.discount).Sum() ? c1 : c2;
        }
    }
    public class DiscountComponentXorPriority : DiscountComponent
    {
        public DiscountComponentXorPriority(DiscountRule model, MarketDbContext _context) : base(model, _context) { }
        public override IEnumerable<TranscationCalculation> Calculate(ICollection<TransactionItem> items)
        {
            if (_model.Components == null || _model.Components.Count != 2)
            {
                List<TranscationCalculation> transcations = new List<TranscationCalculation> { };
                foreach (TransactionItem t in items)
                {
                    transcations.Add(new TranscationCalculation { discount = 0, transcationItem = t });
                }
                return transcations;
            }
            var c1 = Build(_model.Components.ElementAt(0), _context).Calculate(items);
            var c2 = Build(_model.Components.ElementAt(1), _context).Calculate(items);
            return c1.Select(i => i.discount).Sum() > 0 ? c1 : c2;
        }
    }
}
