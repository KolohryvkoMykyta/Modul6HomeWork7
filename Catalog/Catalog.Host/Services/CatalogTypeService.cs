using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;
        private readonly IMapper _mapper;

        public CatalogTypeService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogTypeRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(string name)
        {
            return await ExecuteSafeAsync(async () => await _catalogTypeRepository.AddAsync(name));
        }

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> names)
        {
            return await ExecuteSafeAsync(async () => await _catalogTypeRepository.AddRangeAsync(names));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () => await _catalogTypeRepository.DeleteAsync(id));
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            return await ExecuteSafeAsync(async () => await _catalogTypeRepository.UpdateAsync(id, name));
        }

        public async Task<CatalogTypeDto> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogTypeRepository.GetByIdAsync(id);

                return _mapper.Map<CatalogTypeDto>(result);
            });
        }

        public async Task<IEnumerable<CatalogTypeDto>> GetAllTypeAsync()
        {
            var listItems = await _catalogTypeRepository.GetAllTypeAsync();
            var result = listItems.Select(t => _mapper.Map<CatalogTypeDto>(t)).ToList();

            return result;
        }
    }
}
