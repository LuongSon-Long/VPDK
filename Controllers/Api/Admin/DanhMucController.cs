using HeThongQuanLyVanPhong.Controllers.Api;
using HeThongQuanLyVanPhong.DTOs.DanhMuc;
using HeThongQuanLyVanPhong.Filters;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [RequireSession(AllowGuest = false)]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DanhMucController : ApiControllerBase
    {
        private readonly DanhMucService _danhMucService;

        public DanhMucController(DanhMucService danhMucService)
        {
            _danhMucService = danhMucService;
        }

        [HttpGet("{table}")]
        public async Task<IActionResult> GetList(string table)
        {
            var data = await _danhMucService.GetListAsync(table);
            if (data == null)
                return BadRequest(new { message = "Table không hợp lệ" });
            return Ok(data);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] DanhMucRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { success = false, message = "Tên không được để trống" });

            var result = await _danhMucService.SaveAsync(request, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DanhMucRequestDto request)
        {
            var result = await _danhMucService.DeleteAsync(request.Table, request.Id, CurrentUserId);
            if (!result)
                return Ok(new { success = false, message = "Dữ liệu đang được sử dụng, không thể xóa!" });

            return Ok(new { success = true });
        }
    }
}
