using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class StoreManager
    {
        // TODO: Enforce graph legality in controller (no cycles for example)
        public long Id { get; set; }
        [ForeignKey("Store")]
        public long StoreId { get; set; }
        [JsonIgnore]
        public Store Store { get; set; }

        [ForeignKey("Member")]
        public long MemberId { get; set; }
        [JsonIgnore]
        public Member? Member { get; set; }

        [ForeignKey("ParentManager")]
        public long ParentManagerId { get; set; }
        [JsonIgnore]
        public StoreManager? ParentManager { get; set; }

        public ICollection<StorePermission>? StorePermissions { get; set; }

        public StoreManager() { }

    }
}
