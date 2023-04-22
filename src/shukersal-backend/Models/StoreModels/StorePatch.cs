namespace shukersal_backend.Models
{
    public class StorePatch
    {
        //public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public long RootManagerMemberId { get; set; } //Member Id of the store founder (currently cannot be changed
        //TODO: update for discount rules

    }
}
