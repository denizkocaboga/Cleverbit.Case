using Cleverbit.Case.Common;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cleverbit.Case.Models.Requests;
using Cleverbit.Case.Common.Extensions;
using System.Linq;

namespace Cleverbit.Case.Infrastructure.Cache
{
    public interface ICacheRepository : IBaseCacheRepository
    {
        Task<IEnumerable<string>> GetRegionEmployees(int regionId);
        Task<bool> IsRegionExits(int regionId);
        Task CreateRegion(CreateRegionCommand command);
        Task CreateEmployee(CreateEmployeeCommand command);
    }

    public class RedisRepository : BaseCacheRepository, ICacheRepository
    {
        public RedisRepository(IDatabase redis) : base(redis) { }

        public async Task<bool> IsRegionExits(int regionId)
        {
            bool result = await Cache.KeyExistsAsync(RedisPrefix.Ancestors, regionId);

            return result;
        }

        public async Task<IEnumerable<string>> GetRegionEmployees(int regionId)
        {
            RedisValue[] redisValues = await Cache.SetMembersAsync(RedisPrefix.Employees, regionId);

            return redisValues.ToStringArray();
        }

        public async Task CreateRegion(CreateRegionCommand command)
        {
            RedisValue[] redisValues = new[] { RedisValue.Unbox(command.Id) };

            if (command.ParentId.HasValue)
            {
                var regionIds = await GetAncestors(command.ParentId.Value);
                redisValues = regionIds.Union(redisValues).ToArray();
            }

            await Cache.SetAddAncestorsAsync(command.Id, redisValues);
        }

        public async Task CreateEmployee(CreateEmployeeCommand command)
        {
            IEnumerable<RedisValue> regionIds = await GetAncestors(command.RegionId);

            foreach (var regionId in regionIds)
                await Cache.SetAddAsync(RedisPrefix.Employees, regionId, RedisValue.Unbox(command.NameSurname));
        }

        private async Task<IEnumerable<RedisValue>> GetAncestors(int regionId)
        {
            RedisValue[] redisValues = await Cache.SetMembersAsync(RedisPrefix.Ancestors, regionId);

            return redisValues;
        }
    }
}