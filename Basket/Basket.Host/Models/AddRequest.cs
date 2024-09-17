using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models;

public class AddRequest
{
    [Required] 
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
}