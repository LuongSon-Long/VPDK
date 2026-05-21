using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.View
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QuanLyDanhMuc()
        {
            return View();
        }

        public IActionResult QuanLyQuyTrinh()
        {
            return View();
        }
    }
}