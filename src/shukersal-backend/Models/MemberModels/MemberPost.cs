using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.Models
{
    public class MemberPost
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
