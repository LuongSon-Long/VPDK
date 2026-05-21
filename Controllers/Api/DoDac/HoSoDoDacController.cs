using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class HoSoDoDacController : ControllerBase
    {
        private readonly HoSoDoDacService _hoSoService;
        public HoSoDoDacController(HoSoDoDacService hoSoService)
        {
            _hoSoService = hoSoService;
        }

        private int GetCurrentUserId() => HttpContext.Session.GetInt32("UserId") ?? 0;
        private int GetCurrentDonViId() => HttpContext.Session.GetInt32("UserDonViId") ?? 0;

        [HttpGet]
        public async Task<IActionResult> GetList(bool onlyChuaKetThuc = true)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var list = await _hoSoService.GetByTaiKhoanAsync(userId, onlyChuaKetThuc);
            return Ok(list);
        }

        [HttpGet("next-sohopdong/{idDonVi}")]
        public async Task<IActionResult> GetNextSoHopDong(int idDonVi)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var so = await _hoSoService.GetNextSoHopDongAsync(idDonVi, userId);
            return Ok(new { soHopDong = so });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHoSoDoDacDto dto)
        {
            var userId = GetCurrentUserId();
            var donViId = GetCurrentDonViId();
            if (userId == 0) return Unauthorized();
            var id = await _hoSoService.CreateHoSoAsync(dto, userId, donViId);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHoSoDoDacDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var result = await _hoSoService.UpdateHoSoAsync(dto, userId);
            return Ok(new { success = result });
        }

        [HttpPost("chuyen-buoc")]
        public async Task<IActionResult> ChuyenBuoc([FromBody] ChuyenBuocDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var result = await _hoSoService.ChuyenBuocAsync(dto, userId);
            return Ok(new { success = result });
        }

        [HttpPost("{id}/ketthuc")]
        public async Task<IActionResult> KetThuc(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var result = await _hoSoService.KetThucHoSoAsync(id, userId);
            return Ok(new { success = result });
        }

        [HttpGet("{id}/lichsu")]
        public async Task<IActionResult> GetLichSu(int id)
        {
            var logs = await _hoSoService.GetLichSuAsync(id);
            var result = logs.Select(x => new {
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
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var item = await _hoSoService.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Không tìm thấy hồ sơ" });
            return Ok(item);
        }
    }
}