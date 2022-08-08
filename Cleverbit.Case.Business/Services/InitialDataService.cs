using Cleverbit.Case.Infrastructure.Cache;
using Cleverbit.Case.Infrastructure.SqlServer;
using Cleverbit.Case.Models.Requests;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleverbit.Case.Business.Services
{
    public interface IInitialDataService
    {
        Task ImportInitialDataData(IEnumerable<CreateRegionCommand> regions, IEnumerable<CreateEmployeeCommand> employees);
    }

    public class InitialDataService : IInitialDataService
    {
        private readonly ILogger<InitialDataService> _logger;
        private readonly ICacheInitialRepository _cacheRepo;
        private readonly ISqlRepository _sqlRepo;

        public InitialDataService(
            ILogger<InitialDataService> logger,
            ICacheInitialRepository repo, 
            ISqlRepository sqlRepo)
        {            
            _logger = logger;
            _cacheRepo = repo;
            _sqlRepo = sqlRepo;
        }

        public async Task ImportInitialDataData(IEnumerable<CreateRegionCommand> regions, IEnumerable<CreateEmployeeCommand> employees)
        {
            await _sqlRepo.ReinitializeDb();

            await _sqlRepo.CreateRegions(regions);
            await _cacheRepo.LoadRegionAncestor(regions);            

            await _sqlRepo.CreateEmployees(employees);
            await _cacheRepo.LoadRegionEmployees(employees);

            _logger.LogInformation("ImportInitialDataData Done!");
        }

    }


}
