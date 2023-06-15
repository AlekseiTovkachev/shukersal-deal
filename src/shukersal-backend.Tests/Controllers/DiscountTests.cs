using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Models.StoreModels;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.Controllers
{
    public class DiscountTests
    {
        private readonly Mock<MarketDbContext> _context;
        private readonly DiscountObject _object;
        private readonly PurchaseRuleObject _prObject;
        private readonly ICollection<TransactionItem> items;
        private readonly ITestOutputHelper output;
        private readonly Store store;
        public DiscountTests(ITestOutputHelper output)
        {
            _context = new Mock<MarketDbContext>();

            var l1 = new List<DiscountRule> { };
            _context.Setup(d => d.DiscountRules).ReturnsDbSet(l1.AsQueryable());
            _context.Setup(d => d.DiscountRules.Add(It.IsAny<DiscountRule>())).Callback<DiscountRule>(d => l1.Add(d));

            var l2 = new List<DiscountRuleBoolean> { };
            _context.Setup(d => d.DiscountRuleBooleans).ReturnsDbSet(l2.AsQueryable());
            _context.Setup(d => d.DiscountRuleBooleans.Add(It.IsAny<DiscountRuleBoolean>())).Callback<DiscountRuleBoolean>(d => l2.Add(d));

            store = new Store { Id = 1, DiscountRules = new List<DiscountRule> { }, PurchaseRules = new List<PurchaseRule> { } };

            var l3 = new List<Store> { store };
            _context.Setup(d => d.Stores).ReturnsDbSet(l3.AsQueryable());
            _context.Setup(d => d.Stores.Add(It.IsAny<Store>())).Callback<Store>(d => l3.Add(d));

            var l4 = new List<PurchaseRule> { };
            _context.Setup(d => d.PurchaseRules).ReturnsDbSet(l4.AsQueryable());
            _context.Setup(d => d.PurchaseRules.Add(It.IsAny<PurchaseRule>())).Callback<PurchaseRule>(d => l4.Add(d));

            this.output = output;
            _object = new DiscountObject(_context.Object);
            _prObject = new PurchaseRuleObject(_context.Object);
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
        public async void TestXorDiscount()
        {
            Assert.Equal(90, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 21,
                    discountType = DiscountType.XOR_MIN,
                    Components = new List<DiscountRule>
                    {
                        new DiscountRule
                        {
                            Id = 22,
                            discountType = DiscountType.SIMPLE,
                            Discount = 10,
                            discountOn = DiscountOn.STORE
                        },
                        new DiscountRule
                        {
                            Id = 23,
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
            Assert.Equal(80, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 24,
                    discountType = DiscountType.XOR_MAX,
                    Components = new List<DiscountRule>
                    {
                        new DiscountRule
                        {
                            Id = 25,
                            discountType = DiscountType.SIMPLE,
                            Discount = 10,
                            discountOn = DiscountOn.STORE
                        },
                        new DiscountRule
                        {
                            Id = 26,
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
            Assert.Equal(90, await _object.CalculateDiscount(
                new DiscountRule
                {
                    Id = 27,
                    discountType = DiscountType.XOR_PRIORRITY,
                    Components = new List<DiscountRule>
                    {
                        new DiscountRule
                        {
                            Id = 28,
                            discountType = DiscountType.SIMPLE,
                            Discount = 10,
                            discountOn = DiscountOn.STORE
                        },
                        new DiscountRule
                        {
                            Id = 29,
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

        [Fact]
        public async void AddDiscountTest()
        {
            await _object.CreateDiscount(new DiscountRulePost
            {
                //Id = 13,
                discountType = DiscountType.ADDITIONAL
            },
            store);
            await _object.CreateChildDiscount(13, new DiscountRulePost
            {
                //Id = 14,
                discountType = DiscountType.CONDITIONAL,
                discountOn = DiscountOn.STORE,
                Discount = 10
            });
            await _object.SelectDiscount(store, 13);
            Assert.Equal(10, (await _object.GetAppliedDiscount(store.Id)).Result.Components.FirstOrDefault().Discount);
            await _object.CreateDiscountRuleBoolean(new DiscountRuleBooleanPost
            {
                //Id = 15,
                discountRuleBooleanType = DiscountRuleBooleanType.AND
            }, (await _object.GetAppliedDiscount(store.Id)).Result.Components.FirstOrDefault());
            await _object.CreateChildDiscountRuleBoolean(15,
                new DiscountRuleBooleanPost
                {
                    //Id = 16,
                    discountRuleBooleanType = DiscountRuleBooleanType.PRODUCT_AT_LEAST,
                    conditionString = "",
                    conditionLimit = 5
                });
            Assert.Equal(5, (await _object.GetDiscounts(store.Id)).Result.FirstOrDefault().Components.FirstOrDefault().discountRuleBoolean.Components.FirstOrDefault().conditionLimit);
        }

        [Fact]
        public async void TestConditiionalPurchaseRule()
        {
            Assert.False(_prObject.Evaluate(
                new PurchaseRule
                {
                    Id = 3,
                    purchaseRuleType = PurchaseRuleType.CONDITION,
                    Components = new List<PurchaseRule>
                    {
                        new PurchaseRule
                        {
                            Id = 4,
                            purchaseRuleType = PurchaseRuleType.PRODUCT_AT_LEAST,
                            conditionLimit = 3,
                            conditionString = "1"
                        },
                        new PurchaseRule
                        {
                            Id = 5,
                            purchaseRuleType = PurchaseRuleType.PRODUCT_LIMIT,
                            conditionLimit = 3,
                            conditionString = "2"
                        }
                    }

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



            Assert.True(_prObject.Evaluate(
                new PurchaseRule
                {
                    Id = 6,
                    purchaseRuleType = PurchaseRuleType.CONDITION,
                    Components = new List<PurchaseRule>
                    {
                        new PurchaseRule
                        {
                            Id = 7,
                            purchaseRuleType = PurchaseRuleType.PRODUCT_LIMIT,
                            conditionLimit = 3,
                            conditionString = "1"
                        },
                        new PurchaseRule
                        {
                            Id = 8,
                            purchaseRuleType = PurchaseRuleType.PRODUCT_AT_LEAST,
                            conditionLimit = 3,
                            conditionString = "2"
                        }
                    }

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
        [Fact]
        public async void TestHourCondition()
        {
            Assert.True(_prObject.Evaluate(
                new PurchaseRule
                {
                    Id = 9,
                    purchaseRuleType = PurchaseRuleType.TIME_HOUR_AT_DAY,
                    minHour = 0,
                    maxHour = 24

                },
                new List<TransactionItem> { })
            );

            Assert.False(_prObject.Evaluate(
                new PurchaseRule
                {
                    Id = 10,
                    purchaseRuleType = PurchaseRuleType.TIME_HOUR_AT_DAY,
                    minHour = 5,
                    maxHour = 3

                },
                new List<TransactionItem> { })
            );
        }

        [Fact]
        public async void AddPurchaseRuleTest()
        {
            await _prObject.CreatePurchaseRule(new PurchaseRulePost
            {
                //Id = 1,
                purchaseRuleType = PurchaseRuleType.AND
            },
            store);
            await _prObject.CreateChildPurchaseRule(1, new PurchaseRulePost
            {
                //Id = 2,
                purchaseRuleType = PurchaseRuleType.PRODUCT_LIMIT,
                conditionString = "",
                conditionLimit = 5
            });
            await _prObject.SelectPurchaseRule(store, 1);
            Assert.Equal(5, (await _prObject.GetAppliedPurchaseRule(store.Id)).Result.Components.FirstOrDefault().conditionLimit);
            Assert.Equal(5, (await _prObject.GetPurchaseRules(store.Id)).Result.FirstOrDefault().Components.FirstOrDefault().conditionLimit);
        }

        [Fact]
        public async void SuperComplicatedDiscountTest()
        {
            //NICE
            Assert.Equal(69420, await _object.CalculateDiscount(
                new DiscountRule //base price = 111,100
                {
                    Id = 6942001,
                    discountType = DiscountType.ADDITIONAL,
                    Components = new List<DiscountRule>
                    {
                        new DiscountRule
                        {
                            Id = 6942002,
                            discountType = DiscountType.MAX,
                            Components = new List<DiscountRule>
                            {
                                new DiscountRule //condition fail, do nothing
                                {
                                    Id = 6942003,
                                    discountType = DiscountType.CONDITIONAL,
                                    discountOn = DiscountOn.STORE,
                                    Discount = 60,
                                    discountRuleBoolean = new DiscountRuleBoolean
                                    {
                                        Id = 6942004,
                                        discountRuleBooleanType = DiscountRuleBooleanType.AND,
                                        Components = new List<DiscountRuleBoolean>
                                        {
                                            new DiscountRuleBoolean
                                            {
                                                Id = 6942005,
                                                discountRuleBooleanType = DiscountRuleBooleanType.PRODUCT_AT_LEAST,
                                                conditionLimit = 60,
                                                conditionString = "2"

                                            },
                                            new DiscountRuleBoolean
                                            {
                                                Id = 6942005,
                                                discountRuleBooleanType = DiscountRuleBooleanType.CATEGORY_LIMIT,
                                                conditionLimit = 40,
                                                conditionString = "3"

                                            }
                                        }
                                    }
                                },
                                new DiscountRule //new price = 71,100
                                {
                                    Id = 6942006,
                                    discountType = DiscountType.SIMPLE,
                                    discountOn = DiscountOn.PRODUCT,
                                    discountOnString = "4",
                                    Discount = 40
                                }
                            }
                        }, //new price = 69,600
                        new DiscountRule
                        {
                            Id = 6942007,
                            discountType = DiscountType.SIMPLE,
                            discountOn = DiscountOn.PRODUCT,
                            discountOnString = "3",
                            Discount = 15
                        }, //new price = 69,420
                        new DiscountRule
                        {
                            Id = 6942008,
                            discountType = DiscountType.CONDITIONAL,
                            discountOn = DiscountOn.PRODUCT,
                            discountOnString = "2",
                            Discount = 18,
                            discountRuleBoolean = new DiscountRuleBoolean
                            {
                                Id = 6942009,
                                discountRuleBooleanType = DiscountRuleBooleanType.OR,
                                Components = new List<DiscountRuleBoolean>
                                {
                                    new DiscountRuleBoolean
                                    {
                                        Id = 6942010,
                                        discountRuleBooleanType = DiscountRuleBooleanType.PRODUCT_AT_LEAST,
                                        conditionLimit = 60,
                                        conditionString = "2"

                                    },
                                    new DiscountRuleBoolean
                                    {
                                        Id = 6942011,
                                        discountRuleBooleanType = DiscountRuleBooleanType.CATEGORY_LIMIT,
                                        conditionLimit = 40,
                                        conditionString = "3"

                                    }
                                }
                            }
                        },
                    }


                },
                new List<TransactionItem> {
                    new TransactionItem{
                        ProductName = "1",
                        Quantity = 1,
                        FullPrice = 100

                    },
                    new TransactionItem{
                        ProductName = "2",
                        Quantity = 1,
                        FullPrice = 1000

                    },
                    new TransactionItem{
                        ProductName = "3",
                        Quantity = 1,
                        FullPrice = 10000
                    },
                    new TransactionItem{
                        ProductName = "4",
                        Quantity = 1,
                        FullPrice = 100000
                    }
                }
                )
            );
        }
    }
}
