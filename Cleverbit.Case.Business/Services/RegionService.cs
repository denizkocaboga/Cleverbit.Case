using Cleverbit.Case.Common.Exceptions;
using Cleverbit.Case.Infrastructure.Cache;
using Cleverbit.Case.Infrastructure.SqlServer;
using Cleverbit.Case.Models.Entities;
using Cleverbit.Case.Models.Requests;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleverbit.Case.Business.Services
{
    public interface IRegionService
    {
        Task Create(CreateRegionCommand request);
        Task<IEnumerable<Region>> GetAllRegions();
    }

    public class RegionService : IRegionService
    {
        private readonly ILogger<RegionService> _logger;
        private readonly ISqlRepository _sql;
        private readonly ICacheRepository _cache;

        public RegionService(
            ILogger<RegionService> logger,
            ISqlRepository sqlRepository,
            ICacheRepository cacheRepository
            )
        {
            _logger = logger;
            _sql = sqlRepository;
            _cache = cacheRepository;
        }

        public async Task Create(CreateRegionCommand command)
        {
            await ValidateCreate(command);
            await _sql.CreateRegion(command);

            //ToDo: Fire RegionCreatedEvent in sqlRepository and move _cache.CreateRegion(command) to consumer.
            await _cache.CreateRegion(command);
        }

        private async Task ValidateCreate(CreateRegionCommand command)
        {
            bool isExists = await _cache.IsRegionExits(command.Id);
            if (isExists)
                throw new ConflictException(command.Id);

            if (command.ParentId.HasValue)
            {
                isExists = await _cache.IsRegionExits(command.ParentId.Value);
                if (!isExists)
                    throw new UnprocessableException($"there is no any region with this parentId:{command.ParentId.Value}");
            }
        }

        public async Task<IEnumerable<Region>> GetAllRegions()
        {
            //ToDo: Get this daha from Cache as Tree.             
            IEnumerable<Region> result = await _sql.GetAllRegions();
            return result;
        }
    }
}
