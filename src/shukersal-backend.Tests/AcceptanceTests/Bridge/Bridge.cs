using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace shukersal_backend.Tests.AcceptanceTests
{
    internal class Bridge : IBridge
    {
        private readonly AuthService authService;
        //private readonly StoreManagersController storeManagersController; //TODO: rename me
        private readonly MemberService memberService;
        private readonly PurchaseService purchaseService;
        private readonly ShoppingCartService shoppingCartService;
        private readonly StoreService storeService;
        public readonly Mock<MarketDbContext> _context;
        public Bridge() {
            
            _context = new Mock<MarketDbContext>();
            //_context.Setup(c => c.Database.EnsureCreated()).Returns(true);

            //TODO: init configuration
            authService = new AuthService(null, _context.Object);
            memberService = new MemberService(_context.Object);
            purchaseService = new PurchaseService(_context.Object);
            shoppingCartService = new ShoppingCartService(_context.Object);
            storeService = new StoreService(_context.Object);
        }
        //Member
        public async Task<ActionResult<IEnumerable<Models.Member>>> GetMembers() 
        {
            return await memberService.GetMembers();
        }
        public async Task<ActionResult<Models.Member>> GetMember(long id)
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
        public async Task<IActionResult> AddItemToCart(long id, [FromBody] ShoppingItem item)
        {
            return await shoppingCartService.AddItemToCart(id, item);
        }
        public async Task<IActionResult> RemoveItemFromCart(long id, long itemId)
        {
            return await shoppingCartService.RemoveItemFromCart(id, itemId);
        }
        //Purchase
        public async Task<ActionResult<IEnumerable<Models.Purchase>>> GetPurchases()
        {
            return await purchaseService.GetPurchases();
        }
        public async Task<ActionResult<Models.Purchase>> GetPurchase(long PurchaseId)
        {
            return await purchaseService.GetPurchase(PurchaseId);
        }
        public async Task<ActionResult<Models.Purchase>> PurchaseAShoppingCart(PurchasePost purchasePost)
        {
            return await purchaseService.PurchaseAShoppingCart(purchasePost);
        }
        public async Task<IActionResult> DeletePurchase(long purchaseId)
        {
            return await purchaseService.DeletePurchase(purchaseId);
        }
        public async Task<IActionResult> UpdatePurchase(long purchaseid, PurchasePost post)
        {
            return await purchaseService.UpdatePurchase(purchaseid, post);
        }
        public async Task<ActionResult<Models.Purchase>> BrowesePurchaseHistory(long memberId)
        {
            return await purchaseService.BrowesePurchaseHistory(memberId);
        }
        public async Task<ActionResult<Models.Purchase>> BroweseShopPurchaseHistory(long storeId)
        {
            return await purchaseService.BroweseShopPurchaseHistory(storeId);
        }
        //Manager
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            return null;
        }
        public async Task<ActionResult<StoreManager>> GetStoreManager(long id)
        {
            return null;
        }
        public async Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {
            return null;
        }
        public async Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        {
            return null;
        }
        public async Task<IActionResult> PutStoreManager(long id, StoreManager storeManager)
        {
            return null;
        }
        public async Task<IActionResult> DeleteStoreManager(long id)
        {
            return null;
        }
        public async Task<IActionResult> AddPermissionToManager(long Id, [FromBody] PermissionType permission)
        {
            return null;
        }
        public async Task<IActionResult> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {
            return null;
        }
        //Auth
        public async Task<ActionResult> Login(LoginPost loginRequest)
        {
            return null;
        }
        public async Task<ActionResult<Member>> Register(RegisterPost registerRequest)
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
    }
}
