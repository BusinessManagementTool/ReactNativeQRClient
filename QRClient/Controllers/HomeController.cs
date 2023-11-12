using Microsoft.AspNetCore.Mvc;
using QRClient.Models;
using System.Diagnostics;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace QRClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            var writter = new QRCodeWriter();
            var resultBit = writter.encode(formCollection["QRstring"],ZXing.BarcodeFormat.QR_CODE,200,200);
            var matrix = resultBit;
            int scale = 2;
            Bitmap result = new Bitmap(matrix.Width*scale,matrix.Height*scale);
            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    Color pixel = matrix[i, j] ? Color.Black : Color.White;
                    for (int x = 0; x < scale; x++)
                    {
                        for (int y = 0; y < scale; y++)
                        {
                            result.SetPixel(i * scale + x, j * scale + y, pixel);
                        }
                    }
                }
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
            result.Save(webRootPath + "\\Images\\QrcodeNew.png");
            ViewBag.URL = "\\Images\\QrcodeNew.png";
            return View();
        }

        public IActionResult ReadQR()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var path = webRootPath + "\\Images\\QrcodeNew.png";
            var reader = new BarcodeReaderGeneric();
            Bitmap image = (Bitmap)Image.FromFile(path);
            using (image)
            {
                LuminanceSource source;
                source = new BitmapLuminanceSource(image);
                Result result = reader.Decode(source);
                ViewBag.Text = result.Text;

            }
            return View("Index");
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