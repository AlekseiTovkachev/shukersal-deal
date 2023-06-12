using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class StoreManager
    {
        // TODO: Enforce graph legality in controller (no cycles for example)
        [Key]
        public long Id { get; set; }

        [Required]
        public long StoreId { get; set; }

        [JsonIgnore]
        [ForeignKey("StoreId")]
        [DeleteBehavior(DeleteBehavior.Cascade)]
        [Required]
        public virtual Store Store { get; set; }

        [Required]
        public long MemberId { get; set; }

        [JsonIgnore]
        [Required]
        [DeleteBehavior(DeleteBehavior.Cascade)]
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public long? ParentManagerId { get; set; }

        [JsonIgnore]
        [ForeignKey("ParentManagerId")]
        [InverseProperty("ChildManagers")]
        public virtual StoreManager? ParentManager { get; set; }

        public virtual ICollection<StorePermission>? StorePermissions { get; set; }

        [JsonIgnore]
        [InverseProperty("ParentManager")]
        public virtual ICollection<StoreManager> ChildManagers { get; set; }

        //public StoreManager() { }
    }
}
