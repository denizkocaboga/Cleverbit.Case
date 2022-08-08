using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleverbit.Case.Infrastructure.Cache
{
    public interface ICacheContext
    {
        Task SetAdd<TValue>(string keyPrefix, RedisValue key, IEnumerable<RedisValue> values);
    }

    public class RedisContext : ICacheContext
    {
        private readonly IDatabase _redis;

        public RedisContext(IDatabase redis)
        {
            _redis = redis;
        }

        public async Task SetAdd<TValue>(string keyPrefix, RedisValue key, IEnumerable<TValue> values) 
        {            
            await _redis.SetAddAsync($"{keyPrefix}:{key}", values.ToArray());
        }
    }
}
