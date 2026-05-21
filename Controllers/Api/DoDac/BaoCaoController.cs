using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class BaoCaoController : ControllerBase
    {
        private readonly BaoCaoService _baoCaoService;

        public BaoCaoController(BaoCaoService baoCaoService)
        {
            _baoCaoService = baoCaoService;
        }

        [HttpPost("thongke")]
        public async Task<IActionResult> GetThongKe([FromBody] BaoCaoRequestDto request)
        {
            if (string.IsNullOrEmpty(request.TuNgay) || string.IsNullOrEmpty(request.DenNgay))
                return BadRequest("Vui lòng cung cấp khoảng thời gian");
            var data = await _baoCaoService.GetThongKeAsync(request);
            return Ok(data);
        }

        [HttpPost("chitiet")]
        public async Task<IActionResult> GetDanhSachChiTiet([FromBody] BaoCaoRequestDto request, [FromQuery] string? trangThai, [FromQuery] int? idNhanVien)
        {
            var data = await _baoCaoService.GetDanhSachChiTietAsync(request, trangThai, idNhanVien);
            return Ok(data);
        }
    }
}