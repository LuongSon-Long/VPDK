using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class LuuTruRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public LuuTruRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<DangKyDoDacLuuTru?> GetByHoSoIdAsync(int idDangKyDoDac)
        {
            return await _context.DangKyDoDacLuuTrus
                .FirstOrDefaultAsync(x => x.IddangKyDoDac == idDangKyDoDac);
        }

        public async Task AddAsync(DangKyDoDacLuuTru entity)
        {
            await _context.DangKyDoDacLuuTrus.AddAsync(entity);
        }

        public void Update(DangKyDoDacLuuTru entity)
        {
            _context.DangKyDoDacLuuTrus.Update(entity);
        }

        public async Task<List<string>> GetDistinctKhoAsync()
        {
            return await _context.DangKyDoDacLuuTrus
                .Where(x => x.Kho != null)
                .Select(x => x.Kho!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetGiaByKhoAsync(string kho)
        {
            return await _context.DangKyDoDacLuuTrus
                .Where(x => x.Kho == kho && x.Gia != null)
                .Select(x => x.Gia!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<DangKyDoDacLuuTru?> GetLastRecordByLocationAsync(string kho, string gia, string ngan)
        {
            return await _context.DangKyDoDacLuuTrus
                .Where(x => x.Kho == kho && x.Gia == gia && x.Ngan == ngan)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}