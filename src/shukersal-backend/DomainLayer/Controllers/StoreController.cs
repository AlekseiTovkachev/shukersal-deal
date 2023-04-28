using Microsoft.EntityFrameworkCore;
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

        public StoreController(MarketDbContext context) : base(context) {
            _marketObject = new MarketObject(context);
            _storeObject = new StoreObject(context, _marketObject);
        }

        public async Task<Response<IEnumerable<Store>>> GetStores()
        {
            return await _marketObject.GetStores();
        }


        public async Task<Response<Store>> GetStore(long storeId)
        {
            return await _marketObject.GetStore(storeId);
        }

        public async Task<Response<Store>> CreateStore(StorePost storeData)
        {
            return await _marketObject.CreateStore(storeData);
        }

        public async Task<Response<bool>> UpdateStore(long storeId, StorePatch patch)
        {
            return await _marketObject.UpdateStore(storeId, patch);
        }

        public async Task<Response<bool>> DeleteStore(long storeId)
        {
            return await _marketObject.DeleteStore(storeId);
        }

        public async Task<Response<Product>> AddProduct(long storeId, ProductPost post)
        {
            return await _storeObject.AddProduct(storeId, post);
        }

        public async Task<Response<Product>> UpdateProduct(long storeId, long productId, ProductPatch patch)
        {
            return await _storeObject.UpdateProduct(storeId, productId, patch);
        }

        public async Task<Response<Product>> DeleteProduct(long storeId, long productId)
        {
            return await _storeObject.DeleteProduct(storeId, productId);
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

