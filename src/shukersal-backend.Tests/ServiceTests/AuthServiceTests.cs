using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using shukersal_backend.DomainLayer.Controllers;
using shukersal_backend.Models;
using shukersal_backend.Models.MemberModels;
using shukersal_backend.ServiceLayer;
using Xunit;
using shukersal_backend.Utility;


//TODO: tests are not done yet need to add more and are not runing.


namespace shukersal_backend.Tests.ServiceLayer
{
    public class AuthServiceTests
    {
        private readonly Mock<MarketDbContext> _contextMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<MemberController> _memberControllerMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _contextMock = new Mock<MarketDbContext>();
            _configurationMock = new Mock<IConfiguration>();
            _memberControllerMock = new Mock<MemberController>(_contextMock.Object);
            _authService = new AuthService(_configurationMock.Object, _contextMock.Object);
        }

        [Fact]
        public async Task Login_ValidLoginRequest_ReturnsOkObjectResultWithLoginResponse()
        {
            // Arrange
            var loginRequest = new LoginPost
            {
                Username = "testuser",
                Password = "password"
            };
            var loginResponse = new LoginResponse
            {
                Member = new Member
                {
                    Id = 1,
                    Username = "testuser",
                    PasswordHash = "hashedpassword",
                    Role = "Member"
                },
                Token = "token"
            };
            _memberControllerMock.Setup(x => x.LoginMember(loginRequest, _configurationMock.Object)).ReturnsAsync(new Response<LoginResponse>
            {
                IsSuccess = true,
                Result = loginResponse
            });

            // Act
            var result = await _authService.Login(loginRequest, _configurationMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var loginResult = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal(loginResponse, loginResult);
        }

        [Fact]
        public async Task Login_InvalidLoginRequest_ReturnsBadRequestObjectResultWithModelState()
        {
            // Arrange
            var loginRequest = new LoginPost();
            _authService.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _authService.Login(loginRequest, _configurationMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var modelStateResult = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelStateResult.ContainsKey("Username"));
            Assert.Equal(new[] { "Username is required" }, (string[])modelStateResult["Username"]);
        }



    }
}
