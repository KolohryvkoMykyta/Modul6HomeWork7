using Infrastructure.Services.Interfaces;
using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type, int? radius)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }

        if (radius.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, radius.Value);
        }

        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/items",
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var result = await _httpClient.SendAsync<IEnumerable<CatalogBrand>, object>($"{_settings.Value.CatalogUrl}/getBrands",
           HttpMethod.Get,
           default);

        return result.Select(b => new SelectListItem
        {
            Text = b.Brand,
            Value = b.Id.ToString()
        });
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var result = await _httpClient.SendAsync<IEnumerable<CatalogType>, object>($"{_settings.Value.CatalogUrl}/getTypes",
           HttpMethod.Get,
           default);

        return result.Select(t => new SelectListItem
        {
            Text = t.Type,
            Value = t.Id.ToString()
        });
    }

    public async Task<IEnumerable<SelectListItem>> GetRadiuses()
    {
        var result = await _httpClient.SendAsync<IEnumerable<CatalogRadius>, object>($"{_settings.Value.CatalogUrl}/getRadiuses",
           HttpMethod.Get,
           default);

        return result.Select(t => new SelectListItem
        {
            Text = t.Radius,
            Value = t.Id.ToString()
        });
    }
}
