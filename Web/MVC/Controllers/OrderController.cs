using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.OrderViewModels;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, IBasketService basketService, ILogger<OrderController> logger, ICatalogService catalogService)
        {
            _orderService = orderService;
            _basketService = basketService;
            _logger = logger;
            _catalogService = catalogService;
        }
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            if (orders == null)
            {
                return View("Error");
            }

            var vm = new OrderViewModel()
            {
                OrderList = orders
            };

            return View(vm);
        }

        public async Task<IActionResult> AddOrder(string orderProductsJson)
        {
            List<BasketItem> orderProducts = JsonConvert.DeserializeObject<List<BasketItem>>(orderProductsJson);

            await _orderService.AddOrderItem(orderProducts);

            foreach (var item in orderProducts)
            {
                await _catalogService.ChangeQuantity(item.Id);
            }

            await _basketService.ClearData();

            return RedirectToAction("GetOrders");
        }
    }
}
