using Microsoft.AspNetCore.Mvc;

namespace EventaV1.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
