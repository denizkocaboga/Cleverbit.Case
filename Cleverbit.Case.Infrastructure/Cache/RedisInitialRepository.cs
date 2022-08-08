using Cleverbit.Case.Business;
using Cleverbit.Case.Models.Requests;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cleverbit.Case.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace Cleverbit.Case.Infrastructure.Cache
{
    public interface ICacheInitialRepository : IBaseCacheRepository
    {
        Task LoadRegionEmployees(IEnumerable<CreateEmployeeCommand> employees);
        Task LoadRegionAncestor(IEnumerable<CreateRegionCommand> regions);
    }

    public class RedisInitialRepository : ICacheInitialRepository
    {

        private readonly Dictionary<int, Ancestor> _ancestors = new();
        private readonly Dictionary<int, List<string>> _regionEmployees = new();
        private readonly ILogger<RedisInitialRepository> _logger;
        private readonly IDatabase _cache;

        public RedisInitialRepository(ILogger<RedisInitialRepository> logger, IDatabase redis)
        {
            _logger = logger;
            _cache = redis;
        }

        public async Task LoadRegionAncestor(IEnumerable<CreateRegionCommand> regions)
        {
            GenerateAncestors(regions);

            //ToDo: Use constants for prefix
            //ToDo: Search Bulk 
            foreach (var item in _ancestors)
                await _cache.SetAddAncestorsAsync(item.Key, item.Value.GetAncestorIds());

            _logger.LogInformation("LoadRegionAncestor Done!");

        }

        public async Task LoadRegionEmployees(IEnumerable<CreateEmployeeCommand> employees)
        {
            GenerateRegionEmployees(employees);

            //ToDo:Parallel Foreach ??
            foreach (var regionEmployee in _regionEmployees)
                await _cache.SetAddEmployeesAsync(regionEmployee.Key, regionEmployee.Value);

            _logger.LogInformation("LoadRegionEmployees Done!");
        }

        private void GenerateAncestors(IEnumerable<CreateRegionCommand> regions)
        {
            foreach (var region in regions)
            {
                Ancestor ancestor = GenerateAncestor(region.Id);

                if (region.ParentId.HasValue)
                    GenerateParentAncestor(region.ParentId.Value, ancestor);
            }
        }

        private void GenerateRegionEmployees(IEnumerable<CreateEmployeeCommand> employees)
        {
            var groupedEmployees = employees.GroupBy(p => p.RegionId, r => r.NameSurname);

            foreach (var item in groupedEmployees)
                GenerateRegionEmployee(item.Key, item.ToArray());
        }

        private void GenerateRegionEmployee(int regionId, IEnumerable<string> employeeNames)
        {
            IList<int> parentIds = _ancestors[regionId].GetAncestorIds();

            foreach (int parentId in parentIds)
            {
                if (!_regionEmployees.TryGetValue(parentId, out List<string> employeeList))
                    _regionEmployees.Add(parentId, employeeList = new List<string>());

                employeeList.AddRange(employeeNames);
            }
        }

        private void GenerateParentAncestor(int parentId, Ancestor ancestor)
        {
            if (!_ancestors.TryGetValue(parentId, out Ancestor parentAncestor))
                _ancestors.Add(parentId, parentAncestor = new Ancestor { Id = parentId });

            ancestor.Parent = parentAncestor;
        }
        private Ancestor GenerateAncestor(int id)
        {
            if (!_ancestors.TryGetValue(id, out Ancestor ancestor))
            {
                ancestor = new Ancestor { Id = id };
                _ancestors.Add(id, ancestor);
            }

            return ancestor;
        }
    }
}
