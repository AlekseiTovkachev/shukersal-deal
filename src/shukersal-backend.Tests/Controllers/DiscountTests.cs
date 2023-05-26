using Microsoft.VisualStudio.TestPlatform.Utilities;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.Controllers
{
    public class DiscountTests
    {
        private readonly Mock<MarketDbContext> _context;
        private readonly DiscountObject _object;
        private readonly ICollection<TransactionItem> items;
        private readonly ITestOutputHelper output;
        private readonly Store store;
        public DiscountTests(ITestOutputHelper output)
        {
            _context = new Mock<MarketDbContext>();
            _context.Setup(d => d.DiscountRules).ReturnsDbSet(new List<DiscountRule>());
            _context.Setup(d => d.DiscountRuleBooleans).ReturnsDbSet(new List<DiscountRuleBoolean>());
            store = new Store { Id = 1 };
            _context.Setup(s => s.Stores).ReturnsDbSet(new List<Store> { store });
            this.output = output;
            _object = new DiscountObject(_context.Object);
            items = new List<TransactionItem> { };
        }

        [Fact]
        public async void TestSimpleDiscount()
        {
            Assert.Equal(90, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 1,
                    discountType = DiscountType.SIMPLE,
                    Discount = 10,
                    discountOn = DiscountOn.STORE
                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 100

                    }
                }
                )
            );
        }

        [Fact]
        public async void TestAdditionalDiscount()
        {
            Assert.Equal(70, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 2,
                    discountType = DiscountType.ADDITIONAL,
                    Components = new List<DiscountRule>
                    {
                        new DiscountRule
                        {
                            Id = 3,
                            discountType = DiscountType.SIMPLE,
                            Discount = 10,
                            discountOn = DiscountOn.STORE
                        },
                        new DiscountRule
                        {
                            Id = 4,
                            discountType = DiscountType.SIMPLE,
                            Discount = 20,
                            discountOn = DiscountOn.STORE
                        }
                    }

                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 100

                    }
                }
                )
            );
        }


        [Fact]
        public async void TestMaxDiscount()
        {
            Assert.Equal(80, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 5,
                    discountType = DiscountType.MAX,
                    Components = new List<DiscountRule>
                    {
                        new DiscountRule
                        {
                            Id = 6,
                            discountType = DiscountType.SIMPLE,
                            Discount = 10,
                            discountOn = DiscountOn.STORE
                        },
                        new DiscountRule
                        {
                            Id = 7,
                            discountType = DiscountType.SIMPLE,
                            Discount = 20,
                            discountOn = DiscountOn.STORE
                        }
                    }

                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 100

                    }
                }
                )
            );
        }


        [Fact]
        public async void TestConditionalDiscount()
        {
            Assert.Equal(100, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 8,
                    discountType = DiscountType.CONDITIONAL,
                    discountOn = DiscountOn.STORE,
                    Discount = 10,
                    discountRuleBoolean = new DiscountRuleBoolean
                    {
                        Id = 9,
                        discountRuleBooleanType = DiscountRuleBooleanType.PRODUCT_AT_LEAST,
                        conditionLimit = 2,
                        conditionString = "1"
                    }

                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 100

                    }
                }
                )
            );

            Assert.Equal(90, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 10,
                    discountType = DiscountType.CONDITIONAL,
                    discountOn = DiscountOn.STORE,
                    Discount = 10,
                    discountRuleBoolean = new DiscountRuleBoolean
                    {
                        Id = 11,
                        discountRuleBooleanType = DiscountRuleBooleanType.PRODUCT_LIMIT,
                        conditionLimit = 2,
                        conditionString = "1"
                    }

                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 100

                    }
                }
                )
            );
        }
        [Fact]
        public async void TestDiscountOnProduct()
        {
            Assert.Equal(280, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 12,
                    discountType = DiscountType.SIMPLE,
                    discountOn = DiscountOn.PRODUCT,
                    discountOnString = "1",
                    Discount = 10,

                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 200

                    },
                    new TransactionItem{
                        ProductName = "2",
                        Quantity = 1,
                        FullPrice = 100

                    }
                }
                )
            );
        }
    }
}
