using HeThongQuanLyVanPhong.DTOs.DoDac;
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

        public async Task<List<LuuTruResponseDto>> GetAllLuuTruAsync(string? search)
        {
            // Join thủ công giữa 2 bảng để lấy Tên người đăng ký
            var query = from lt in _context.DangKyDoDacLuuTrus
                        join hs in _context.DangKyDoDacs on lt.IddangKyDoDac equals hs.Id into hsJoin
                        from hs in hsJoin.DefaultIfEmpty() // Left Join
                        select new { lt, hs };

            // Tìm kiếm (nếu cần)
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x =>
                    (x.hs != null && (x.hs.NguoiDangKy ?? "").ToLower().Contains(s)) ||
                    (x.lt.IddangKyDoDac.HasValue && x.lt.IddangKyDoDac.Value.ToString().Contains(s))
                );
            }

            // MAP TRỰC TIẾP VÀO DTO
            return await query.OrderByDescending(x => x.lt.Id).Take(100)
                .Select(x => new LuuTruResponseDto
                {
                    Id = x.lt.Id,
                    IDDangKyDoDac = x.lt.IddangKyDoDac,
                    // Gán trực tiếp từ bảng hs, nếu null thì là "Không tên"
                    NguoiDangKy = x.hs != null ? x.hs.NguoiDangKy : "Không tên",
                    Kho = x.lt.Kho,
                    Gia = x.lt.Gia,
                    Ngan = x.lt.Ngan,
                    SoHSLuu = x.lt.SoHsluu ?? "---"
                }).ToListAsync();
        }

        public async Task<DangKyDoDacLuuTru?> GetByIdAsync(int id)
        {
            return await _context.DangKyDoDacLuuTrus.FindAsync(id);
        }

        public void Remove(DangKyDoDacLuuTru entity)
        {
            _context.DangKyDoDacLuuTrus.Remove(entity);
        }

        public async Task<List<string>> GetNganByKhoGiaAsync(string kho, string gia)
        {
            return await _context.DangKyDoDacLuuTrus
                .Where(x => x.Kho == kho && x.Gia == gia && x.Ngan != null)
                .Select(x => x.Ngan!) 
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<LuuTruResponseDto>> FindAsync(string kho, string gia, string ngan)
        {
            // 1. Thực hiện Join ngay tại đây để lấy thông tin hồ sơ
            var query = from lt in _context.DangKyDoDacLuuTrus
                        join hs in _context.DangKyDoDacs on lt.IddangKyDoDac equals hs.Id into hsJoin
                        from hs in hsJoin.DefaultIfEmpty()
                        select new { lt, hs };

            // 2. Lọc theo điều kiện
            if (!string.IsNullOrEmpty(kho)) query = query.Where(x => x.lt.Kho == kho);
            if (!string.IsNullOrEmpty(gia)) query = query.Where(x => x.lt.Gia == gia);
            if (!string.IsNullOrEmpty(ngan)) query = query.Where(x => x.lt.Ngan == ngan);

            // 3. Select ra đúng DTO chứa tên người đăng ký
            return await query.Select(x => new LuuTruResponseDto
            {
                Id = x.lt.Id,
                IDDangKyDoDac = x.lt.IddangKyDoDac,
                NguoiDangKy = x.hs != null ? x.hs.NguoiDangKy : "Không tên",
                Kho = x.lt.Kho,
                Gia = x.lt.Gia,
                Ngan = x.lt.Ngan,
                SoHSLuu = x.lt.SoHsluu ?? "---"
            }).ToListAsync();
        }
    }
}