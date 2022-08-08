using Cleverbit.Case.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cleverbit.Case.Api.Controllers
{
    [ApiController]
    [Route("Region")]
    public class SearchRegionController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly ISearchRegionService _service;

        public SearchRegionController(ILogger<EmployeeController> logger, ISearchRegionService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{regionId}/employees")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMembers([FromRoute] int regionId)
        {
            IEnumerable<string> result = await _service.GetEmployees(regionId);

            return Ok(result);
        }
    }
}