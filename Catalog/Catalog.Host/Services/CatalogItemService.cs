using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, int catalogRadiusId, string pictureFileName)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.AddAsync(name, description, price, availableStock, catalogBrandId, catalogTypeId, catalogRadiusId, pictureFileName));
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () => await _catalogItemRepository.DeleteAsync(id));
    }

    public async Task<bool> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, int catalogRadiusId, string pictureFileName)
    {
        return await ExecuteSafeAsync(async () => await _catalogItemRepository.UpdateAsync(id, name, description, price, availableStock, catalogBrandId, catalogTypeId, catalogRadiusId, pictureFileName));
    }
}