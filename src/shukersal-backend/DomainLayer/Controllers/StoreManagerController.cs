using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.DomainLayer.notifications;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;

namespace shukersal_backend.DomainLayer.Controllers
{
    public class StoreManagerController : AbstractController
    {
        private StoreManagerObject _managerObject;
        private readonly NotificationController _notificationController;

        public StoreManagerController(MarketDbContext context, NotificationController notificationController) : base(context)
        {

            _managerObject = new StoreManagerObject(context, notificationController);
            _notificationController = notificationController;

        }

        public async Task<Response<IEnumerable<StoreManager>>> GetStoreManagers()
        {
            return await _managerObject.GetStoreManagers();
        }


        public async Task<Response<IEnumerable<StoreManager>>> GetStoreOwnersByStoreId(long id)
        {
            return await _managerObject.GetStoreOwnersByStoreId(id);
        }


        public async Task<Response<StoreManager>> GetStoreManager(long id, Member member)
        {
            return await _managerObject.GetStoreManager(id, member);
        }
        public async Task<Response<IEnumerable<StoreManager>>> GetStoreManagersByMemberId(long id)
        {
            return await _managerObject.GetStoreManagersByMemberId(id);
        }

        public async Task<Response<StoreManager>> GetStoreManagersByStoreId(long storeId, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(storeId, member.Id, PermissionType.Get_manager_info_permission);

            if (!hasPermission)
            {
                return Response<StoreManager>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.GetStoreManagersByStoreId(storeId);
        }

        public async Task<Response<StoreManager>> PostStoreManager(OwnerManagerPost post, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Appoint_manager_permission);

            if (!hasPermission)
            {
                return Response<StoreManager>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.PostStoreManager(post, member.Id);
        }

        public async Task<Response<StoreManager>> PostStoreOwner(OwnerManagerPost post, Member member)
        {
            bool hasPermission = await _managerObject
                .CheckPermission(post.StoreId, member.Id, PermissionType.Appoint_owner_permission);

            if (!hasPermission)
            {
                return Response<StoreManager>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.PostStoreOwner(post, member.Id);
        }


        public async Task<Response<bool>> PutStoreManager(long id, StoreManager storeManager)
        {
            return await _managerObject.PutStoreManager(id, storeManager);
        }


        public async Task<Response<bool>> DeleteStoreManager(long id, Member boss)
        {

            return await _managerObject.DeleteStoreManager(id, boss);
        }

        public async Task<Response<bool>> AddPermissionToManager(long id, [FromBody] PermissionType permission, Member member)
        {
            var storeManager = (await _managerObject.GetStoreManager(id, member)).Result;
            if (storeManager == null)
                return Response<bool>.Error(HttpStatusCode.NotFound, "a manager with this id doesn't exits");
            bool hasPermission = await _managerObject
                .CheckPermission(storeManager.StoreId, member.Id, PermissionType.Edit_manager_permissions_permission);
            //.CheckPermission(id, PermissionType.Edit_manager_permissions_permission);

            if (!hasPermission)
            {
                return Response<bool>.Error(HttpStatusCode.Unauthorized, "");
            }
            return await _managerObject.AddPermissionToManager(id, permission);
        }

        public async Task<Response<bool>> RemovePermissionFromManager(long id, [FromBody] PermissionType permission, Member member)
        {
            var storeManager = (await _managerObject.GetStoreManager(id, member)).Result;
            if (storeManager == null)
                return Response<bool>.Error(HttpStatusCode.NotFound, "a manager with this id doesn't exits");
            bool hasPermission = await _managerObject
                .CheckPermission(storeManager.StoreId, member.Id, PermissionType.Edit_manager_permissions_permission);
            //.CheckPermission(id, PermissionType.Edit_manager_permissions_permission);

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
