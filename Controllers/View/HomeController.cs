using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.View
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}