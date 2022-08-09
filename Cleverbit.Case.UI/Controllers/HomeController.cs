using Cleverbit.Case.Business.Services;
using Cleverbit.Case.Models.Entities;
using Cleverbit.Case.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cleverbit.Case.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRegionService _service;

        public HomeController(ILogger<HomeController> logger, IRegionService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Region> result = await _service.GetAllRegions();
            ViewBag.Regions = result.OrderBy(p => p.Name);
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}