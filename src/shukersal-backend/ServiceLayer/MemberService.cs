﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Utility;

namespace shukersal_backend.ServiceLayer
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MemberService : ControllerBase
    {

        private readonly MemberController memberController;

        public MemberService(MarketDbContext context)
        {
            memberController = new MemberController(context);
        }

        // GET: api/Members
        [HttpGet]
        [Route("GetMembers")]
        public async Task<ActionResult<IEnumerable<Models.Member>>> GetMembers()
        {
            var response = await memberController.GetMembers();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        //[Route("getMember")]
        public async Task<ActionResult<Models.Member>> GetMember(long id)
        {
            var response = await memberController.GetMember(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        [Route("AddMember")]
        public async Task<ActionResult<MemberPost>> AddMember(MemberPost memberData)
        {
            if (ModelState.IsValid)
            {
                var response = await memberController.AddMember(memberData);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(ModelState);
                }
                var member = response.Result;
                return CreatedAtAction("GetMember", new { id = member.Id }, member);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
        public async Task<IActionResult> DeleteMember(long id)
        {
            var response = await memberController.DeleteMember(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }

        public async Task<ActionResult<Member>> Logout(long id)
        {
            var response = await memberController.Logout(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
