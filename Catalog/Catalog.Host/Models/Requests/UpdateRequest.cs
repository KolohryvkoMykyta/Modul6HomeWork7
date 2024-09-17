using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class UpdateRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} field must be {1} characters or shorter.")]
        public string Name { get; set; } = null!;
    }
}
