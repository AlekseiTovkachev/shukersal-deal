using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace shukersal_backend.Tests.AcceptanceTests
{
    public class Bridge : IBridge
    {
        private readonly AuthService authService;
        private readonly StoreManagerService storeManagersService;
        private readonly MemberService memberService;
        private readonly TransactionService TransactionService;
        private readonly ShoppingCartService shoppingCartService;
        private readonly StoreService storeService;
        public readonly Mock<MarketDbContext> _context;
        public readonly Mock<ILogger<StoreService>> _logger;
        private readonly IConfiguration _configuration;
        public Bridge() {
            
            _context = new Mock<MarketDbContext>();
            _logger = new Mock<ILogger<StoreService>>();
            //_context.Setup(c => c.Database.EnsureCreated()).Returns(true);

            //TODO: init configuration
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            authService = new AuthService(_configuration, _context.Object, new Mock<ILogger<AuthService>>().Object);
            memberService = new MemberService(_context.Object, new Mock<ILogger<MemberService>>().Object);
            TransactionService = new TransactionService(_context.Object);
            shoppingCartService = new ShoppingCartService(_context.Object, new Mock<ILogger<ShoppingCartService>>().Object);
            
            storeService = new StoreService(_context.Object,_logger.Object);
        }
        //Member
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers() 
        {
            return await memberService.GetMembers();
        }
        public async Task<ActionResult<Member>> GetMember(long id)
        {
            return await memberService.GetMember(id);
        }
        public async Task<ActionResult<MemberPost>> AddMember(MemberPost memberData)
        {
            return await memberService.AddMember(memberData);
        }
        public async Task<IActionResult> DeleteMember(long id)
        {
            return await memberService.DeleteMember(id);
        }
        //Store
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            return await storeService.GetStores();
        }
        public async Task<ActionResult<Store>> GetStore(long id)
        {
            return await storeService.GetStore(id);
        }
        public async Task<ActionResult<Store>> CreateStore(StorePost storeData)
        {
            return await storeService.CreateStore(storeData);
        }
        public async Task<IActionResult> UpdateStore(long id, StorePatch patch)
        {
            return await storeService.UpdateStore(id, patch);
        }
        public async Task<IActionResult> DeleteStore(long id)
        {
            return await storeService.DeleteStore(id);
        }
        public async Task<IActionResult> AddProduct(long storeId, ProductPost product)
        {
            return await storeService.AddProduct(storeId, product);
        }
        public async Task<IActionResult> UpdateProduct(long storeId, long productId, ProductPatch product)
        {
            return await storeService.UpdateProduct(storeId, productId, product);
        }
        public async Task<IActionResult> DeleteProduct(long storeId, long productId)
        {
            return await storeService.DeleteProduct(storeId, productId);
        }
        public async Task<ActionResult<Store>> GetStoreProducts(long storeId)
        {
            return await storeService.GetStoreProducts(storeId);
        }
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await storeService.GetCategories();
        }
        //Shopping Cart
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByUserId(long memberId)
        {
            return await shoppingCartService.GetShoppingCartByUserId(memberId);
        }
        public async Task<ActionResult<ShoppingItem>> AddItemToCart(long id, [FromBody] ShoppingItem item)
        {
            return await shoppingCartService.AddItemToCart(id, item);
        }
        public async Task<ActionResult<ShoppingItem>> RemoveItemFromCart(long id, ShoppingItem itemId)
        {
            return await shoppingCartService.RemoveItemFromCart(id, itemId);
        }
        //Transaction
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await TransactionService.GetTransactions();
        }
        public async Task<ActionResult<Transaction>> GetTransaction(long TransactionId)
        {
            return await TransactionService.GetTransaction(TransactionId);
        }
        public async Task<ActionResult<Transaction>> PurchaseAShoppingCart(TransactionPost TransactionPost)
        {
            return await TransactionService.PurchaseAShoppingCart(TransactionPost);
        }
        public async Task<IActionResult> DeleteTransaction(long TransactionId)
        {
            return await TransactionService.DeleteTransaction(TransactionId);
        }
        public async Task<IActionResult> UpdateTransaction(long Transactionid, TransactionPost post)
        {
            return await TransactionService.UpdateTransaction(Transactionid, post);
        }
        public async Task<ActionResult<Transaction>> BrowseTransactionHistory(long memberId)
        {
            return await TransactionService.BrowseTransactionHistory(memberId);
        }
        public async Task<ActionResult<Transaction>> BrowseShopTransactionHistory(long storeId)
        {
            return await TransactionService.BrowseShopTransactionHistory(storeId);
        }
        //Manager
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            return await storeManagersService.GetStoreManagers();
        }
        public async Task<ActionResult<StoreManager>> GetStoreManager(long id)
        {
            return await storeManagersService.GetStoreManager(id);
        }
        public async Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {
            return await storeManagersService.PostStoreManager(post);
        }
        public async Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        {
            return await storeManagersService.PostStoreOwner(post);
        }
        public async Task<IActionResult> PutStoreManager(long id, StoreManager storeManager)
        {
            return await storeManagersService.PutStoreManager(id, storeManager);
        }
        public async Task<IActionResult> DeleteStoreManager(long id)
        {
            return await storeManagersService.DeleteStoreManager(id);
        }
        public async Task<IActionResult> AddPermissionToManager(long Id, [FromBody] PermissionType permission)
        {
            return await storeManagersService.AddPermissionToManager(Id, permission);
        }
        public async Task<IActionResult> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {
            return await storeManagersService.RemovePermissionFromManager(id, permission);
        }
        //Auth
        public async Task<ActionResult<LoginResponse>> Login(LoginPost loginRequest)
        {
            return await authService.Login(loginRequest, new ConfigurationBuilder().Build());
        }
        public async Task<ActionResult<RegisterPost>> Register(RegisterPost registerRequest)
        {
            return await authService.Register(registerRequest);
        }
        public async Task<ActionResult<Member>> GetLoggedUser()
        {
            return await authService.GetLoggedUser();
        }
        public async Task<ActionResult<Member>> ChangePassword(ChangePasswordPost changePasswordRequest)
        {
            return await authService.ChangePassword(changePasswordRequest);
        }

        public async void init()
        {
            var membersList = new List<Member>
            {
                new Member
                {
                    Id = 101,
                    Username = "TestM1",
                    PasswordHash = HashingUtilities.HashPassword("password")
                },
                new Member
                {
                    Id = 102,
                    Username = "TestM2",
                    PasswordHash = HashingUtilities.HashPassword("password")
                },
                new Member
                {
                    Id = 103,
                    Username = "TestM3",
                    PasswordHash = HashingUtilities.HashPassword("password")
                },
                new Member
                {
                    Id = 104,
                    Username = "TestM4",
                    PasswordHash = HashingUtilities.HashPassword("password")
                }
            };
            _context.Setup(m => m.Members).ReturnsDbSet(membersList.AsQueryable());
            _context.Setup(s => s.ShoppingCarts).ReturnsDbSet(new List<ShoppingCart>().AsQueryable());
            string debug = (AddMember(new MemberPost { Password = HashingUtilities.HashPassword("AdminPass"), Role = "Administator", Username = "Admin"}).Result.ToJson());
            string debug2 =(AddMember(new MemberPost { Password = HashingUtilities.HashPassword("AdminPass"), Role = "Administator", Username = "Admin2"}).Result.ToJson());


            var p1 = new Product { Id = 1, Name = "1", Description = "1", Price = 1, IsListed = true, UnitsInStock = 1 };
            var p2 = new Product { Id = 2, Name = "2", Description = "2", Price = 2, IsListed = true, UnitsInStock = 1 };
            var product = new List<Product>
            {
                p1,p2
            };

            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", RootManagerId = 1, Products = new List<Product>{p1,p2 }, DiscountRules = new List<DiscountRule>() },
                new Store { Id = 2, Name = "Store 2", RootManagerId = 2, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() },
                new Store { Id = 3, Name = "Store 3", RootManagerId = 3, Products = new List<Product>(), DiscountRules = new List<DiscountRule>() }
            };

            _context.Setup(c => c.Stores).ReturnsDbSet(stores.AsQueryable());
            _context.Setup(p => p.Products).ReturnsDbSet(new List<Product>().AsQueryable());
            _context.Setup(m => m.StoreManagers).ReturnsDbSet(new List<StoreManager>().AsQueryable());
            _context.Setup(m => m.StorePermissions).ReturnsDbSet(new List<StorePermission>().AsQueryable());
        }
    }
}
