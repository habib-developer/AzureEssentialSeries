using Ep10.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ep10.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            try
            {
                throw new Exception("Hello world from asp.net core");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Home Controller Privacy Erorr");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
