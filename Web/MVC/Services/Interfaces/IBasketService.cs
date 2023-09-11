using MVC.ViewModels;

namespace MVC.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddBasketItem(int id, string name, decimal price);
        Task RemoveBasketItem(int id);
        Task<IReadOnlyList<BasketItem>> GetBasketItems();
        Task ClearData();
    }
}
