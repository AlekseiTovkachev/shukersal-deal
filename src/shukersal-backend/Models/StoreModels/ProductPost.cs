using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class ProductPost
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; } = 0;
        [Range(0, int.MaxValue)]
        public int UnitsInStock { get; set; } = 0;
        [Url]
        public string? ImageUrl { get; set; } = "https://cdn-icons-png.flaticon.com/512/9458/9458302.png";
        [Required]
        public bool IsListed { get; set; } // Indicates if the product is available for normal Transaction

        //public virtual Category? Category { get; set; }
        public int CategoryId { get; set; }

        public ProductPost()
        {

        }

    }
}

