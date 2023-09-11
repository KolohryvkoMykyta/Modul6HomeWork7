using Catalog.Host.Data.Entities;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Infrastructure.Exceptions;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogBrandRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogBrandRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> AddAsync(string name)
        {
            var item = await _dbContext.AddAsync(new CatalogBrand
            {
                Brand = name
            });

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Added new brand");

            return item.Entity.Id;
        }

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> brandStrings)
        {
            var brands = brandStrings.Select(b => new CatalogBrand
            {
                Brand = b
            }).ToList();

            await _dbContext.AddRangeAsync(brands);
            _dbContext.SaveChanges();

            _logger.LogInformation("Added new range of brands");

            return brands.Select(b => b.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _dbContext.CatalogBrands.FindAsync(id);

            if (result == null)
            {
                throw new BusinessException("Incorrect id");
            }

            _dbContext.CatalogBrands.Remove(result);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted brand");

            return true;
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            var updatingItem = await _dbContext.CatalogBrands.FindAsync(id);

            if (updatingItem == null)
            {
                throw new BusinessException("Incorrect id");
            }

            updatingItem.Brand = name;

            _dbContext.CatalogBrands.Update(updatingItem);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Updated brand");

            return true;
        }

        public async Task<CatalogBrand> GetByIdAsync(int id)
        {
            var result = await _dbContext.CatalogBrands.FirstOrDefaultAsync(b => b.Id == id);

            if (result == null)
            {
                throw new BusinessException("Incorrect Id");
            }

            return result;
        }

        public async Task<IEnumerable<CatalogBrand>> GetAllBrands()
        {
            return await _dbContext.CatalogBrands.ToListAsync();
        }
    }
}
