using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class GetByIdRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
