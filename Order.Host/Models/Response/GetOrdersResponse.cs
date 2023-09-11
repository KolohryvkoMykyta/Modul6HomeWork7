using Order.Host.Models.Dtos;

namespace Order.Host.Models.Response
{
    public class GetOrdersResponse
    {
        public List<OrderDto> Data { get; set; } = null!;
    }
}
