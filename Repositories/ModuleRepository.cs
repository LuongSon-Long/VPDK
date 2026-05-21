using Microsoft.EntityFrameworkCore;
using HeThongQuanLyVanPhong.Models;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class ModuleRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public ModuleRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<List<Module>> GetAllAsync()
        {
            return await _context.Modules.ToListAsync();
        }

        public async Task<Module?> GetByIdAsync(int id)
        {
            return await _context.Modules.FindAsync(id);
        }

        public async Task<List<string>> GetModuleNamesByIdsAsync(List<int> moduleIds)
        {
            return await _context.Modules
                .Where(m => moduleIds.Contains(m.Id))
                .Select(m => m.TenModule ?? "")
                .ToListAsync();
        }
    }
}