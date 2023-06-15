using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace shukersal_backend.Models
{
    public class Transaction
    {
        [Key]
        public long Id { get; set; }
        public bool IsMember { get; set; }
        [Required]
        public long MemberId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TotalPrice { get; set; }
        public virtual ICollection<TransactionItem>? TransactionItems { get; set; }


    }
}
