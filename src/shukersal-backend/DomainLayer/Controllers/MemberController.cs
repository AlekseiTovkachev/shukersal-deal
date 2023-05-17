using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using shukersal_backend.DomainLayer.Objects;
using shukersal_backend.Models.MemberModels;

namespace shukersal_backend.DomainLayer.Controllers
{
    //TODO: move the logic to the MemberObject
    public class MemberController : AbstractController
    {

        private MemberObject _memberObject;
        public MemberController(MarketDbContext context) : base(context) { 
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



        public async Task<Response<Member>> AddMember(MemberPost memberData)
        {
            return await _memberObject.AddMember(memberData);
        }


        public async Task<Response<Member>> RegisterMember(RegisterPost registerData)
        {
            return await _memberObject.RegisterMember(registerData);
        }


        public async Task<Response<LoginResponse>> LoginMember(LoginPost loginData, IConfiguration _configuration)
        {
            return await _memberObject.LoginMember(loginData, _configuration);
        }


        // DELETE: api/Members/5
        [HttpDelete("{id}")]
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

