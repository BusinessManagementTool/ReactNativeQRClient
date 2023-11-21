using Microsoft.AspNetCore.Mvc;

namespace QRClient.Controllers
{
    public class AnalyticsController : Controller
    {
        // GET: AnalyticsController
        public void Index()
        {

           // TODO: Make sure Analytics api running locally
           Response.Redirect("http://127.0.0.1:5000/dash/");
        }
    }
}
