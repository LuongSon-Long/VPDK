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
    }
}