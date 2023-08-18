using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<int?> AddAsync(string name);
        Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> typeStrings);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, string name);
        Task<CatalogType> GetByIdAsync(int id);
        Task<IEnumerable<CatalogType>> GetAllTypeAsync();
    }
}
