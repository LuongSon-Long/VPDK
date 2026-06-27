using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class LuuTruController : ControllerBase
    {
        private readonly LuuTruService _luuTruService;

        public LuuTruController(LuuTruService luuTruService)
        {
            _luuTruService = luuTruService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        // GET: api/dodac/luutru/kho-list
        [HttpGet("kho-list")]
        public async Task<IActionResult> GetKhoList()
        {
            var result = await _luuTruService.GetKhoListAsync();
            return Ok(result);
        }

        // GET: api/dodac/luutru/gia-by-kho/{kho}
        [HttpGet("gia-by-kho/{kho}")]
        public async Task<IActionResult> GetGiaByKho(string kho)
        {
            var result = await _luuTruService.GetGiaByKhoAsync(kho);
            return Ok(result);
        }

        // GET: api/dodac/luutru/next-so-hsluu?kho=xxx&gia=xxx&ngan=xxx
        [HttpGet("next-so-hsluu")]
        public async Task<IActionResult> GetNextSoHSLuu([FromQuery] string kho, [FromQuery] string gia, [FromQuery] string ngan)
        {
            var result = await _luuTruService.GetNextSoHSLuuAsync(kho, gia, ngan);
            return Ok(new { soHSLuu = result.ToString() });
        }

        // POST: api/dodac/luutru/save
        [HttpPost("save")]
        public async Task<IActionResult> SaveLuuTru([FromBody] LuuTruDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var result = await _luuTruService.SaveLuuTruAsync(request, currentUserId);
            return Ok(new { success = result });
        }

        // GET: api/dodac/luutru/by-hoso/{idDangKyDoDac}
        [HttpGet("by-hoso/{idDangKyDoDac}")]
        public async Task<IActionResult> GetLuuTruByHoSoId(int idDangKyDoDac)
        {
            var result = await _luuTruService.GetLuuTruByHoSoIdAsync(idDangKyDoDac);
            return Ok(result);
        }
        // GET: api/dodac/luutru/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var result = await _luuTruService.GetAllLuuTruAsync(search);
            return Ok(result);
        }

        // DELETE: api/dodac/luutru/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Kiểm tra phân quyền nếu cần (Ví dụ: chỉ Admin mới được xóa)
            // var chucVu = HttpContext.Session.GetString("ChucVu");
            // if (chucVu != "Admin") return Forbid();

            var result = await _luuTruService.DeleteLuuTruAsync(id);
            if (result) return Ok(new { success = true });

            return BadRequest(new { success = false, message = "Không tìm thấy dữ liệu hoặc không thể xóa" });
        }
        // GET: api/dodac/luutru/ngan-list?kho=...&gia=...
        [HttpGet("ngan-list")]
        public async Task<IActionResult> GetNganList([FromQuery] string kho, [FromQuery] string gia)
        {
            var result = await _luuTruService.GetNganByKhoGiaAsync(kho, gia);
            return Ok(result);
        }

        // GET: api/dodac/luutru/find?kho=...&gia=...&ngan=...
        [HttpGet("find")]
        public async Task<IActionResult> FindLuuTru(string kho, string gia, string ngan)
        {
            var list = await _luuTruService.FindLuuTruAsync(kho, gia, ngan);
            return Ok(list); // Trả về list DTO đã có tên
        }
    }
}