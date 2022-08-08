using Cleverbit.Case.Business.Services;
using Cleverbit.Case.Infrastructure.Csv;
using Cleverbit.Case.Models.Requests;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cleverbit.Case.Initial
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInitialDataService _service;
        private readonly ICsvContext _csvContext;

        public Worker(
            ILogger<Worker> logger,
            IInitialDataService service,

            ICsvContext csvReader)
        {
            _logger = logger;
            _service = service;
            _csvContext = csvReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string csvPath = Path.Combine("..", "CsvData");
            string regionsPath = Path.Join(csvPath, "regions.csv");
            string employeesPath = Path.Join(csvPath, "employees.csv");


            var regions = _csvContext.GetData<CreateRegionCommand>(regionsPath);
            var employees = _csvContext.GetData<CreateEmployeeCommand>(employeesPath);

            await _service.ImportInitialDataData(regions, employees);
            
            _logger.LogInformation("Data Initialize Finished...");
        }
    }
}