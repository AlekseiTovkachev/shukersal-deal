namespace shukersal_backend.Models.PurchaseModels
{
    public class PurchasePost
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalPrice { get; set; }


    }
}
