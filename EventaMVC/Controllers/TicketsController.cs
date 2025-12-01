using Microsoft.AspNetCore.Mvc;

namespace EventaV1.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult MyTickets() => View();
        public IActionResult Checkout() => View();
        public IActionResult Success() => View();
    }
}
