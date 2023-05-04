using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models.PurchaseModels
{
    public class DeliveryDetails
    {
        [Required]
        [JsonIgnore]
        public string ReceiverFirstName { get; set; }

        [Required]
        [JsonIgnore]
        public string ReceiverLastName { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(10, MinimumLength = 10)]
        public string ReceiverPhoneNum { get; set; }

        [Required]
        [JsonIgnore]
        public string ReceiverAddress { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(7, MinimumLength = 7)]
        public string ReceiverPostalCode { get; set; }       

    }
}
