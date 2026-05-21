using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class SoManhController : ControllerBase
    {
        private readonly SoManhService _soManhService;

        public SoManhController(SoManhService soManhService)
        {
            _soManhService = soManhService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        private int GetCurrentDonViId()
        {
            return HttpContext.Session.GetInt32("UserDonViId") ?? 0;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSoManh([FromBody] CreateSoManhDto request)
        {
            var currentUserId = GetCurrentUserId();
            var donViId = GetCurrentDonViId();

            if (currentUserId == 0)
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập" });

            var result = await _soManhService.CreateSoManhAsync(request, currentUserId, donViId);
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
    }
}