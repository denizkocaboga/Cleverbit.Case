using StackExchange.Redis;
using System;

namespace Cleverbit.Case.Infrastructure.Cache
{
    public interface IBaseCacheRepository
    {
    }

    public class BaseCacheRepository : IBaseCacheRepository
    {
        protected IDatabase Cache { get; }

        protected BaseCacheRepository(IDatabase cache)
        {
            Cache = cache;
        }
    }
}
