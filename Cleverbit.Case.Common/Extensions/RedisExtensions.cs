using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleverbit.Case.Common.Extensions
{
    public static class RedisExtensions
    {
        private static string KeyJoin(string prefix, RedisKey key) => $"{prefix}:{key}";

        public static async Task SetAddAncestorsAsync(this IDatabase redis, int key, IEnumerable<RedisValue> values)
        {
            string redisKey = KeyJoin(RedisPrefix.Ancestors, key.ToString());
            await redis.SetAddAsync(redisKey, values.ToRedisValues());
        }

        public static async Task SetAddAncestorsAsync(this IDatabase redis, int key, IEnumerable<int> values)
        {
            string redisKey = KeyJoin(RedisPrefix.Ancestors, key.ToString());
            await redis.SetAddAsync(redisKey, values.ToRedisValues());
        }


        public static async Task SetAddEmployeesAsync(this IDatabase redis, int key, IEnumerable<string> values)
        {
            string redisKey = KeyJoin(RedisPrefix.Employees, key.ToString());
            await redis.SetAddAsync(redisKey, values.ToRedisValues());
        }

        public static async Task SetAddAsync(this IDatabase redis, string keyPrefix, string key, string value)
        {
            string redisKey = KeyJoin(keyPrefix, key.ToString());
            await redis.SetAddAsync(redisKey, value);
        }

        public static async Task<RedisValue[]> SetMembersAsync(this IDatabase redis, string prefix, int key)
        {
            string redisKey = KeyJoin(prefix, key.ToString());
            RedisValue[] result = await redis.SetMembersAsync(redisKey);
            return result;
        }

        public static Task<bool> KeyExistsAsync(this IDatabase redis, string prefix, int key)
        {
            string redisKey = KeyJoin(prefix, key.ToString());
            return redis.KeyExistsAsync(redisKey);
        }
    }
}