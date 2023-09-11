using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type, int? radius);
    Task<IEnumerable<SelectListItem>> GetBrands();
    Task<IEnumerable<SelectListItem>> GetTypes();
    Task<IEnumerable<SelectListItem>> GetRadiuses();
    Task<CatalogItem> GetItemById(int id);
    Task ChangeQuantity(int id);
}
