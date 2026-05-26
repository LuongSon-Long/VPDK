using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using HeThongQuanLyVanPhong.Services; // Gọi thư mục Services chứa TaiKhoanService

namespace HeThongQuanLyVanPhong.Controllers.View
{
    public class HomeController : Controller
    {
        // Khắc phục lỗi "_taiKhoanService does not exist": Khai báo biến
        private readonly TaiKhoanService _taiKhoanService;

        // Tiêm (Inject) service vào thông qua Constructor
        public HomeController(TaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        // Khắc phục lỗi "await operator...": Đổi IActionResult thành async Task<IActionResult>
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Bây giờ hàm đã là async nên có thể dùng await thoải mái
            var userModules = await _taiKhoanService.GetUserDashboardModulesAsync(userId.Value);

            return View(userModules);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}