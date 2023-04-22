using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace shukersal_backend.Models
{
    public class PurchasePost
    {
        //public long Id { get; set; }
        [Required]
        [JsonIgnore]
        public long Member__ID { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public double TotalPrice { get; set; }

        //payment details
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
        public DateOnly expirationDate { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(3, MinimumLength = 3)]
        public string CVC { get; set; }

        //delivery details
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
