using Cleverbit.Case.Common.Exceptions;
using Cleverbit.Case.Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleverbit.Case.Business.Services
{
    public interface ISearchRegionService
    {
        Task<IEnumerable<string>> GetEmployees(int regionId);
    }


    public class SearchRegionService : ISearchRegionService
    {
        private readonly ILogger<RegionService> _logger;
        private readonly ICacheRepository _cacheRepository;

        public SearchRegionService(
            ILogger<RegionService> logger,
            ICacheRepository cacheRepository
            )
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<string>> GetEmployees(int regionId)
        {
            bool isExists = await _cacheRepository.IsRegionExits(regionId);
            if (!isExists)
                throw new NotFoundException($"region not found ({regionId})");

            IEnumerable<string> result = await _cacheRepository.GetRegionEmployees(regionId);
            return result;
        }
    }
}
