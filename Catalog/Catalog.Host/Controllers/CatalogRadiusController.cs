using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("catalog.catalogradius")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogRadiusController : ControllerBase
    {
        private readonly ILogger<CatalogRadiusController> _logger;
        private readonly ICatalogRadiusService _catalogRadiusService;

        public CatalogRadiusController(
            ILogger<CatalogRadiusController> logger,
            ICatalogRadiusService catalogRadiusService)
        {
            _logger = logger;
            _catalogRadiusService = catalogRadiusService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody] CreateRequest request)
        {
            var result = await _catalogRadiusService.AddAsync(request.Name);
            return Ok(new AddResponse() { Id = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<AddResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddRange([FromBody] IEnumerable<CreateRequest> requests)
        {
            var result = await _catalogRadiusService.AddRangeAsync(requests.Select(r => r.Name));
            return Ok(result.Select(r => new AddResponse() { Id = r }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(DeleteResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            var result = await _catalogRadiusService.DeleteAsync(request.Id);
            return Ok(new DeleteResponse() { Success = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            var result = await _catalogRadiusService.UpdateAsync(request.Id, request.Name);
            return Ok(new UpdateResponse() { Success = result });
        }
    }
}
