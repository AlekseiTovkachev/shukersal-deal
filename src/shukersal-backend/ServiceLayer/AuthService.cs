using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;
using shukersal_backend.Utility;
using System.Data;
using System.Security.Claims;

namespace shukersal_backend.ServiceLayer
{
    // TODO: Move logic to domain
    [Route("api/auth")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AuthService : ControllerBase
    {
        private readonly MarketDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly MemberController memberController;
        private readonly ILogger<ControllerBase> logger;
        public AuthService(IConfiguration configuration, MarketDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            memberController = new MemberController(context);
            this.logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginPost loginRequest, IConfiguration _configuration)
        {
            logger.LogInformation("LogInRequestCalled With {USERNAME}", loginRequest.Username);
            if (ModelState.IsValid)
            {
                var response = await memberController.LoginMember(loginRequest, _configuration);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
                }
                var loginResponse = response.Result;
                return Ok(loginResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }


        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<RegisterPost>> Register(RegisterPost registerRequest)
        {
            logger.LogInformation("RequestRequestCalled With {USERNAME}", registerRequest.Username);
            if (ModelState.IsValid)
            {
                var response = await memberController.RegisterMember(registerRequest);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(ModelState);
                }
                var member = response.Result;
                return CreatedAtAction("GetMember", "MemberService", new { id = member.Id }, member);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }




        [HttpGet]
        [Route("GetLoggedUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AllGroup)]
        public async Task<ActionResult<Member>> GetLoggedUser()
        {
            logger.LogInformation("GetLoggedCalled");
            string? username = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null)
            {
                return BadRequest("Authentication error. ");
            }
            Member? user = _context.Members.Where(user => user.Username.Equals
            (username)).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Authentication error. ");
            }
            return user;
        }


        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AllGroup)]
        public async Task<ActionResult<Member>> ChangePassword(ChangePasswordPost changePasswordRequest)
        {
            logger.LogInformation("ChangePasswordCalled");
            string? username = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null)
            {
                return BadRequest("Authentication error. ");
            }
            Member? member = _context.Members.Where(m => m.Username.Equals
            (username)).FirstOrDefault();

            if (member == null ||
                !HashingUtilities.VerifyHashedPassword(member.PasswordHash, changePasswordRequest.OldPassword))
            {
                return BadRequest("Wrong password. ");
            }
            member.PasswordHash = HashingUtilities.HashPassword(changePasswordRequest.NewPassword);
            _context.Entry(member).Property(u => u.Username).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(member.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool UserExists(long id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
