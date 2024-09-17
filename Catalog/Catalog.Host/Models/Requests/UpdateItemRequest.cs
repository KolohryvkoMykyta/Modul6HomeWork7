using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class UpdateItemRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} field must be {1} characters or shorter.")]
        public string Name { get; set; } = null!;

        [StringLength(100, ErrorMessage = "The {0} field must be {1} characters or shorter.")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(1, 5000)]
        public decimal Price { get; set; }

        [Url]
        public string PictureFileName { get; set; } = null!;

        [Required]
        public int CatalogTypeId { get; set; }

        [Required]
        public int CatalogBrandId { get; set; }

        [Required]
        public int CatalogRadiusId { get; set; }

        public int AvailableStock { get; set; }
    }
}
