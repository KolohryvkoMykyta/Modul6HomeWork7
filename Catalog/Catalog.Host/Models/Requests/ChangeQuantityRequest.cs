using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class ChangeQuantityRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
