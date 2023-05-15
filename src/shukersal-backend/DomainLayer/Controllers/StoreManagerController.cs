using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

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
        public async Task<Response<IEnumerable<StoreManager>>> GetStoreManagersByMemberId(long id)
        {
            return await _managerObject.GetStoreManagersByMemberId(id);
        }


        public async Task<Response<StoreManager>> PostStoreManager(OwnerManagerPost post, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Appoint_manager_permission);

            if (!hasPermission)
            {
                return Response<StoreManager>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.PostStoreManager(post);
        }

        public async Task<Response<StoreManager>> PostStoreOwner(OwnerManagerPost post, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Appoint_owner_permission);

            if (!hasPermission)
            {
                return Response<StoreManager>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.PostStoreOwner(post);
        }


        public async Task<Response<bool>> PutStoreManager(long id, StoreManager storeManager)
        {
            return await _managerObject.PutStoreManager(id, storeManager);
        }


        public async Task<Response<bool>> DeleteStoreManager(long id)
        {
            //bool hasPermission = await _managerObject
            //    .CheckPermission(post.StoreId, member.Id, PermissionType.);

            //if (!hasPermission)
            //{
            //    return Response<StoreManager>.Error(HttpStatusCode.Unauthorized, "");
            //}
            return await _managerObject.DeleteStoreManager(id);
        }

        public async Task<Response<bool>> AddPermissionToManager(long id, [FromBody] PermissionType permission, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(id, PermissionType.Edit_manager_permissions_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.AddPermissionToManager(id, permission);
        }

        public async Task<Response<bool>> RemovePermissionFromManager(long id, [FromBody] PermissionType permission)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(id, PermissionType.Edit_manager_permissions_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.RemovePermissionFromManager(id, permission);
        }

        public async Task<Response<IEnumerable<Store>>> GetManagedStoresByMemberId(long memberId)
        {
            return await _managerObject.GetManagedStoresByMemberId(memberId);
        }


    }
}
