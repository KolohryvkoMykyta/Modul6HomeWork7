using Catalog.Host.Data.Entities;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Infrastructure.Exceptions;

namespace Catalog.Host.Repositories
{
    public class CatalogRadiusRepository : ICatalogRadiusRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogRadiusRepository> _logger;

        public CatalogRadiusRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogRadiusRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> AddAsync(string name)
        {
            var item = await _dbContext.AddAsync(new CatalogRadius
            {
                Radius = name
            });

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Added new radius");

            return item.Entity.Id;
        }

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> radiusStrings)
        {
            var radiuses = radiusStrings.Select(b => new CatalogRadius
            {
                Radius = b
            }).ToList();

            await _dbContext.AddRangeAsync(radiuses);
            _dbContext.SaveChanges();

            _logger.LogInformation("Added range of radiuses");

            return radiuses.Select(b => b.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _dbContext.CatalogRadiuses.FindAsync(id);

            if (result == null)
            {
                throw new BusinessException("Incorrect id");
            }

            _dbContext.CatalogRadiuses.Remove(result);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted radius");

            return true;
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            var updatingItem = await _dbContext.CatalogRadiuses.FindAsync(id);

            if (updatingItem == null)
            {
                throw new BusinessException("Incorrect id");
            }

            updatingItem.Radius = name;

            _dbContext.CatalogRadiuses.Update(updatingItem);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Updated radius");

            return true;
        }

        public async Task<CatalogRadius> GetByIdAsync(int id)
        {
            var result = await _dbContext.CatalogRadiuses.FirstOrDefaultAsync(b => b.Id == id);

            if (result == null)
            {
                throw new Infrastructure.Exceptions.BusinessException("Incorrect Id");
            }

            return result;
        }

        public async Task<IEnumerable<CatalogRadius>> GetAllRadiusesAsync()
        {
            return await _dbContext.CatalogRadiuses.ToListAsync();
        }
    }
}
