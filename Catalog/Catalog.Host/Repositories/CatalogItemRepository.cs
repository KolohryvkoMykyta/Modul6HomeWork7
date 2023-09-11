using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Infrastructure.Exceptions;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter, int? radiusFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        if (radiusFilter.HasValue)
        {
            query = query.Where(w => w.CatalogRadiusId == radiusFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .Include(i => i.CatalogRadius)
           .Skip(pageSize * pageIndex)
           .Take(pageSize)
           .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, int radiusTypeId, string pictureFileName)
    {
        var item1 = new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            CatalogRadiusId = radiusTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price,
            AvailableStock = availableStock,
        };
        var item = await _dbContext.AddAsync(item1);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Added new item");

        return item.Entity.Id;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _dbContext.CatalogItems.FindAsync(id);

        if (result == null)
        {
            throw new BusinessException("Incorrect id");
        }

        _dbContext.CatalogItems.Remove(result);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Deleted item");

        return true;
    }

    public async Task<bool> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, int catalogRadiusId, string pictureFileName)
    {
        var updatingItem = await _dbContext.CatalogItems.FindAsync(id);

        if (updatingItem == null)
        {
            throw new BusinessException("Incorrect id");
        }

        updatingItem.CatalogBrandId = catalogBrandId;
        updatingItem.CatalogTypeId = catalogTypeId;
        updatingItem.CatalogRadiusId = catalogRadiusId;
        updatingItem.Description = description;
        updatingItem.Name = name;
        updatingItem.PictureFileName = pictureFileName;
        updatingItem.Price = price;
        updatingItem.AvailableStock = availableStock;

        _dbContext.CatalogItems.Update(updatingItem);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Updated item");

        return true;
    }

    public async Task<CatalogItem> GetItemByIdAsync(int id)
    {
        var result = await _dbContext.CatalogItems
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .Include(c => c.CatalogRadius)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (result == null)
        {
            throw new BusinessException("Incorrect Id");
        }

        return result;
    }

    public async Task ChangeQuantity(int id)
    {
        var updatingItem = await _dbContext.CatalogItems.FindAsync(id);

        if (updatingItem == null)
        {
            throw new BusinessException("Incorrect id");
        }

        if (updatingItem.AvailableStock > 0)
        {
            updatingItem.AvailableStock -= 1;

            _dbContext.CatalogItems.Update(updatingItem);
            await _dbContext.SaveChangesAsync();
        }

        _logger.LogInformation($"Changed quantity items Id: {id}");
    }
}