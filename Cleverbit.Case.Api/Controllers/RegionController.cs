using Cleverbit.Case.Business.Services;
using Cleverbit.Case.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Cleverbit.Case.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly ILogger<RegionController> _logger;
        private readonly IRegionService _service;

        public RegionController(ILogger<RegionController> logger, IRegionService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateRegionCommand request)
        {
            await _service.Create(request);

            return Created($"{Request.Host}/region/{request.Id}", request.Id);
        }

    }
}