using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class StoreManagerTreeNode
    {
        //public long Id { get; set; }
        //public long StoreId { get; set; }
        //public long MemberId { get; set; }
        //public long ParentManagerId { get; set; }
        public StoreManager StoreManager { get; set; }
        [JsonPropertyName("subordinates")]
        public List<StoreManagerTreeNode> Subordinates { get; set; }

        //public StoreManager() { }

    }
}
