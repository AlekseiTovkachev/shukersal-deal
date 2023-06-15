using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models.PurchaseModels
{
    public class PaymentDetails
    {
        [Required]

        public string HolderFirstName { get; set; }

        [Required]

        public string HolderLastName { get; set; }

        [Required]

        [StringLength(9, MinimumLength = 9)]
        public string HolderID { get; set; }

        [Required]

        [StringLength(19, MinimumLength = 16)]
        public string CardNumber { get; set; }

        [Required]  //only month & year need to be presented 
        public DateOnly ExpirationDate { get; set; }

        [Required]

        [StringLength(3, MinimumLength = 3)]
        public string CVC { get; set; }

        [JsonIgnore]
        public double TotalPrice { get; set; } 

    }
}
