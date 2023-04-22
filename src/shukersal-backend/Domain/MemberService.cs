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
using shukersal_backend.Domain;
using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Domain
{
    public class MemberService : BaseService
    {
        public MemberService(MarketDbContext context) : base(context) { }

        public async Task<Response<IEnumerable<Member>>> GetMembers()
        {
            if (_context.Members == null)
            {
                return Response<IEnumerable<Member>>.Error(HttpStatusCode.NotFound, "Entity set 'MemberContext.members'  is null."); 
            }
            var members = await _context.Members.Include(m => m.ShoppingCart).ToListAsync();

            return Response<IEnumerable<Member>>.Success(HttpStatusCode.OK, members);
        }


        [HttpGet("{id}")]
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

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.AdministratorGroup)]
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

    }

}

