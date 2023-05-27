using Microsoft.Extensions.Hosting;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Models.StoreModels;
using shukersal_backend.Utility;
using System.Collections.Generic;
using System.Net;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class StoreController : AbstractController
    {
        private MarketObject _marketObject;
        private StoreObject _storeObject;
        private StoreManagerObject _managerObject;
        private DiscountObject _discountObject;
        private PurchaseRuleObject _purchaseRuleObject;

        public StoreController(MarketDbContext context) : base(context)
        {
            _marketObject = new MarketObject(context);
            _managerObject = new StoreManagerObject(context);
            _storeObject = new StoreObject(context, _marketObject, _managerObject);
            _purchaseRuleObject = new PurchaseRuleObject(context);
        }

        //Constructor for tests
        public StoreController(MarketDbContext context,
            MarketObject marketObject, StoreObject storeObject, StoreManagerObject managerObject) : base(context)
        {
            _marketObject = marketObject;
            _storeObject = storeObject;
            _managerObject = managerObject;
        }

        public async Task<Response<IEnumerable<Store>>> GetStores()
        {
            return await _marketObject.GetStores();
        }


        public async Task<Response<Store>> GetStore(long storeId)
        {
            return await _marketObject.GetStore(storeId);
        }

        public async Task<Response<Store>> CreateStore(StorePost storeData, Member member)
        {
            return await _marketObject.CreateStore(storeData, member);
        }

        public async Task<Response<bool>> UpdateStore(long storeId, StorePatch patch, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(storeId, member.Id, PermissionType.Manager_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }


            return await _marketObject.UpdateStore(storeId, patch, member);
        }

        public async Task<Response<bool>> DeleteStore(long storeId, Member member)
        {
            return await _marketObject.DeleteStore(storeId, member);
        }

        public async Task<Response<Product>> GetProduct(long productId)
        {
            return await _storeObject.GetProduct(productId);
        }

        public async Task<Response<Product>> AddProduct(long storeId, ProductPost post, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(storeId, member.Id, PermissionType.Manage_products_permission);

            if (!hasPermission)
            {
                return Response<Product>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _storeObject.AddProduct(storeId, post, member);
        }

        public async Task<Response<Product>> UpdateProduct(long storeId, long productId, ProductPatch patch, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(storeId, member.Id, PermissionType.Manage_products_permission);

            if (!hasPermission)
            {
                return Response<Product>.Error(HttpStatusCode.Unauthorized, "");
            }

            return await _storeObject.UpdateProduct(storeId, productId, patch, member);
        }

        public async Task<Response<Product>> DeleteProduct(long storeId, long productId, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(storeId, member.Id, PermissionType.Manage_products_permission);

            if (!hasPermission)
            {
                return Response<Product>.Error(HttpStatusCode.Unauthorized, "");
            }

            return await _storeObject.DeleteProduct(storeId, productId, member);
        }

        public async Task<Response<IEnumerable<Product>>> GetStoreProducts(long storeId)
        {
            return await _storeObject.GetProducts(storeId);
        }

        public async Task<Response<IEnumerable<Category>>> GetCategories()
        {
            return await _marketObject.GetCategories();
        }

        public async Task<Response<bool>> CreateDiscount(DiscountRulePost post, Member member)
        {
            var store = await _marketObject.GetStore(post.StoreId);
            if (store.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }


            return await _discountObject.CreateDiscount(post, store.Result);
        }

        public async Task<Response<bool>> CreateChildDiscount(long compositeId, DiscountRulePost post, Member member)
        {
            bool hasPermission = await _managerObject
                 .CheckPermission(post.StoreId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }

            return await _discountObject.CreateChildDiscount(compositeId, post);
        }

        public async Task<Response<bool>> CreateDiscountRuleBoolean(DiscountRuleBooleanPost post, Member member, long compositeId)
        {
            var store = await GetStore(post.StoreId);
            var rule = _context.DiscountRules.Where(d => d.Id == compositeId).FirstOrDefault();
            if (store.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }
            if (rule == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "discount rule not found");
            }
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }


            return await _discountObject.CreateDiscountRuleBoolean(post, rule);
        }

        public async Task<Response<bool>> CreateChildDiscountRuleBoolean(long compositeId, DiscountRuleBooleanPost post, Member member)
        {
            bool hasPermission = await _managerObject
                 .CheckPermission(post.StoreId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }

            return await _discountObject.CreateChildDiscountRuleBoolean(compositeId, post);
        }

        public async Task<Response<ICollection<DiscountRule>>> GetDiscounts(long storeId)
        {
            return await _discountObject.GetDiscounts(storeId);
        }

        public async Task<Response<DiscountRule>> GetAppliedDiscount(long storeId)
        {

            return await _discountObject.GetAppliedDiscount(storeId);
        }

        public async Task<Response<DiscountRule>> SelectDiscount(long storeId ,long discountId, Member member)
        {
            bool hasPermission = await _managerObject
                 .CheckPermission(storeId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<DiscountRule>.Error(HttpStatusCode.Unauthorized, "");
            }

            var store = await GetStore(storeId);
            return await _discountObject.SelectDiscount(store.Result, discountId);
        }

        public async Task<Response<ICollection<PurchaseRule>>> GetPurchaseRules(long storeId)
        {

            return await _purchaseRuleObject.GetPurchaseRules(storeId);
        }

        public async Task<Response<PurchaseRule>> GetAppliedPurchaseRule(long storeId)
        {

            return await _purchaseRuleObject.GetAppliedPurchaseRule(storeId);
        }

        public async Task<Response<PurchaseRule>> SelectPurchaseRule(long storeId, long discountId, Member member)
        {
            bool hasPermission = await _managerObject
                 .CheckPermission(storeId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<PurchaseRule>.Error(HttpStatusCode.Unauthorized, "");
            }

            var store = await GetStore(storeId);
            return await _purchaseRuleObject.SelectPurchaseRule(store.Result, discountId);
        }

        public async Task<Response<bool>> CreatePurchaseRule(PurchaseRulePost post, Member member)
        {
            var store = await _marketObject.GetStore(post.StoreId);
            if (store.Result == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Store not found");
            }
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }


            return await _purchaseRuleObject.CreatePurchaseRule(post, store.Result);
        }

        public async Task<Response<bool>> CreateChildPurchaseRule(long compositeId, PurchaseRulePost post, Member member)
        {
            bool hasPermission = await _managerObject
                 .CheckPermission(post.StoreId, member.Id, PermissionType.Manage_discounts_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }

            return await _purchaseRuleObject.CreateChildPurchaseRule(compositeId, post);
        }
    }
}

