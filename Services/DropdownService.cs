using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Services
{
    public class DropdownService
    {
        private readonly DropdownRepository _dropdownRepo;

        public DropdownService(DropdownRepository dropdownRepo)
        {
            _dropdownRepo = dropdownRepo;
        }

        public async Task<List<XaByTinhDto>> GetXaByTinhAsync(int tinhId)
        {
            var xas = await _dropdownRepo.GetXaByTinhAsync(tinhId);
            return xas.Select(x => new XaByTinhDto
            {
                Id = x.Id,
                MaXa = x.MaXa,
                TenXa = x.TenXa
            }).ToList();
        }

        public async Task<List<DonViByTinhDto>> GetDonViByTinhAsync(int idTinh)
        {
            var donVis = await _dropdownRepo.GetDonViByTinhAsync(idTinh);
            return donVis.Select(x => new DonViByTinhDto
            {
                Id = x.Id,
                TenDonVi = x.TenDonVi
            }).ToList();
        }

        public async Task<List<DropdownDto>> GetAllDonViAsync()
        {
            var donVis = await _dropdownRepo.GetAllDonViAsync();
            return donVis.Select(x => new DropdownDto
            {
                Id = x.Id,
                Ten = x.TenDonVi
            }).ToList();
        }

        public async Task<List<DropdownDto>> GetTaiKhoanByDonViAsync(int idDonVi)
        {
            var taiKhoans = await _dropdownRepo.GetTaiKhoanByDonViAsync(idDonVi);
            return taiKhoans.Select(x => new DropdownDto
            {
                Id = x.Id,
                Ten = x.HoVaTen ?? x.TenTaiKhoan
            }).ToList();
        }

        public async Task<List<DropdownDto>> GetAllTinhAsync()
        {
            var tinhs = await _dropdownRepo.GetAllTinhAsync();
            return tinhs.Select(x => new DropdownDto
            {
                Id = x.Id,
                Ten = x.TenTinh
            }).ToList();
        }

        public async Task<List<DropdownDto>> GetAllLoaiBanVeAsync()
        {
            var loaiBanVes = await _dropdownRepo.GetAllLoaiBanVeAsync();
            return loaiBanVes.Select(x => new DropdownDto
            {
                Id = x.Id,
                Ten = x.TenLoaiBanVe
            }).ToList();
        }

        public async Task<List<DropdownDto>> GetAllTrangThaiBanVeAsync()
        {
            var trangThais = await _dropdownRepo.GetAllTrangThaiBanVeAsync();
            return trangThais.Select(x => new DropdownDto
            {
                Id = x.Id,
                Ten = x.TrangThai
            }).ToList();
        }
    }
}