using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogbrand")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(
        ILogger<CatalogBrandController> logger,
        ICatalogBrandService catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add([FromBody] CreateRequest request)
    {
        var result = await _catalogBrandService.AddAsync(request.Name);
        return Ok(new AddResponse() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<AddResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddRange([FromBody] IEnumerable<CreateRequest> requests)
    {
        var result = await _catalogBrandService.AddRangeAsync(requests.Select(r => r.Name));
        return Ok(result.Select(r => new AddResponse() { Id = r }));
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeleteResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
    {
        var result = await _catalogBrandService.DeleteAsync(request.Id);
        return Ok(new DeleteResponse() { Success = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromBody] UpdateRequest request)
    {
        var result = await _catalogBrandService.UpdateAsync(request.Id, request.Name);
        return Ok(new UpdateResponse() { Success = result });
    }
}