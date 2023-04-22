using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace shukersal_backend.ApiControllers.MemberControllers
{
    // TODO: Move logic to MembersService and AuthService
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AuthController : ControllerBase
    {
        private readonly MarketDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, MarketDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginPost loginRequest)
        {
            Member? member = _context.Members.Where(m => m.Username.Equals
            (loginRequest.Username)).FirstOrDefault();

            if (member == null ||
                !HashingUtilities.VerifyHashedPassword(member.PasswordHash, loginRequest.Password))
            {
                return BadRequest("Wrong username or password. ");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, member.Username),
                //new Claim(ClaimTypes.Email, member.Email),
                new Claim(ClaimTypes.Role, member.Role)
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenString);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<Member>> Register(RegisterPost registerRequest)
        {
            // TODO: Move code logic to MembersService, wrap with error handlers like in store service.

            Member member = new Member();
            member.Username = registerRequest.Username;
            member.PasswordHash = HashingUtilities.HashPassword(registerRequest.Password);
            member.Role = UserRoles.Member;

            // Verify the user does not exist
            Member? existingUsername = _context.Members.Where(u => u.Username.Equals
            (member.Username)).FirstOrDefault();
            if (existingUsername != null)
            {
                return BadRequest("A user with that name already exists.");
            }

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(actionName: "GetUser", controllerName: "Users", new { id = member.Id }, member);
        }
        [HttpGet]
        [Route("GetLoggedUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AllGroup)]
        public async Task<ActionResult<Member>> GetLoggedUser()
        {
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
