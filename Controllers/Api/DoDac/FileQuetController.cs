using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class FileQuetController : ControllerBase
    {
        private readonly FileQuetService _fileQuetService;

        public FileQuetController(FileQuetService fileQuetService)
        {
            _fileQuetService = fileQuetService;
        }

        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        [HttpGet("by-hoso/{idHoSo}")]
        public async Task<IActionResult> GetFilesByHoSo(int idHoSo)
        {
            var result = await _fileQuetService.GetFilesByHoSoIdAsync(idHoSo);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] int idHoSo, [FromForm] string? noiDung, IFormFile file)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized();

            var result = await _fileQuetService.UploadFileAsync(idHoSo, noiDung ?? "", file, currentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
                return Unauthorized();

            var result = await _fileQuetService.DeleteFileAsync(id, currentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message });
        }

        [HttpGet("view/{id}")]
        public async Task<IActionResult> ViewFile(int id)
        {
            var (fileBytes, contentType, fileName) = await _fileQuetService.ViewFileAsync(id);
            if (fileBytes == null)
                return NotFound("File không tồn tại");

            return File(fileBytes, contentType, fileName);
        }
    }
}