using HeThongQuanLyVanPhong.DTOs.DoDac;
using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class SoManhRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public SoManhRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<int?> GetMaxSoHieuByXaAndNamAsync(string maXa, int nam)
        {
            return await _context.DangKyDoDacSoManhTds
                .Where(x => x.MaXa == maXa && x.Nam == nam)
                .MaxAsync(x => (int?)x.ShbanVe);
        }

        public async Task AddAsync(DangKyDoDacSoManhTd entity)
        {
            await _context.DangKyDoDacSoManhTds.AddAsync(entity);
        }

        public async Task<List<DangKyDoDacSoManhTd>> GetLichSuByMaXaAndNamAsync(string maXa, int nam)
        {
            return await _context.DangKyDoDacSoManhTds
                .Where(x => x.MaXa == maXa && x.Nam == nam)
                .OrderByDescending(x => x.ShbanVe)
                .ToListAsync();
        }

        public async Task<List<SoHieuMaxDto>> GetSoHieuMaxByTinhAndNamAsync(int idTinh, int nam)
        {
            var query = from xa in _context.Dvhcxas
                        join tinh in _context.DonViHanhChinhTinhs on xa.Idtinh equals tinh.Id
                        where tinh.Id == idTinh
                        select new SoHieuMaxDto
                        {
                            IDXa = xa.Id,
                            TenXa = xa.TenXa,
                            MaXa = xa.MaXa,
                            SoHieuMax = _context.DangKyDoDacSoManhTds
                                        .Where(s => s.MaXa == xa.MaXa && s.Nam == nam)
                                        .Max(s => (int?)s.ShbanVe) ?? 0
                        };

            return await query.OrderBy(x => x.TenXa).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}