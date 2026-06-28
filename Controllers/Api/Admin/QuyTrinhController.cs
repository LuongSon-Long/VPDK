using HeThongQuanLyVanPhong.Controllers.Api;
using HeThongQuanLyVanPhong.DTOs.QuyTrinh;
using HeThongQuanLyVanPhong.Filters;
using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [RequireSession(AllowGuest = false)]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class QuyTrinhController : ApiControllerBase
    {
        private readonly QuyTrinhService _quyTrinhService;

        public QuyTrinhController(QuyTrinhService quyTrinhService)
        {
            _quyTrinhService = quyTrinhService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _quyTrinhService.GetAllQuyTrinhAsync();
            return Ok(list);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetWorkflowDetails(int id)
        {
            var data = await _quyTrinhService.GetWorkflowDetailsAsync(id);
            if (data == null) return NotFound(new { message = "Không tìm thấy quy trình" });
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuyTrinh([FromBody] SaveQuyTrinhRequestDto request)
        {
            var (success, message) = await _quyTrinhService.SaveQuyTrinhAsync(request, CurrentUserId);
            if (!success) return BadRequest(new { success = false, message });
            return Ok(new { success = true });
        }

        [HttpPost("buoc")]
        public async Task<IActionResult> SaveBuoc([FromBody] BuocQuyTrinhDto request)
        {
            var result = await _quyTrinhService.SaveBuocAsync(request, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpPost("nhaybuoc")]
        public async Task<IActionResult> SaveNhayBuoc([FromBody] NhayBuocDto request)
        {
            var result = await _quyTrinhService.SaveNhayBuocAsync(request, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpDelete("buoc/{id}")]
        public async Task<IActionResult> DeleteBuoc(int id)
        {
            var result = await _quyTrinhService.DeleteBuocAsync(id, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpDelete("nhaybuoc/{id}")]
        public async Task<IActionResult> DeleteNhayBuoc(int id)
        {
            var result = await _quyTrinhService.DeleteNhayBuocAsync(id, CurrentUserId);
            return Ok(new { success = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuyTrinh(int id)
        {
            var result = await _quyTrinhService.DeleteQuyTrinhAsync(id, CurrentUserId);
            return Ok(new { success = result });
        }
    }
}
