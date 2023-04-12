using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using shukersal_backend.Models.ShoppingCartModels;

namespace shukersal_backend.Models
{
    public class Member
    {
        public long Id { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [JsonIgnore]
        // TODO: Store as hash
        [StringLength(50)]
        public string Password { get; set; }

        [JsonIgnore]
        public virtual ShoppingCart ShoppingCart { get; set; }

        public Member(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

}
