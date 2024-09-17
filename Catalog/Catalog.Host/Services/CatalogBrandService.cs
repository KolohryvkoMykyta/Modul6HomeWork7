using System.Xml.Linq;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
    {
        private readonly ICatalogBrandRepository _catalogBrandRepository;
        private readonly IMapper _mapper;

        public CatalogBrandService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogBrandRepository catalogBrandRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogBrandRepository = catalogBrandRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(string name)
        {
            return await ExecuteSafeAsync(async () => await _catalogBrandRepository.AddAsync(name));
        }

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<string> names)
        {
            return await ExecuteSafeAsync(async () => await _catalogBrandRepository.AddRangeAsync(names));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () => await _catalogBrandRepository.DeleteAsync(id));
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            return await ExecuteSafeAsync(async () => await _catalogBrandRepository.UpdateAsync(id, name));
        }

        public async Task<CatalogBrandDto> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogBrandRepository.GetByIdAsync(id);

                return _mapper.Map<CatalogBrandDto>(result);
            });
        }

        public async Task<IEnumerable<CatalogBrandDto>> GetAllBrandsAsync()
        {
            var listItems = await _catalogBrandRepository.GetAllBrands();
            var result = listItems.Select(b => _mapper.Map<CatalogBrandDto>(b)).ToList();

            return result;
        }
    }
}
