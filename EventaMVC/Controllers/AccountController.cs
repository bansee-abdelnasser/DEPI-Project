using Microsoft.AspNetCore.Mvc;

namespace EventaV1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();
        public IActionResult Register() => View();
        public IActionResult Profile() => View();
        public IActionResult EditProfile() => View();
    }
}
