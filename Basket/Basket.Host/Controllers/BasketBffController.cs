using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IBasketService _basketService;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddItem(AddRequest data)
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var item = new BasketItem() { Id = data.Id, Name = data.Name, Price = data.Price };
        await _basketService.AddItem(basketId!, item);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItems()
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var response = await _basketService.GetItems(basketId!);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ClearData()
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        await _basketService.ClearData(basketId!);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemoveItem(RemoveItemRequest data)
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        await _basketService.RemoveItem(basketId!, data.Id);
        return Ok();
    }
}