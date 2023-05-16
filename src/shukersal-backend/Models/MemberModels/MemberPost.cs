using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class MemberPost
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

}
