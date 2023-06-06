using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models.MemberModels
{
    public class AdminPost
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(6)]
        public string Password { get; set; }

    }
}
