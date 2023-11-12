using Microsoft.AspNetCore.Mvc;
using QRClient.Models;
using QRClient.QREngine;
using System.Diagnostics;

namespace QRClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IQREngine _qREngine;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IQREngine qREngine)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _qREngine = qREngine;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            var filePath = _qREngine.QRGenerator(formCollection,_webHostEnvironment);
            ViewBag.URL = filePath;
            return View();
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