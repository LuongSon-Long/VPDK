using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class XuatFileController : ControllerBase
    {
        private readonly XuatFileService _xuatFileService;

        public XuatFileController(XuatFileService xuatFileService)
        {
            _xuatFileService = xuatFileService;
        }

        [HttpPost("excel-chitiet")]
        public async Task<IActionResult> XuatExcelChiTiet([FromBody] XuatExcelRequestDto request)
        {
            var bytes = await _xuatFileService.XuatExcelChiTietAsync(request);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BaoCao_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        [HttpPost("excel-thanhtoan")]
        public async Task<IActionResult> XuatExcelThanhToan([FromBody] XuatExcelThanhToanRequestDto request)
        {
            var bytes = await _xuatFileService.XuatExcelThanhToanAsync(request);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BaoCao_ThanhToan_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        [HttpPost("word-thanhly")]
        public async Task<IActionResult> XuatWordThanhLy([FromForm] XuatWordThanhLyRequestDto request, IFormFile templateFile)
        {
            if (templateFile == null)
                return BadRequest("Vui lòng chọn file mẫu");

            var bytes = await _xuatFileService.XuatWordThanhLyAsync(request, templateFile);
            string fileName = !string.IsNullOrEmpty(request.FileNameOut) ? request.FileNameOut : "HoSoThanhLy";
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{fileName}.docx");
        }
    }
}