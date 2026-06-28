using HeThongQuanLyVanPhong.DTOs.DoDac;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class BanVeController : ApiControllerBase
    {
        private readonly BanVeService _banVeService;

        public BanVeController(BanVeService banVeService)
        {
            _banVeService = banVeService;
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
            var result = await _banVeService.SaveBanVeAsync(dto, CurrentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message, id = result.id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanVe(int id)
        {
            var result = await _banVeService.DeleteBanVeAsync(id, CurrentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message });
        }
    }
}
