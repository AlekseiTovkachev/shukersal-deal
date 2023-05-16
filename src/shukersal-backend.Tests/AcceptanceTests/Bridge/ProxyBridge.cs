using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shukersal_backend.Models.MemberModels;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Utility;

namespace shukersal_backend.Tests.AcceptanceTests
{
    public class ProxyBridge : ControllerBase, IBridge
    {
        public ProxyBridge() { }
        //Member
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers() { return StatusCode(501); }
        public async Task<ActionResult<Member>> GetMember(long id) { return StatusCode(501); }
        public async Task<ActionResult<MemberPost>> AddMember(MemberPost memberData) { return StatusCode(501); }
        public async Task<IActionResult> DeleteMember(long id) { return StatusCode(501); }
        //Store
        public async Task<ActionResult<IEnumerable<Store>>> GetStores() { return StatusCode(501); }
        public async Task<ActionResult<Store>> GetStore(long id) { return StatusCode(501); }
        public async Task<ActionResult<Store>> CreateStore(StorePost storeData) { return StatusCode(501); }
        public async Task<IActionResult> UpdateStore(long id, StorePatch patch) { return StatusCode(501); }
        public async Task<IActionResult> DeleteStore(long id) { return StatusCode(501); }
        public async Task<IActionResult> AddProduct(long storeId, ProductPost product) { return StatusCode(501); }
        public async Task<IActionResult> UpdateProduct(long storeId, long productId, ProductPatch product) { return StatusCode(501); }
        public async Task<IActionResult> DeleteProduct(long storeId, long productId) { return StatusCode(501); }
        public async Task<ActionResult<Store>> GetStoreProducts(long storeId) { return StatusCode(501); }
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories() { return StatusCode(501); }
        //Shopping Cart
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByUserId(long memberId) { return StatusCode(501); }
        public async Task<ActionResult<ShoppingItem>> AddItemToCart(long id, [FromBody] ShoppingItem item) { return StatusCode(501); }
        public async Task<ActionResult<ShoppingItem>> RemoveItemFromCart(long id, ShoppingItem itemId) { return StatusCode(501); }
        //Transaction
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions() { return StatusCode(501); }
        public async Task<ActionResult<Transaction>> GetTransaction(long TransactionId) { return StatusCode(501); }
        public async Task<ActionResult<Transaction>> PurchaseAShoppingCart(TransactionPost TransactionPost) { return StatusCode(501); }
        public async Task<IActionResult> DeleteTransaction(long TransactionId) { return StatusCode(501); }
        public async Task<IActionResult> UpdateTransaction(long Transactionid, TransactionPost post) { return StatusCode(501); }
        public async Task<ActionResult<Transaction>> BrowseTransactionHistory(long memberId) { return StatusCode(501); }
        public async Task<ActionResult<Transaction>> BrowseShopTransactionHistory(long storeId) { return StatusCode(501); }
        //Manager
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers() { return StatusCode(501); }
        public async Task<ActionResult<StoreManager>> GetStoreManager(long id) { return StatusCode(501); }
        public async Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post) { return StatusCode(501); }
        public async Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post) { return StatusCode(501); }
        public async Task<IActionResult> PutStoreManager(long id, StoreManager storeManager) { return StatusCode(501); }
        public async Task<IActionResult> DeleteStoreManager(long id) { return StatusCode(501); }
        public async Task<IActionResult> AddPermissionToManager(long Id, [FromBody] PermissionType permission) { return StatusCode(501); }
        public async Task<IActionResult> RemovePermissionFromManager(long id, [FromBody] PermissionType permission) { return StatusCode(501); }
        //Auth
        public async Task<ActionResult<LoginResponse>> Login(LoginPost loginRequest) { return StatusCode(501); }
        public async Task<ActionResult<RegisterPost>> Register(RegisterPost registerRequest) { return StatusCode(501); }
        public async Task<ActionResult<Member>> GetLoggedUser() { return StatusCode(501); }
        public async Task<ActionResult<Member>> ChangePassword(ChangePasswordPost changePasswordRequest) { return StatusCode(501); }

        public void init()
        {
            //do nothing
        }
    }
}
