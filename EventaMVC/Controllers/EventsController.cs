using Microsoft.AspNetCore.Mvc;

namespace EventaV1.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Category() => View();
        public IActionResult Details(int id = 1) // ?id=1
        {
            ViewBag.EventId = id;
            return View();
        }
        public IActionResult Create() => View();
    }
}
