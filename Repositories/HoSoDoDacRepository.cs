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

        public async Task<List<DangKyDoDac>> GetByTaiKhoanAsync()
        {
            return await _context.DangKyDoDacs
                .Include(x => x.IddonViCongTacNavigation)
                .Include(x => x.IdxaNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdtaiKhoanDoNavigation)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
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