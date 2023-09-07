using Basket.Host.Models;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddItem(string userId, BasketItem data);
    Task<GetResponse> GetItems(string userId);
    Task ClearData(string userId);
    Task RemoveItem(string userId, int itemId);
}