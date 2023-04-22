using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class LoginPost
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
