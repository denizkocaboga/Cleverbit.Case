using AutoMapper;
using Cleverbit.Case.Models.Entities;
using Cleverbit.Case.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleverbit.Case.Infrastructure.SqlServer
{
    public interface ISqlRepository
    {
        Task<int> CreateEmployee(CreateEmployeeCommand model);
        Task CreateEmployees(IEnumerable<CreateEmployeeCommand> models);
        Task CreateRegion(CreateRegionCommand command);
        Task CreateRegions(IEnumerable<CreateRegionCommand> models);
        Task ReinitializeDb();
        Task<IEnumerable<Region>> GetAllRegions();
    }

    public class SqlRepository : ISqlRepository
    {
        private readonly ILogger<SqlRepository> _logger;
        private readonly IMapper _mapper;
        private readonly SqlContext _db;

        public SqlRepository(ILogger<SqlRepository> logger, IMapper mapper, SqlContext db)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
        }

        public async Task ReinitializeDb()
        {
            await _db.Database.EnsureDeletedAsync();
            _logger.LogInformation("Database Delete Done!");

            await _db.Database.MigrateAsync();
            _logger.LogInformation("Database Migration Done!");
        }
        public async Task CreateRegions(IEnumerable<CreateRegionCommand> models)
        {
            IEnumerable<Region> entities = _mapper.Map<IEnumerable<Region>>(models);

            await _db.Regions.AddRangeAsync(entities);
            _db.SaveChanges();

            //ToDo: Publish RegionsCreatedEvent. May use  BatchConsumer.

            _logger.LogInformation("CreateRegions Done!");

        }

        public async Task CreateEmployees(IEnumerable<CreateEmployeeCommand> models)
        {
            IEnumerable<Employee> entities = _mapper.Map<IEnumerable<Employee>>(models);

            await _db.Employees.AddRangeAsync(entities);
            _db.SaveChanges();

            //ToDo: Publish EmployeesCreatedEvent. May use BatchConsumer.
            _logger.LogInformation("CreateEmployees Done!");

        }

        public async Task CreateRegion(CreateRegionCommand model)
        {
            Region entity = _mapper.Map<Region>(model);

            await _db.Regions.AddAsync(entity);
            _db.SaveChanges();

            //ToDo: Publish RegionCreatedEvent.
        }

        public async Task<int> CreateEmployee(CreateEmployeeCommand model)
        {
            Employee entity = _mapper.Map<Employee>(model);

            EntityEntry<Employee> entry = await _db.Employees.AddAsync(entity);
            _db.SaveChanges();

            //ToDo: Publish EmployeeCreatedEvent. And set cache on consumer

            return entry.Entity.Id;
        }

        public async Task<IEnumerable<Region>> GetAllRegions()
        {
            Region[] result = await _db.Regions.ToArrayAsync();
            return result;
        }
    }
}