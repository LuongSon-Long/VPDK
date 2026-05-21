using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class HoSoDoDacRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public HoSoDoDacRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<List<DangKyDoDac>> GetByTaiKhoanAsync(int taiKhoanId, bool onlyChuaKetThuc = true)
        {
            var query = _context.DangKyDoDacs
                .Include(x => x.IddonViCongTacNavigation)
                .Include(x => x.IdxaNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdtaiKhoanDoNavigation)
                .Where(x => x.IdtaiKhoan == taiKhoanId);
            if (onlyChuaKetThuc)
                query = query.Where(x => x.TrangThaiDo != "KetThuc");
            return await query.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<DangKyDoDac?> GetByIdAsync(int id)
        {
            return await _context.DangKyDoDacs
                .Include(x => x.IddonViCongTacNavigation)
                .Include(x => x.IdxaNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdtaiKhoanDoNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(DangKyDoDac entity)
        {
            await _context.DangKyDoDacs.AddAsync(entity);
        }

        public void Update(DangKyDoDac entity)
        {
            _context.DangKyDoDacs.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}