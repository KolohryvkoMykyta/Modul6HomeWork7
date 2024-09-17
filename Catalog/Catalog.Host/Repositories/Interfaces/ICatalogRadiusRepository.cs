using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogRadiusRepository
    {
        Task<int?> AddAsync(string name);
        Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> radiusStrings);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, string name);
        Task<CatalogRadius> GetByIdAsync(int id);
        Task<IEnumerable<CatalogRadius>> GetAllRadiusesAsync();
    }
}
