using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class ProductBoot
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
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string StoreName { get; set; }
    }
}
