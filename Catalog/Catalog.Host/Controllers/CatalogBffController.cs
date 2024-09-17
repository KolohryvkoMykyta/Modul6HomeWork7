using Catalog.Host.Configurations;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;
    private readonly ICatalogBrandService _brandService;
    private readonly ICatalogTypeService _typeService;
    private readonly ICatalogRadiusService _radiusService;
    private readonly IOptions<CatalogConfig> _config;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService,
        ICatalogBrandService brandService,
        ICatalogTypeService typeService,
        IOptions<CatalogConfig> config,
        ICatalogRadiusService catalogRadiusService)
    {
        _logger = logger;
        _catalogService = catalogService;
        _brandService = brandService;
        _typeService = typeService;
        _radiusService = catalogRadiusService;
        _config = config;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetByIdResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetItemById([FromBody] GetByIdRequest request)
    {
        var result = await _catalogService.GetItemByIdAsync(request.Id);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetByIdResponse<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBrandById([FromBody] GetByIdRequest request)
    {
        var result = await _brandService.GetByIdAsync(request.Id);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetByIdResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypeById([FromBody] GetByIdRequest request)
    {
        var result = await _typeService.GetByIdAsync(request.Id);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GetByIdResponse<CatalogRadiusDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetRadiusById([FromBody] GetByIdRequest request)
    {
        var result = await _radiusService.GetByIdAsync(request.Id);
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBrands()
    {
        var result = await _brandService.GetAllBrandsAsync();
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypes()
    {
        var result = await _typeService.GetAllTypeAsync();
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CatalogRadiusDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetRadiuses()
    {
        var result = await _radiusService.GetAllRadiusesAsync();
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeQuantity(ChangeQuantityRequest id)
    {
        await _catalogService.ChangeQuantityAsync(id.Id);
        return Ok();
    }
}