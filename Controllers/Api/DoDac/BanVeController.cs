using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class BanVeController : ControllerBase
    {
        private readonly BanVeService _banVeService;

        public BanVeController(BanVeService banVeService)
        {
            _banVeService = banVeService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        [HttpGet("by-hoso/{idDangKyDoDac}")]
        public async Task<IActionResult> GetByHoSoId(int idDangKyDoDac)
        {
            var result = await _banVeService.GetByHoSoIdAsync(idDangKyDoDac);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _banVeService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy bản vẽ" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBanVe([FromBody] SaveBanVeDto dto)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized();

            var result = await _banVeService.SaveBanVeAsync(dto, currentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message, id = result.id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanVe(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized();

            var result = await _banVeService.DeleteBanVeAsync(id, currentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message });
        }
    }
}