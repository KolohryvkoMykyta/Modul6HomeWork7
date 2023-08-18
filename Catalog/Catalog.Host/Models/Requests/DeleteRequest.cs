using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class DeleteRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
