using Ep13.Models;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Ep13.Services;

namespace Ep13.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PushNotificationService _pushNotificationService;

        public HomeController(ILogger<HomeController> logger,PushNotificationService pushNotificationService)
        {
            _logger = logger;
            this._pushNotificationService = pushNotificationService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> SendPushAsync()
        {
            await _pushNotificationService.SendNotificationAsync(new PushSubscription
            {
                Endpoint = "Endpoint from subscriptin of device inside browser",
                Keys = new Dictionary<string, string>
                {
                    {"p256dh","p256dh key from subscription of device inside browser" },
                    {"auth","auth key from subscription of device inside browser" }
                }
            }, JsonConvert.SerializeObject(new
            {
                body = "Hello world from Azure Notification Hub",
                icon = "icon.png",
                badge = "badge.png",
                url = "google.com",
                title = "Test notification"
            }));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
