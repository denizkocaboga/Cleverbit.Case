using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cleverbit.Case.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static RedisValue[] ToRedisValues<T>(this IEnumerable<T> values) where T : IConvertible
        {
            RedisValue[] redisValues = values.Select(p => RedisValue.Unbox(p)).ToArray();
            return redisValues;
        }

    }
}