using Microsoft.AspNetCore.Authentication;

namespace shukersal_backend.Models.MemberModels
{
    public class LoginResponse
    {
        public Member Member { get; set; }
        public string Token { get; set; }
    }
}
