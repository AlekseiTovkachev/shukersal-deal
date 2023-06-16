using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;
using shukersal_backend.Utility;

namespace shukersal_backend.DomainLayer.Controllers
{
    //TODO: move the logic to the MemberObject
    public class MemberController : AbstractController
    {

        private MemberObject _memberObject;
        public MemberController(MarketDbContext context) : base(context)
        {
            _memberObject = new MemberObject(context);
        }

        public async Task<Response<IEnumerable<Member>>> GetMembers()
        {
            return await _memberObject.GetMembers();
        }


        public async Task<Response<Member>> GetMember(long id)
        {
            return await _memberObject.GetMember(id);
        }

        public async Task<Response<Member>> GetMember(string username)
        {
            return await _memberObject.GetMember(username);
        }

        public async Task<Response<Member>> AddMember(MemberPost memberData)
        {
            return await _memberObject.AddMember(memberData);
        }

        public async Task<Response<Member>> AddAdmin(AdminPost adminData)
        {
            return await _memberObject.AddAdmin(adminData);
        }



        public async Task<Response<Member>> RegisterMember(RegisterPost registerData)
        {
            return await _memberObject.RegisterMember(registerData);
        }


        public async Task<Response<LoginResponse>> LoginMember(LoginPost loginData, IConfiguration _configuration)
        {
            return await _memberObject.LoginMember(loginData, _configuration);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
        public async Task<Response<bool>> DeleteMember(long id)
        {
            return await _memberObject.DeleteMember(id);
        }


        //not sure if needed
        private bool MemberExists(long id)
        {
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Response<Member>> Logout(long id)
        {
            return await _memberObject.Logout(id);
        }
    }

}

