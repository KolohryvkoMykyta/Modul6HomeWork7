using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Configurations;
using Order.Host.Models.Dtos;
using Order.Host.Models.Requests;
using Order.Host.Models.Response;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderBffController : ControllerBase
    {
        private readonly ILogger<OrderBffController> _logger;
        private readonly IOrderService _orderService;
        private readonly IOptions<OrderConfig> _config;

        public OrderBffController(
            ILogger<OrderBffController> logger,
            IOrderService orderService,
            IOptions<OrderConfig> config)
        {
            _logger = logger;
            _orderService = orderService;
            _config = config;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddOrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddOrder(AddOrderRequests data)
        {
            _logger.LogInformation($"Request: {data.Data.Count()}");
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var result = await _orderService.AddOrderAsync(userId, data.Data);

            return Ok(new AddOrderResponse() { Id = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetOrdersResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var result = await _orderService.GetOrdersAsync(userId);

            return Ok(new GetOrdersResponse() { Data = result });
        }
    }
}
