namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, int catalogRadiusId, string pictureFileName);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, int catalogRadiusId, string pictureFileName);
}