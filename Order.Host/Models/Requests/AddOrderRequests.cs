using System.ComponentModel.DataAnnotations;
using Order.Host.Models.Dtos;

namespace Order.Host.Models.Requests
{
    public class AddOrderRequests
    {
        [Required]
        public List<OrderProductDto> Data { get; set; } = null!;
    }
}
