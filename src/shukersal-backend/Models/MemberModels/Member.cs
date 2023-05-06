using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class Member
    {
        public long Id { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [Required]
        public string Role { get; set; } // Member or Administrator

        [JsonIgnore]
        public virtual ShoppingCart ShoppingCart { get; set; }
    }

}
