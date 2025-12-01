using System.Diagnostics;
using EventaV1.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaV1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();
        public IActionResult Terms() => View();
    }
}
