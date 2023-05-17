using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class StorePost
    {
        //public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        //[Required]
        //public long RootManagerMemberId { get; set; } //Member Id of the store founder

    }
}
