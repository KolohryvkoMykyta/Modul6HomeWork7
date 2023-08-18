using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public class CreateRequest
{
    [Required]
    [StringLength(40, ErrorMessage = "The {0} field must be {1} characters or shorter.")]
    public string Name { get; set; } = null!;
}