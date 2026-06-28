using HeThongQuanLyVanPhong.DTOs.Auth;
using HeThongQuanLyVanPhong.Filters;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymousSession]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.User != null)
            {
                HttpContext.Session.SetInt32(SessionAuthExtensions.UserIdKey, result.User.Id);
                HttpContext.Session.SetString("Username", result.User.TenTaiKhoan ?? "");
                HttpContext.Session.SetString("FullName", result.User.HoVaTen ?? "");
                HttpContext.Session.SetString("ChucVu", result.User.TenChucVu ?? "");
                HttpContext.Session.SetString("DonViCongTac", result.User.TenDonViCongTac ?? "");
                if (result.User.IdDonViCongTac.HasValue)
                    HttpContext.Session.SetInt32(SessionAuthExtensions.UserDonViIdKey, result.User.IdDonViCongTac.Value);
                if (result.User.IdTinh.HasValue)
                    HttpContext.Session.SetInt32(SessionAuthExtensions.UserTinhIdKey, result.User.IdTinh.Value);
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.GetSessionUserId();
            await _authService.LogoutAsync(userId);
            HttpContext.Session.Clear();
            return Ok(new { success = true, message = "Đã đăng xuất" });
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = HttpContext.GetSessionUserId()!.Value;
            var user = await _authService.GetCurrentUserAsync(userId);
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
            var userId = HttpContext.GetSessionUserId()!.Value;
            var hasPermission = await _authService.HasPermissionAsync(userId, moduleId);
            return Ok(hasPermission);
        }

        [AllowAnonymousSession]
        [HttpGet("guest-login")]
        public IActionResult GuestLogin()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32(SessionAuthExtensions.UserIdKey, -1);
            HttpContext.Session.SetString("FullName", "Khách đăng ký");
            HttpContext.Session.SetString("Username", "Khach");
            HttpContext.Session.SetString("ChucVu", "Khách");
            HttpContext.Session.SetString("DonViCongTac", "Hộ gia đình, cá nhân, tổ chức đăng ký");
            HttpContext.Session.SetInt32(SessionAuthExtensions.UserTinhIdKey, 0);
            return LocalRedirect("/");
        }

        [HttpPost("set-tinh")]
        public IActionResult SetTinh([FromBody] UpdateTinhDto request)
        {
            if (request.IdTinh > 0)
            {
                HttpContext.Session.SetInt32(SessionAuthExtensions.UserTinhIdKey, request.IdTinh);
            }
            return Ok(new { success = true });
        }
    }

    public class UpdateTinhDto
    {
        public int IdTinh { get; set; }
    }
}
