using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class DropdownRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public DropdownRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<List<Dvhcxa>> GetXaByTinhAsync(int tinhId)
        {
            return await _context.Dvhcxas
                .Where(x => x.Idtinh == tinhId)
                .OrderBy(x => x.TenXa)
                .ToListAsync();
        }

        public async Task<List<DonViCongTac>> GetDonViByTinhAsync(int idTinh)
        {
            return await _context.DonViCongTacs
                .Where(x => x.IddonViHanhChinhTinh == idTinh)
                .OrderBy(x => x.TenDonVi)
                .ToListAsync();
        }

        public async Task<List<DonViCongTac>> GetAllDonViAsync()
        {
            return await _context.DonViCongTacs
                .OrderBy(x => x.TenDonVi)
                .ToListAsync();
        }

        public async Task<List<TaiKhoan>> GetTaiKhoanByDonViAsync(int idDonVi)
        {
            return await _context.TaiKhoans
                .Where(x => x.IddonViCongTac == idDonVi)
                .OrderBy(x => x.HoVaTen)
                .ToListAsync();
        }

        public async Task<List<DonViHanhChinhTinh>> GetAllTinhAsync()
        {
            return await _context.DonViHanhChinhTinhs
                .OrderBy(x => x.TenTinh)
                .ToListAsync();
        }

        public async Task<List<LoaiBanVe>> GetAllLoaiBanVeAsync()
        {
            return await _context.LoaiBanVes
                .OrderBy(x => x.TenLoaiBanVe)
                .ToListAsync();
        }

        public async Task<List<TrangThaiBanVe>> GetAllTrangThaiBanVeAsync()
        {
            return await _context.TrangThaiBanVes
                .OrderBy(x => x.TrangThai)
                .ToListAsync();
        }
    }
}