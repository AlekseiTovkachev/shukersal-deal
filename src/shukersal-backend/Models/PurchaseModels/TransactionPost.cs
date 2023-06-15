using Newtonsoft.Json;
using shukersal_backend.Models.PurchaseModels;
using System.ComponentModel.DataAnnotations;


namespace shukersal_backend.Models
{
    public class TransactionPost
    {  
        public bool IsMember { get; set; }
        public long MemberId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public PaymentDetails BillingDetails { get; set; }

        [Required]
        public DeliveryDetails DeliveryDetails { get; set; }

        public virtual ICollection<TransactionItemPost>? TransactionItems { get; set; }

        public double TotalPrice { get; set; }



    }

    }
