using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models
{
    public class RemoveItemRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
