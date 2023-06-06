using HotelBackend.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;
using shukersal_backend.Utility;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace shukersal_backend.DomainLayer.Objects
{
    public class MemberObject
    {
        private MarketDbContext _context;
        public MemberObject(MarketDbContext context) {
            _context = context;
        }

        public async Task<Response<IEnumerable<Member>>> GetMembers()
        {
            if (_context.Members == null)
            {
                return Response<IEnumerable<Member>>.Error(HttpStatusCode.NotFound, "Entity set 'MemberContext.members'  is null.");
            }
            var members = await _context.Members.Include(m => m.ShoppingCart).ToListAsync();

            return Response<IEnumerable<Member>>.Success(HttpStatusCode.OK, members);
        }

        public async Task<Response<Member>> GetMember(long id)
        {
            if (_context.Members == null)
            {
                return Response<Member>.Error(HttpStatusCode.NotFound, "Member was not found");
            }
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return Response<Member>.Error(HttpStatusCode.NotFound, "Member was not found");
            }

            return Response<Member>.Success(HttpStatusCode.OK, member);
        }



        public async Task<Response<Member>> RegisterMember(RegisterPost registerData)
        {
            if (_context.Members == null)
            {
                return Response<Member>.Error(HttpStatusCode.NotFound, "\"Entity set 'MemberContext.Members'  is null.\"");
            }
            var member = new Member
            {
                Username = registerData.Username,
                PasswordHash = HashingUtilities.HashPassword(registerData.Password),
                Role = UserRoles.Member
            };
            // Create a new shopping cart and associate it with the new member
            var shoppingCart = new ShoppingCart
            {
                Member = member,
                ShoppingBaskets = new List<ShoppingBasket>()
            };

            // Add the shopping cart and member to the database
            _context.ShoppingCarts.Add(shoppingCart);
            _context.Members.Add(member);
            if (_context.SaveChangesAsync().IsFaulted)
                return Response<Member>.Error(HttpStatusCode.OK, "");
            return Response<Member>.Success(HttpStatusCode.Created, member);
        }


        public async Task<Response<LoginResponse>> LoginMember(LoginPost loginData, IConfiguration _configuration)
        {
            Member? member = _context.Members.Where(m => m.Username.Equals
            (loginData.Username)).FirstOrDefault();

            if (member == null ||
                !HashingUtilities.VerifyHashedPassword(member.PasswordHash, loginData.Password))
            {
                //return BadRequest("Wrong username or password. ");
                return Response<LoginResponse>.Error(HttpStatusCode.Unauthorized, "\"Wrong username or password.\"");
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

            var loginResponse = new LoginResponse
            {
                Member = member,
                Token = tokenString
            };
            return Response<LoginResponse>.Success(HttpStatusCode.Created, loginResponse);
        }



        public async Task<Response<Member>> AddMember(MemberPost memberData)
        {
            if (_context.Members == null)
            {
                return Response<Member>.Error(HttpStatusCode.NotFound, "\"Entity set 'MemberContext.Members'  is null.\"");
            }
            var member = new Member
            {
                Username = memberData.Username,
                PasswordHash = HashingUtilities.HashPassword(memberData.Password),
                Role = memberData.Role
            };
            // Create a new shopping cart and associate it with the new member
            var shoppingCart = new ShoppingCart
            {
                Member = member,
                ShoppingBaskets = new List<ShoppingBasket>()
            };

            // Add the shopping cart and member to the database
            _context.ShoppingCarts.Add(shoppingCart);
            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return Response<Member>.Success(HttpStatusCode.Created, member);
        }



        public async Task<Response<Member>> AddAdmin(AdminPost adminData)
        {
            if (_context.Members == null)
            {
                return Response<Member>.Error(HttpStatusCode.NotFound, "\"Entity set 'MemberContext.Members'  is null.\"");
            }
            var member = new Member
            {
                Username = adminData.Username,
                PasswordHash = HashingUtilities.HashPassword(adminData.Password),
                Role = "Administrator"
            };
            // Create a new shopping cart and associate it with the new member
            var shoppingCart = new ShoppingCart
            {
                Member = member,
                ShoppingBaskets = new List<ShoppingBasket>()
            };

            // Add the shopping cart and member to the database
            _context.ShoppingCarts.Add(shoppingCart);
            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return Response<Member>.Success(HttpStatusCode.Created, member);
        }


        public async Task<Response<bool>> DeleteMember(long id)
        {
            if (_context.Members == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Entity set 'Members' is null.");
            }
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return Response<bool>.Error(HttpStatusCode.NotFound, "Member is not found");
            }
            var shoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.MemberId == id);
            if (shoppingCart != null)
            {
                _context.ShoppingCarts.Remove(shoppingCart); // remove ShoppingCart entity
            }
            _context.Members.Remove(member);

            await _context.SaveChangesAsync();

            return Response<bool>.Success(HttpStatusCode.NoContent, true);
        }




        private bool MemberExists(long id)
        {
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<Response<Member>> Logout(long id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return Response<Member>.Error(HttpStatusCode.NotFound, "Member was not found");
            }
            return Response<Member>.Success(HttpStatusCode.OK,member);
        }
    }
}
