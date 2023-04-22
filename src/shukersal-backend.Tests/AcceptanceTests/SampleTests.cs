using shukersal_backend.Controllers;
using shukersal_backend.Models;
using Xunit.Abstractions;

namespace shukersal_backend.Tests.AcceptanceTests
{

    public class SampleTests
    {
        private readonly MarketDbContext _dbContext;
        private readonly StoresController _storesController;
        private readonly ITestOutputHelper output;

        public SampleTests(ITestOutputHelper output)
        {
            this.output = output;

            var dbOptions = new DbContextOptionsBuilder<MarketDbContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options;

            _dbContext = new MarketDbContext(dbOptions);

            InitializeData();


            _storesController = new StoresController(_dbContext);
        }

        [Fact]
        public async Task SampleTest1()
        {
            var result = await _storesController.GetCategories();
            Assert.NotNull(result);
        }

        private void InitializeData()
        {
            // Add test data to the DbContext
            _dbContext.Categories.Add(new Category { Id = 1, Name = "Electronics" });
            _dbContext.Categories.Add(new Category { Id = 2, Name = "Electronics" });
            _dbContext.Categories.Add(new Category { Id = 3, Name = "Electronics" });

            _dbContext.SaveChanges();
        }

    }
}
