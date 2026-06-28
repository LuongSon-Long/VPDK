using HeThongQuanLyVanPhong.Controllers.Api;
using HeThongQuanLyVanPhong.DTOs.TaiKhoan;
using HeThongQuanLyVanPhong.Filters;
using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [RequireSession(AllowGuest = false)]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class TaiKhoanController : ApiControllerBase
    {
        private readonly TaiKhoanService _taiKhoanService;

        public TaiKhoanController(TaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
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
            var user = new TaiKhoan
            {
                TenTaiKhoan = request.TenTaiKhoan,
                MatKhau = request.MatKhau,
                HoVaTen = request.HoVaTen,
                IdchucVu = request.IdchucVu,
                IddonViCongTac = request.IddonViCongTac,
                Idtinh = request.IdTinh
            };

            var result = await _taiKhoanService.CreateAsync(user, request.SelectedModules ?? new List<int>(), CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaiKhoanRequestDto request)
        {
            var user = new TaiKhoan
            {
                Id = id,
                HoVaTen = request.HoVaTen,
                IdchucVu = request.IdchucVu,
                IddonViCongTac = request.IddonViCongTac,
                Idtinh = request.IdTinh
            };

            var result = await _taiKhoanService.UpdateAsync(user, request.SelectedModules ?? new List<int>(), CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var result = await _taiKhoanService.ResetPasswordAsync(id, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taiKhoanService.DeleteAsync(id, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpGet("my-modules")]
        public async Task<IActionResult> GetMyModules()
        {
            var modules = await _taiKhoanService.GetMyModulesAsync(CurrentUserId);
            return Ok(modules);
        }
    }
}
