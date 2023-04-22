using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class ChangePasswordPost
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
