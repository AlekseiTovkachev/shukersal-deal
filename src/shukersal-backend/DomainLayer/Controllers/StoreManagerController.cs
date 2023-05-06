using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Utility;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class StoreManagerController : AbstractController
    {
        private StoreManagerObject _managerObject;

        public StoreManagerController(MarketDbContext context) : base(context)
        {

            _managerObject = new StoreManagerObject(context);

        }

        public async Task<Response<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            return await _managerObject.GetStoreManagers();
        }

        public async Task<Response<StoreManager>> GetStoreManager(long id)
        {
            return await _managerObject.GetStoreManager(id);
        }


        public async Task<Response<StoreManager>> PostStoreManager(OwnerManagerPost post)
        {
            return await _managerObject.PostStoreManager(post);
        }

        public async Task<Response<StoreManager>> PostStoreOwner(OwnerManagerPost post)
        {
            return await _managerObject.PostStoreOwner(post);
        }


        public async Task<Response<bool>> PutStoreManager(long id, StoreManager storeManager)
        {
            return await _managerObject.PutStoreManager(id, storeManager);
        }


        public async Task<Response<bool>> DeleteStoreManager(long id)
        {
            return await _managerObject.DeleteStoreManager(id);
        }

        public async Task<Response<bool>> AddPermissionToManager(long Id, [FromBody] PermissionType permission)
        {
            return await _managerObject.AddPermissionToManager(Id, permission);
        }

        public async Task<Response<bool>> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {

            return await _managerObject.RemovePermissionFromManager(id, permission);
        }


    }
}
