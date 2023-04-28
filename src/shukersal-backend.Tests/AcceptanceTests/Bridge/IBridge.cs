using shukersal_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shukersal_backend.Tests.AcceptanceTests
{
    internal interface IBridge
    {
        //Member
        Task<ActionResult<IEnumerable<Models.Member>>> GetMembers();
        Task<ActionResult<Models.Member>> GetMember(long id);
        Task<ActionResult<MemberPost>> AddMember(MemberPost memberData);
        Task<IActionResult> DeleteMember(long id);
        //Store
        Task<ActionResult<IEnumerable<Store>>> GetStores();
        Task<ActionResult<Store>> GetStore(long id);
        Task<ActionResult<Store>> CreateStore(StorePost storeData);
        Task<IActionResult> UpdateStore(long id, StorePatch patch);
        Task<IActionResult> DeleteStore(long id);
        Task<IActionResult> AddProduct(long storeId, ProductPost product);
        Task<IActionResult> UpdateProduct(long storeId, long productId, ProductPatch product);
        Task<IActionResult> DeleteProduct(long storeId, long productId);
        Task<ActionResult<Store>> GetStoreProducts(long storeId);
        Task<ActionResult<IEnumerable<Category>>> GetCategories();
        //Shopping Cart
        Task<ActionResult<ShoppingCart>> GetShoppingCartByUserId(long memberId);
        Task<IActionResult> AddItemToCart(long id, [FromBody] ShoppingItem item);
        Task<IActionResult> RemoveItemFromCart(long id, long itemId);
        //Purchase
        Task<ActionResult<IEnumerable<Models.Purchase>>> GetPurchases();
        Task<ActionResult<Models.Purchase>> GetPurchase(long PurchaseId);
        Task<ActionResult<Models.Purchase>> PurchaseAShoppingCart(PurchasePost purchasePost);
        Task<IActionResult> DeletePurchase(long purchaseId);
        Task<IActionResult> UpdatePurchase(long purchaseid, PurchasePost post);
        Task<ActionResult<Models.Purchase>> BrowesePurchaseHistory(long memberId);
        Task<ActionResult<Models.Purchase>> BroweseShopPurchaseHistory(long storeId);
        //Manager
        Task<ActionResult<IEnumerable<StoreManager>>> GetStoreManagers();
        Task<ActionResult<StoreManager>> GetStoreManager(long id);
        Task<ActionResult<StoreManager>> PostStoreManager(OwnerManagerPost post);
        Task<ActionResult<StoreManager>> PostStoreOwner(OwnerManagerPost post);
        Task<IActionResult> PutStoreManager(long id, StoreManager storeManager);
        Task<IActionResult> DeleteStoreManager(long id);
        Task<IActionResult> AddPermissionToManager(long Id, [FromBody] PermissionType permission);
        Task<IActionResult> RemovePermissionFromManager(long id, [FromBody] PermissionType permission);
        //Auth
        Task<ActionResult> Login(LoginPost loginRequest);
        Task<ActionResult<Member>> Register(RegisterPost registerRequest);
        Task<ActionResult<Member>> GetLoggedUser();
        Task<ActionResult<Member>> ChangePassword(ChangePasswordPost changePasswordRequest);



    }
}
