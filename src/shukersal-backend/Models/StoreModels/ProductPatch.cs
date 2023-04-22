using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models
{
    public class ProductPatch
    {

        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string? Description { get; set; }
        [Range(-1, double.MaxValue)]
        public double Price { get; set; } = -1;
        [Range(-1, int.MaxValue)]
        public int UnitsInStock { get; set; } = -1;
        [Url]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; } = -1;

        public ProductPatch()
        {

        }

    }
}

