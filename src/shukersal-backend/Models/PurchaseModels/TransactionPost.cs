using Newtonsoft.Json;
using shukersal_backend.Models.PurchaseModels;
using System.ComponentModel.DataAnnotations;


namespace shukersal_backend.Models
{
    public class TransactionPost
    {
        [Required]
        [JsonIgnore]
        public long MemberId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [JsonIgnore]
        public PaymentDetails BillingDetails { get; set; }

        [Required]
        [JsonIgnore]
        public DeliveryDetails DeliveryDetails { get; set; }


        [Required]
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }

        public double TotalPrice { get; set; }



    }

    }
