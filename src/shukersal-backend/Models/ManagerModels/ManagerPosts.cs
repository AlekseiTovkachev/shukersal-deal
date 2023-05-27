using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class OwnerManagerPost
    {
        //[Required]
        //public long AppointerId { get; set; }
        [Required]
        public long BossId { get; set; }
        [Required]
        public long MemberId { get; set; }
        [Required]
        public long StoreId { get; set; }
        [Required]
        public bool Owner { get; set; }
    }

    //public class FounderPost
    //{
    //    [Required]
    //    public long MemberId { get; set; }
    //    [Required]
    //    public long StoreId { get; set; }
    //}

    public class PermissionsPost
    {
        [Required]
        public long AppointerId { get; set; }
        [Required]
        public long TargetId { get; set; }
        [Required]
        public long StoreId { get; set; }
        [Required]
        public int PermissionType { get; set; }
    }
}
