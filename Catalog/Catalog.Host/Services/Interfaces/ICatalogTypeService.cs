using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task<int?> AddAsync(string name);
        Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> names);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, string name);
        Task<CatalogTypeDto> GetByIdAsync(int id);
        Task<IEnumerable<CatalogTypeDto>> GetAllTypeAsync();
    }
}
