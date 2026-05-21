using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class DonGiaRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public DonGiaRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetVanBanQuyDinhGiaAsync()
        {
            return await _context.DangKyDoDacDonGia
                .Where(x => x.VanBanQuyDinhGia != null)
                .Select(x => x.VanBanQuyDinhGia!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetLoaiBanVeByVanBanAsync(string vanBan)
        {
            return await _context.DangKyDoDacDonGia
                .Where(x => x.VanBanQuyDinhGia == vanBan && x.LoaiBanVe != null)
                .Select(x => x.LoaiBanVe!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetLoaiKhuVucByVanBanAndLoaiBanVeAsync(string vanBan, string loaiBanVe)
        {
            return await _context.DangKyDoDacDonGia
                .Where(x => x.VanBanQuyDinhGia == vanBan && x.LoaiBanVe == loaiBanVe && x.LoaiKhuVuc != null)
                .Select(x => x.LoaiKhuVuc!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<DangKyDoDacDonGium>> GetChiTietDonGiaAsync(string vanBan, string loaiBanVe, string loaiKhuVuc)
        {
            return await _context.DangKyDoDacDonGia
                .Where(x => x.VanBanQuyDinhGia == vanBan
                         && x.LoaiBanVe == loaiBanVe
                         && x.LoaiKhuVuc == loaiKhuVuc)
                .ToListAsync();
        }
    }
}