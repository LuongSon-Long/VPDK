using HeThongQuanLyVanPhong.DTOs.DoDac;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class HoSoDoDacController : ApiControllerBase
    {
        private readonly HoSoDoDacService _hoSoService;

        public HoSoDoDacController(HoSoDoDacService hoSoService)
        {
            _hoSoService = hoSoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(bool onlyChuaKetThuc = true)
        {
            var list = await _hoSoService.GetByTaiKhoanAsync(
                CurrentUserId, CurrentDonViId, CurrentTinhId, CurrentChucVu, onlyChuaKetThuc);
            return Ok(list);
        }

        [HttpGet("next-sohopdong/{idDonVi}")]
        public async Task<IActionResult> GetNextSoHopDong(int idDonVi)
        {
            var so = await _hoSoService.GetNextSoHopDongAsync(idDonVi, CurrentUserId);
            return Ok(new { soHopDong = so });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHoSoDoDacDto dto)
        {
            var id = await _hoSoService.CreateHoSoAsync(dto, CurrentUserId, CurrentDonViId);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHoSoDoDacDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var result = await _hoSoService.UpdateHoSoAsync(dto, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpPost("chuyen-buoc")]
        public async Task<IActionResult> ChuyenBuoc([FromBody] ChuyenBuocDto dto)
        {
            var result = await _hoSoService.ChuyenBuocAsync(dto, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpPost("{id}/ketthuc")]
        public async Task<IActionResult> KetThuc(int id)
        {
            var result = await _hoSoService.KetThucHoSoAsync(id, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpGet("{id}/lichsu")]
        public async Task<IActionResult> GetLichSu(int id)
        {
            var logs = await _hoSoService.GetLichSuAsync(id);
            var result = logs.Select(x => new
            {
                x.Id,
                x.Details,
                x.Timestamp,
                TenCanBo = x.User?.HoVaTen ?? $"User #{x.UserId}"
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _hoSoService.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Không tìm thấy hồ sơ" });
            return Ok(item);
        }
    }
}
