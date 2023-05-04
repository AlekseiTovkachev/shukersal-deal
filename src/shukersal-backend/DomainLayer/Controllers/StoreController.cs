using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class StoreController : AbstractController
    {
        private MarketObject _marketObject;
        private StoreObject _storeObject;
        private StoreManagerObject _managerObject;

        public StoreController(MarketDbContext context) : base(context)
        {
            _marketObject = new MarketObject(context);
            _storeObject = new StoreObject(context, _marketObject);
            _managerObject = new StoreManagerObject(context);
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
                .CheckPemission(storeId, member.Id, PermissionType.Manager_permission);

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

        public async Task<Response<Product>> AddProduct(long storeId, ProductPost post, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPemission(storeId, member.Id, PermissionType.Manage_products_permission);

            if (!hasPermission)
            {
                return Response<Product>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _storeObject.AddProduct(storeId, post, member);
        }

        public async Task<Response<Product>> UpdateProduct(long storeId, long productId, ProductPatch patch, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPemission(storeId, member.Id, PermissionType.Manage_products_permission);

            if (!hasPermission)
            {
                return Response<Product>.Error(HttpStatusCode.Unauthorized, "");
            }

            return await _storeObject.UpdateProduct(storeId, productId, patch, member);
        }

        public async Task<Response<Product>> DeleteProduct(long storeId, long productId, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPemission(storeId, member.Id, PermissionType.Manage_products_permission);

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
    }
}

