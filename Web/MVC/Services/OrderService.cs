using MVC.Models.Requests;
using MVC.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using System.Diagnostics;

namespace MVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IHttpClientService httpClient, ILogger<OrderService> logger, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> AddOrderItem(List<BasketItem> items)
        {
            var orderItems = items.Select(o => new OrderProduct() { Id = 0, ProductId = o.Id, Name = o.Name, Price = o.Price, OrderId = 0 }).ToList();

            var result = await _httpClient.SendAsync<AddOrderResponse, AddOrderRequest>($"{_settings.Value.OrderUrl}/addOrder",
           HttpMethod.Post,
           new AddOrderRequest() { Data = orderItems });

            return result.Id;
        }

        public async Task<List<Order>> GetOrders()
        {
            var result = await _httpClient.SendAsync<GetOrdersResponse, object>($"{_settings.Value.OrderUrl}/getOrders",
           HttpMethod.Post,
           default);

            if(result == null)
            {
                return new List<Order>();
            }

            return result.Data;
        }
    }
}
