using Catalog.Host.Data.Entities;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Infrastructure.Exceptions;

namespace Catalog.Host.Repositories
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogTypeRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogTypeRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> AddAsync(string name)
        {
            var item = await _dbContext.AddAsync(new CatalogType
            {
                Type = name
            });
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Added new type");

            return item.Entity.Id;
        }

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> typeStrings)
        {
            var types = typeStrings.Select(b => new CatalogType
            {
                Type = b
            }).ToList();

            await _dbContext.AddRangeAsync(types);
            _dbContext.SaveChanges();

            _logger.LogInformation("Added range of types");

            return types.Select(b => b.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _dbContext.CatalogTypes.FindAsync(id);

            if (result == null)
            {
                throw new BusinessException("Incorrect id");
            }

            _dbContext.CatalogTypes.Remove(result);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted type");

            return true;
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            var updatingItem = await _dbContext.CatalogTypes.FindAsync(id);

            if (updatingItem == null)
            {
                throw new BusinessException("Incorrect id");
            }

            updatingItem.Type = name;

            _dbContext.CatalogTypes.Update(updatingItem);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Updated type");

            return true;
        }

        public async Task<CatalogType> GetByIdAsync(int id)
        {
            var result = await _dbContext.CatalogTypes.FirstOrDefaultAsync(t => t.Id == id);

            if (result == null)
            {
                throw new BusinessException("Incorrect id");
            }

            return result;
        }

        public async Task<IEnumerable<CatalogType>> GetAllTypeAsync()
        {
            return await _dbContext.CatalogTypes.ToListAsync();
        }
    }
}
