using HeThongQuanLyVanPhong.DTOs.DoDac;
using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class SoManhController : ApiControllerBase
    {
        private readonly SoManhService _soManhService;

        public SoManhController(SoManhService soManhService)
        {
            _soManhService = soManhService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSoManh([FromBody] CreateSoManhDto request)
        {
            var result = await _soManhService.CreateSoManhAsync(request, CurrentUserId, CurrentDonViId);
            return Ok(result);
        }

        [HttpGet("lichsu")]
        public async Task<IActionResult> GetLichSuCapSo([FromQuery] string maXa, [FromQuery] int nam)
        {
            var result = await _soManhService.GetLichSuCapSoAsync(maXa, nam);
            return Ok(result);
        }

        [HttpGet("sohieu-max")]
        public async Task<IActionResult> GetSoHieuMax([FromQuery] int idTinh, [FromQuery] int nam)
        {
            var result = await _soManhService.GetSoHieuMaxByTinhAndNamAsync(idTinh, nam);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("update-max")]
        public async Task<IActionResult> UpdateSoHieuMax([FromBody] UpdateSoHieuMaxDto request)
        {
            var result = await _soManhService.UpdateSoHieuMaxAsync(request, CurrentUserId, CurrentDonViId);

            if (result.success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoHieu(int id)
        {
            if (CurrentChucVu != "Admin")
                return Forbid();

            var result = await _soManhService.DeleteSoHieuAsync(id);
            return Ok(result);
        }
    }
}
