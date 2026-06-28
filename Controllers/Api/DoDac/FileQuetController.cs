using HeThongQuanLyVanPhong.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class FileQuetController : ApiControllerBase
    {
        private readonly FileQuetService _fileQuetService;

        public FileQuetController(FileQuetService fileQuetService)
        {
            _fileQuetService = fileQuetService;
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
            var result = await _fileQuetService.UploadFileAsync(idHoSo, noiDung ?? "", file, CurrentUserId);
            if (!result.success)
                return BadRequest(new { success = false, message = result.message });

            return Ok(new { success = true, message = result.message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await _fileQuetService.DeleteFileAsync(id, CurrentUserId);
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
