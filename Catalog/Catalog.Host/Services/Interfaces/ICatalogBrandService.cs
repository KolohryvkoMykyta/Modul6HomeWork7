using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task<int?> AddAsync(string name);
        Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> names);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, string name);
        Task<CatalogBrandDto> GetByIdAsync(int id);
        Task<IEnumerable<CatalogBrandDto>> GetAllBrandsAsync();
    }
}
