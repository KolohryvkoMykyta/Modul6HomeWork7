using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogRadiusService : BaseDataService<ApplicationDbContext>, ICatalogRadiusService
    {
        private readonly ICatalogRadiusRepository _catalogRadiusRepository;
        private readonly IMapper _mapper;

        public CatalogRadiusService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogRadiusRepository catalogRadiusRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogRadiusRepository = catalogRadiusRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(string name)
        {
            return await ExecuteSafeAsync(async () => await _catalogRadiusRepository.AddAsync(name));
        }

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> names)
        {
            return await ExecuteSafeAsync(async () => await _catalogRadiusRepository.AddRangeAsync(names));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () => await _catalogRadiusRepository.DeleteAsync(id));
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            return await ExecuteSafeAsync(async () => await _catalogRadiusRepository.UpdateAsync(id, name));
        }

        public async Task<CatalogRadiusDto> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogRadiusRepository.GetByIdAsync(id);

                return _mapper.Map<CatalogRadiusDto>(result);
            });
        }

        public async Task<IEnumerable<CatalogRadiusDto>> GetAllRadiusesAsync()
        {
            var listItems = await _catalogRadiusRepository.GetAllRadiusesAsync();
            var result = listItems.Select(r => _mapper.Map<CatalogRadiusDto>(r)).ToList();

            return result;
        }
    }
}
