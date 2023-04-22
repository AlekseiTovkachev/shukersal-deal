using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class RegisterPost
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
