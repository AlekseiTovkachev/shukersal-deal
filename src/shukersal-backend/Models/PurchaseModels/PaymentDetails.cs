using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models.PurchaseModels
{
    public class PaymentDetails
    {
        [Required]
        [JsonIgnore]
        public string HolderFirstName { get; set; }

        [Required]
        [JsonIgnore]
        public string HolderLastName { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(9, MinimumLength = 9)]
        public string HolderID { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(19, MinimumLength = 16)]
        public string CardNumber { get; set; }

        [Required]
        [JsonIgnore]
        public DateOnly ExpirationDate { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(3, MinimumLength = 3)]
        public string CVC { get; set; }
        public double TotalPrice { get; set; }

    }
}
