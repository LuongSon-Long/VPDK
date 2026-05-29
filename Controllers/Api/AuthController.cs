using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.Auth;

namespace HeThongQuanLyVanPhong.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            // Lưu session
            if (result.User != null)
            {
                HttpContext.Session.SetInt32("UserId", result.User.Id);
                HttpContext.Session.SetString("Username", result.User.TenTaiKhoan ?? "");
                HttpContext.Session.SetString("FullName", result.User.HoVaTen ?? "");
                HttpContext.Session.SetString("ChucVu", result.User.TenChucVu ?? "");
                HttpContext.Session.SetString("DonViCongTac", result.User.TenDonViCongTac ?? "");
                if (result.User.IdDonViCongTac.HasValue)
                    HttpContext.Session.SetInt32("UserDonViId", result.User.IdDonViCongTac.Value);
                if (result.User.IdTinh.HasValue)
                    HttpContext.Session.SetInt32("UserTinhId", result.User.IdTinh.Value);
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            await _authService.LogoutAsync(userId);
            HttpContext.Session.Clear();
            return Ok(new { success = true, message = "Đã đăng xuất" });
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Unauthorized(new { message = "Chưa đăng nhập" });
            }

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            if (user == null)
            {
                HttpContext.Session.Clear();
                return Unauthorized(new { message = "Tài khoản không tồn tại" });
            }

            return Ok(user);
        }

        [HttpGet("has-permission")]
        public async Task<IActionResult> HasPermission(int moduleId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Ok(false);
            }

            var hasPermission = await _authService.HasPermissionAsync(userId.Value, moduleId);
            return Ok(hasPermission);
        }

        [HttpGet("guest-login")]
        public IActionResult GuestLogin()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("UserId", -1);
            HttpContext.Session.SetString("FullName", "Khách đăng ký");
            HttpContext.Session.SetString("Username", "Khach");
            HttpContext.Session.SetString("ChucVu", "Khách");
            HttpContext.Session.SetString("DonViCongTac", "Hộ gia đình, cá nhân, tổ chức đăng ký");
            HttpContext.Session.SetInt32("UserTinhId", 0);
            return LocalRedirect("/");
        }

        [HttpPost("set-tinh")]
        public IActionResult SetTinh([FromBody] UpdateTinhDto request)
        {
            if (request.IdTinh > 0)
            {
                HttpContext.Session.SetInt32("UserTinhId", request.IdTinh);
            }
            return Ok(new { success = true });
        }
    }
    public class UpdateTinhDto
    {
        public int IdTinh { get; set; }
    }
}