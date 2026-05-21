using Microsoft.EntityFrameworkCore;
using HeThongQuanLyVanPhong.Models;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class TaiKhoanRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public TaiKhoanRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

       
        public IQueryable<TaiKhoan> GetQueryable()
        {
            return _context.TaiKhoans.AsQueryable();
        }

        public async Task<TaiKhoan?> GetByIdAsync(int id)
        {
            return await _context.TaiKhoans.FindAsync(id);
        }

        public async Task<TaiKhoan?> GetByUsernameAsync(string username)
        {
            return await _context.TaiKhoans
                .FirstOrDefaultAsync(t => t.TenTaiKhoan == username);
        }

        public async Task<TaiKhoan?> GetWithDetailsAsync(int id)
        {
            return await _context.TaiKhoans
                .Include(t => t.IdchucVuNavigation)
                .Include(t => t.IddonViCongTacNavigation)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<int>> GetModuleIdsByUserIdAsync(int userId)
        {
            return await _context.PhanQuyenModules
                .Where(pq => pq.IdtaiKhoan == userId)
                .Select(pq => pq.Idmodule ?? 0)
                .ToListAsync();
        }

        public async Task<TaiKhoan> AddAsync(TaiKhoan entity)
        {
            await _context.TaiKhoans.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TaiKhoan entity)
        {
            _context.TaiKhoans.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaiKhoan entity)
        {
            // Xóa phân quyền module trước
            var phanQuyenModules = _context.PhanQuyenModules.Where(p => p.IdtaiKhoan == entity.Id);
            _context.PhanQuyenModules.RemoveRange(phanQuyenModules);

            // Xóa tài khoản
            _context.TaiKhoans.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserModulesAsync(int userId, List<int> moduleIds)
        {
            var oldModules = _context.PhanQuyenModules.Where(p => p.IdtaiKhoan == userId);
            _context.PhanQuyenModules.RemoveRange(oldModules);

            foreach (var moduleId in moduleIds)
            {
                _context.PhanQuyenModules.Add(new PhanQuyenModule
                {
                    IdtaiKhoan = userId,
                    Idmodule = moduleId
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}