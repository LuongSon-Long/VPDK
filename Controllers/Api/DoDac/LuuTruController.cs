using HeThongQuanLyVanPhong.DTOs.DoDac;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class LuuTruController : ApiControllerBase
    {
        private readonly LuuTruService _luuTruService;

        public LuuTruController(LuuTruService luuTruService)
        {
            _luuTruService = luuTruService;
        }

        [HttpGet("kho-list")]
        public async Task<IActionResult> GetKhoList()
        {
            var result = await _luuTruService.GetKhoListAsync();
            return Ok(result);
        }

        [HttpGet("gia-by-kho/{kho}")]
        public async Task<IActionResult> GetGiaByKho(string kho)
        {
            var result = await _luuTruService.GetGiaByKhoAsync(kho);
            return Ok(result);
        }

        [HttpGet("next-so-hsluu")]
        public async Task<IActionResult> GetNextSoHSLuu([FromQuery] string kho, [FromQuery] string gia, [FromQuery] string ngan)
        {
            var result = await _luuTruService.GetNextSoHSLuuAsync(kho, gia, ngan);
            return Ok(new { soHSLuu = result.ToString() });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveLuuTru([FromBody] LuuTruDto request)
        {
            var result = await _luuTruService.SaveLuuTruAsync(request, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpGet("by-hoso/{idDangKyDoDac}")]
        public async Task<IActionResult> GetLuuTruByHoSoId(int idDangKyDoDac)
        {
            var result = await _luuTruService.GetLuuTruByHoSoIdAsync(idDangKyDoDac);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var result = await _luuTruService.GetAllLuuTruAsync(search);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _luuTruService.DeleteLuuTruAsync(id);
            if (result) return Ok(new { success = true });

            return BadRequest(new { success = false, message = "Không tìm thấy dữ liệu hoặc không thể xóa" });
        }

        [HttpGet("ngan-list")]
        public async Task<IActionResult> GetNganList([FromQuery] string kho, [FromQuery] string gia)
        {
            var result = await _luuTruService.GetNganByKhoGiaAsync(kho, gia);
            return Ok(result);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindLuuTru(string kho, string gia, string ngan)
        {
            var list = await _luuTruService.FindLuuTruAsync(kho, gia, ngan);
            return Ok(list);
        }
    }
}
