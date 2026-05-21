using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.QuyTrinh;

namespace HeThongQuanLyVanPhong.Controllers.Api.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class QuyTrinhController : ControllerBase
    {
        private readonly QuyTrinhService _quyTrinhService;

        public QuyTrinhController(QuyTrinhService quyTrinhService)
        {
            _quyTrinhService = quyTrinhService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        // GET: api/admin/QuyTrinh
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _quyTrinhService.GetAllQuyTrinhAsync();
            return Ok(list);
        }

        // GET: api/admin/QuyTrinh/{id}/details
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetWorkflowDetails(int id)
        {
            var data = await _quyTrinhService.GetWorkflowDetailsAsync(id);
            if (data == null) return NotFound(new { message = "Không tìm thấy quy trình" });
            return Ok(data);
        }

        // POST: api/admin/QuyTrinh
        [HttpPost]
        public async Task<IActionResult> SaveQuyTrinh([FromBody] SaveQuyTrinhRequestDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var (success, message) = await _quyTrinhService.SaveQuyTrinhAsync(request, currentUserId);
            if (!success) return BadRequest(new { success = false, message });
            return Ok(new { success = true });
        }

        // POST: api/admin/QuyTrinh/buoc
        [HttpPost("buoc")]
        public async Task<IActionResult> SaveBuoc([FromBody] BuocQuyTrinhDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var result = await _quyTrinhService.SaveBuocAsync(request, currentUserId);
            return Ok(new { success = result });
        }

        // POST: api/admin/QuyTrinh/nhaybuoc
        [HttpPost("nhaybuoc")]
        public async Task<IActionResult> SaveNhayBuoc([FromBody] NhayBuocDto request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var result = await _quyTrinhService.SaveNhayBuocAsync(request, currentUserId);
            return Ok(new { success = result });
        }

        // DELETE: api/admin/QuyTrinh/buoc/{id}
        [HttpDelete("buoc/{id}")]
        public async Task<IActionResult> DeleteBuoc(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var result = await _quyTrinhService.DeleteBuocAsync(id, currentUserId);
            return Ok(new { success = result });
        }

        // DELETE: api/admin/QuyTrinh/nhaybuoc/{id}
        [HttpDelete("nhaybuoc/{id}")]
        public async Task<IActionResult> DeleteNhayBuoc(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var result = await _quyTrinhService.DeleteNhayBuocAsync(id, currentUserId);
            return Ok(new { success = result });
        }

        // DELETE: api/admin/QuyTrinh/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuyTrinh(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0) return Unauthorized();

            var result = await _quyTrinhService.DeleteQuyTrinhAsync(id, currentUserId);
            return Ok(new { success = result });
        }
    }
}