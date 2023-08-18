using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog.catalogtype")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogTypeController> _logger;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeController(
        ILogger<CatalogTypeController> logger,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add([FromBody] CreateRequest request)
    {
        var result = await _catalogTypeService.AddAsync(request.Name);
        return Ok(new AddResponse() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<AddResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddRange([FromBody] IEnumerable<CreateRequest> requests)
    {
        var result = await _catalogTypeService.AddRangeAsync(requests.Select(r => r.Name));
        return Ok(result.Select(r => new AddResponse() { Id = r }));
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeleteResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
    {
        var result = await _catalogTypeService.DeleteAsync(request.Id);
        return Ok(new DeleteResponse() { Success = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromBody] UpdateRequest request)
    {
        var result = await _catalogTypeService.UpdateAsync(request.Id, request.Name);
        return Ok(new UpdateResponse() { Success = result });
    }
}