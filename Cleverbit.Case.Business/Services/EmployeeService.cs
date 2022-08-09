using Cleverbit.Case.Common.Exceptions;
using Cleverbit.Case.Infrastructure.Cache;
using Cleverbit.Case.Infrastructure.SqlServer;
using Cleverbit.Case.Models.Requests;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cleverbit.Case.Business.Services
{
    public interface IEmployeeService
    {
        Task<int> Create(CreateEmployeeCommand request);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly ISqlRepository _sql;
        private readonly ICacheRepository _cache;

        public EmployeeService(
            ILogger<EmployeeService> logger,
            ISqlRepository sqlRepository,
            ICacheRepository cacheRepository
            )
        {
            _logger = logger;
            _sql = sqlRepository;
            _cache = cacheRepository;
        }

        public async Task<int> Create(CreateEmployeeCommand command)
        {
            await ValidateCreate(command);

            int result = await _sql.CreateEmployee(command);

            //ToDo: Fire EmployeeCreatedEvent in sqlRepository and move 
            await _cache.CreateEmployee(command);

            return result;
        }

        private async Task ValidateCreate(CreateEmployeeCommand command)
        {
            bool isRegionExists = await _cache.IsRegionExits(command.RegionId);
            if (!isRegionExists)
                throw new UnprocessableException($"there is no any region with this regionId:{command.RegionId}");

        }
    }


}
