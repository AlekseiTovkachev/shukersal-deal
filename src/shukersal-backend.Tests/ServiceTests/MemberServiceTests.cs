using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.ServiceLayer;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using HotelBackend.Util;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Net;


//TODO: tests are not done yet need to add more and are not runing.

namespace shukersal_backend.Tests.ServiceTests
{
    public class MemberServiceTests
    {
        private readonly Mock<MarketDbContext> _context;
        private readonly MemberController _service;
        private readonly ITestOutputHelper output;


        public MemberServiceTests(ITestOutputHelper output)
        {
            _context = new Mock<MarketDbContext>();
            _service = new MemberController(_context.Object);
            this.output = output;
        }




        [Fact]
        public async Task GetMember_ReturnsMember_WhenMemberExists()
        {
            // Arrange
            long memberId = 1;
            var member = new Member { Id = memberId, Username = "test", PasswordHash = "testhash", Role = "Member" };
            _context.Setup(c => c.Members.FindAsync(memberId)).ReturnsAsync(member);

            // Act
            var response = await _service.GetMember(memberId);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(member, response.Result);
        }

        [Fact]
        public async Task GetMember_ReturnsError_WhenMemberDoesNotExist()
        {
            // Arrange
            long memberId = 1;
            _context.Setup(c => c.Members.FindAsync(memberId)).ReturnsAsync((Member)null);

            // Act
            var response = await _service.GetMember(memberId);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddMember_ReturnsError_WhenMemberPostIsNull()
        {
            // Arrange

            // Act
            var response = await _service.AddMember(null);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


    }

}
