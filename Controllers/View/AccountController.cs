using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.Auth;

namespace HeThongQuanLyVanPhong.Controllers.View
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly TaiKhoanService _taiKhoanService; 

        public AccountController(AuthService authService, TaiKhoanService taiKhoanService) 
        {
            _authService = authService;
            _taiKhoanService = taiKhoanService; 
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string tenTaiKhoan, string matKhau)
        {
            if (string.IsNullOrEmpty(tenTaiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập và mật khẩu";
                return View();
            }

            var result = await _authService.LoginAsync(new LoginRequestDto { TenTaiKhoan = tenTaiKhoan, MatKhau = matKhau });
            if (result.Success && result.User != null)
            {
                HttpContext.Session.SetInt32("UserId", result.User.Id);
                HttpContext.Session.SetString("Username", result.User.TenTaiKhoan);
                HttpContext.Session.SetString("FullName", result.User.HoVaTen);
                HttpContext.Session.SetString("ChucVu", result.User.TenChucVu);
                HttpContext.Session.SetString("DonViCongTac", result.User.TenDonViCongTac);
               // HttpContext.Session.SetInt32("UserDonViId", result.User.IddonViCongTac ?? 0);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = result.Message ?? "Sai tên đăng nhập hoặc mật khẩu";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
                await _authService.LogoutAsync(userId.Value);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

       

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Chưa đăng nhập" });

            var (success, message) = await _taiKhoanService.ChangePasswordAsync(userId.Value, oldPassword, newPassword);
            return Json(new { success, message });
        }
    }
}