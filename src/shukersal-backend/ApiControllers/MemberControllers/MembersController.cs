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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.Domain;
using shukersal_backend.Models;
using shukersal_backend.Utility;

namespace shukersal_backend.Controllers.MemberControllers
{
    // TODO: Move logic to service (like in store)
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MembersController : ControllerBase
    {

        private readonly MemberService memberService;

        public MembersController(MarketDbContext context)
        {
            memberService = new MemberService(context);
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var response = await memberService.GetMembers();
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(long id)
        {
            var response = await memberService.GetMember(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<MemberPost>> AddMember(MemberPost memberData)
        {
            if (ModelState.IsValid)
            {
                var response = await memberService.AddMember(memberData);
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
            var response = await memberService.DeleteMember(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
