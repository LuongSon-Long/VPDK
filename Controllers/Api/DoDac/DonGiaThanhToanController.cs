using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class DonGiaThanhToanController : ControllerBase
    {
        private readonly DonGiaThanhToanService _service;

        public DonGiaThanhToanController(DonGiaThanhToanService service)
        {
            _service = service;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        [HttpGet("vanban")]
        public async Task<IActionResult> GetVanBanQuyDinhGia()
        {
            var result = await _service.GetVanBanQuyDinhGiaAsync();
            return Ok(result);
        }

        [HttpGet("loaibanve")]
        public async Task<IActionResult> GetLoaiBanVeDonGia([FromQuery] string vanBan)
        {
            var result = await _service.GetLoaiBanVeByVanBanAsync(vanBan);
            return Ok(result);
        }

        [HttpGet("khu-vuc")]
        public async Task<IActionResult> GetLoaiKhuVucDonGia([FromQuery] string vanBan, [FromQuery] string loaiBanVe)
        {
            var result = await _service.GetLoaiKhuVucAsync(vanBan, loaiBanVe);
            return Ok(result);
        }

        [HttpGet("chitiet")]
        public async Task<IActionResult> GetChiTietDonGia([FromQuery] string vanBan, [FromQuery] string loaiBanVe, [FromQuery] string loaiKhuVuc)
        {
            var result = await _service.GetChiTietDonGiaAsync(vanBan, loaiBanVe, loaiKhuVuc);
            return Ok(result);
        }

        [HttpPost("thanhtoan")]
        public async Task<IActionResult> SaveThanhToanBanVe([FromBody] ThanhToanBanVeDto request)
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == 0)
                return Unauthorized();

            var result = await _service.SaveThanhToanBanVeAsync(request, currentUserId);
            return Ok(new { success = result });
        }

        [HttpGet("thanhtoan/{idBanVe}")]
        public async Task<IActionResult> GetThanhToanByBanVe(int idBanVe)
        {
            var result = await _service.GetThanhToanByBanVeAsync(idBanVe);
            return Ok(result);
        }
    }
}