using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.BasketViewModels;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IActionResult> TakeBasket()
        {
            var basket = await _basketService.GetBasketItems();
            if (basket == null)
            {
                return View("Error");
            }
            
            var vm = new BasketViewModel()
            {
                BasketItems = basket.Select(i => new BasketItem() { Id = i.Id, Name = i.Name, Price = i.Price}).ToList(),
            };

            return View(vm);
        }

        public async Task<IActionResult> AddBasketItem(int id, string name, decimal price)
        {
            await _basketService.AddBasketItem(id, name, price);

            return RedirectToAction("Product", "Catalog", new { id = id });
        }

        public async Task<IActionResult> RemoveBasketItem(int id)
        {
            await _basketService.RemoveBasketItem(id);

            return RedirectToAction("TakeBasket");
        }

        public async Task<IActionResult> ClearBasket()
        {
            await _basketService.ClearData();

            return RedirectToAction("TakeBasket");
        }
    }
}
