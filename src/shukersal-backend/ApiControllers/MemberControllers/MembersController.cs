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
        private readonly MarketDbContext _context;

        public MembersController(MarketDbContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var members = await _context.Members.Include(m => m.ShoppingCart).ToListAsync();

            return members;
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(long id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberPost>> PostMember(MemberPost memberData)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'MemberContext.Members'  is null.");
            }
            if (ModelState.IsValid)
            {
                var member = new Member {
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

                return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
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
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var shoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.MemberId == id);
            if (shoppingCart != null)
            {
                _context.ShoppingCarts.Remove(shoppingCart); // remove ShoppingCart entity
            }
            _context.Members.Remove(member);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(long id)
        {
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
