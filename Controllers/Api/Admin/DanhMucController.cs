using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DanhMuc;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly DanhMucService _danhMucService;

        public DanhMucController(DanhMucService danhMucService)
        {
            _danhMucService = danhMucService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        // GET: api/admin/DanhMuc/ChucVu
        [HttpGet("{table}")]
        public async Task<IActionResult> GetList(string table)
        {
            var data = await _danhMucService.GetListAsync(table);
            if (data == null)
                return BadRequest(new { message = "Table không hợp lệ" });
            return Ok(data);
        }

        // POST: api/admin/DanhMuc/save
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] DanhMucRequestDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized(new { message = "Chưa đăng nhập" });

            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { success = false, message = "Tên không được để trống" });

            var result = await _danhMucService.SaveAsync(request, currentUserId);
            return Ok(new { success = result });
        }

        // POST: api/admin/DanhMuc/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DanhMucRequestDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized(new { message = "Chưa đăng nhập" });

            var result = await _danhMucService.DeleteAsync(request.Table, request.Id, currentUserId);
            if (!result)
                return Ok(new { success = false, message = "Dữ liệu đang được sử dụng, không thể xóa!" });

            return Ok(new { success = true });
        }
    }
}