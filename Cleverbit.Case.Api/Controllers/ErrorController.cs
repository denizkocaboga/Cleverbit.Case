using Cleverbit.Case.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Cleverbit.Case.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            Exception ex = context.Error;

            _logger.LogError(ex, ex.Message, ex.Data);
            ;
            return ex is HttpResponseException responseException
                ? new ObjectResult(responseException.Message) { StatusCode = (int?)responseException.StatusCode }
                : (IActionResult)new ObjectResult($"unexpected error occured. please contact with this traceId:{Guid.NewGuid}") { StatusCode = 500 };

            //ToDo: response corelationId for exceptions.
            //ToDo: Use corelationId all this request life time. e.g. fired events.
            //HttpContext.Request.Headers["X-CorelationId"];
        }

        [Route("health")]
        public ActionResult Health()
        {
            return Health();
        }
    }
}