using Microsoft.AspNetCore.Mvc;
using HeThongQuanLyVanPhong.Services;

namespace HeThongQuanLyVanPhong.Controllers.Api.DoDac
{
    [Route("api/dodac/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        private readonly DropdownService _dropdownService;

        public DropdownController(DropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        [HttpGet("xa-by-tinh/{tinhId}")]
        public async Task<IActionResult> GetXaByTinh(int tinhId)
        {
            var result = await _dropdownService.GetXaByTinhAsync(tinhId);
            return Ok(result);
        }

        [HttpGet("donvi-by-tinh/{idTinh}")]
        public async Task<IActionResult> GetDonViByTinh(int idTinh)
        {
            var result = await _dropdownService.GetDonViByTinhAsync(idTinh);
            return Ok(result);
        }

        [HttpGet("all-donvi")]
        public async Task<IActionResult> GetAllDonVi()
        {
            var result = await _dropdownService.GetAllDonViAsync();
            return Ok(result);
        }

        [HttpGet("taikhoan-by-donvi/{idDonVi}")]
        public async Task<IActionResult> GetTaiKhoanByDonVi(int idDonVi)
        {
            var result = await _dropdownService.GetTaiKhoanByDonViAsync(idDonVi);
            return Ok(result);
        }

        [HttpGet("all-tinh")]
        public async Task<IActionResult> GetAllTinh()
        {
            var result = await _dropdownService.GetAllTinhAsync();
            return Ok(result);
        }

        [HttpGet("all-loai-banve")]
        public async Task<IActionResult> GetAllLoaiBanVe()
        {
            var result = await _dropdownService.GetAllLoaiBanVeAsync();
            return Ok(result);
        }

        [HttpGet("all-trangthai-banve")]
        public async Task<IActionResult> GetAllTrangThaiBanVe()
        {
            var result = await _dropdownService.GetAllTrangThaiBanVeAsync();
            return Ok(result);
        }
    }
}