using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;

namespace shukersal_backend.ServiceLayer
{

    [Route("api/members")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MemberService : ControllerBase
    {

        private readonly MemberController memberController;
        private readonly ILogger<ControllerBase> logger;

        public MemberService(MarketDbContext context, ILogger<ControllerBase> logger)
        {
            memberController = new MemberController(context);
            this.logger = logger;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Member>>> GetMembers()
        {
            logger.LogInformation("GetMemebers Called");
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
        public async Task<ActionResult<Models.Member>> GetMember(long id)
        {
            logger.LogInformation("LogInRequest Called With {id}", id);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
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
            logger.LogInformation("DeleteMember Called With {id}", id);
            var response = await memberController.DeleteMember(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPost]
        [Route("admins/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
        public async Task<ActionResult<MemberPost>> addAdmin(AdminPost adminData)
        {
            if (ModelState.IsValid)
            {
                var response = await memberController.AddAdmin(adminData);
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

        //[HttpPost]
        //public async Task<ActionResult<Member>> Logout(long id)
        //{
        //    var response = await memberController.Logout(id);
        //    if (!response.IsSuccess)
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}


    }
}
