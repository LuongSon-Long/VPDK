using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.TaiKhoan;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly TaiKhoanService _taiKhoanService;

        public TaiKhoanController(TaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] string? searchTerm)
        {
            var list = await _taiKhoanService.GetAllAsync(searchTerm);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var user = await _taiKhoanService.GetDetailByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy tài khoản" });

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaiKhoanRequestDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized(new { message = "Chưa đăng nhập" });

            var user = new TaiKhoan
            {
                TenTaiKhoan = request.TenTaiKhoan,
                MatKhau = request.MatKhau,
                HoVaTen = request.HoVaTen,
                IdchucVu = request.IdchucVu,
                IddonViCongTac = request.IddonViCongTac
            };

            var result = await _taiKhoanService.CreateAsync(user, request.SelectedModules ?? new List<int>(), currentUserId);
            return Ok(new { success = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaiKhoanRequestDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized(new { message = "Chưa đăng nhập" });

            var user = new TaiKhoan
            {
                Id = id,
                HoVaTen = request.HoVaTen,
                IdchucVu = request.IdchucVu,
                IddonViCongTac = request.IddonViCongTac
            };

            var result = await _taiKhoanService.UpdateAsync(user, request.SelectedModules ?? new List<int>(), currentUserId);
            return Ok(new { success = result });
        }

        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized(new { message = "Chưa đăng nhập" });

            var result = await _taiKhoanService.ResetPasswordAsync(id, currentUserId);
            return Ok(new { success = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized(new { message = "Chưa đăng nhập" });

            var result = await _taiKhoanService.DeleteAsync(id, currentUserId);
            return Ok(new { success = result });
        }

        [HttpGet("my-modules")]
        public async Task<IActionResult> GetMyModules()
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var modules = await _taiKhoanService.GetMyModulesAsync(userId);
            return Ok(modules);
        }
    }
}