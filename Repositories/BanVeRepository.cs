using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class BanVeRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public BanVeRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<List<DangKyDoDacBanVe>> GetByHoSoIdAsync(int idDangKyDoDac)
        {
            return await _context.DangKyDoDacBanVes
                .Where(x => x.IddangKyDoDac == idDangKyDoDac)
                .ToListAsync();
        }

        public async Task<DangKyDoDacBanVe?> GetByIdAsync(int id)
        {
            return await _context.DangKyDoDacBanVes.FindAsync(id);
        }

        public async Task AddAsync(DangKyDoDacBanVe entity)
        {
            await _context.DangKyDoDacBanVes.AddAsync(entity);
        }

        public void Update(DangKyDoDacBanVe entity)
        {
            _context.DangKyDoDacBanVes.Update(entity);
        }

        public void Delete(DangKyDoDacBanVe entity)
        {
            _context.DangKyDoDacBanVes.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DangKyDoDacBanVes.AnyAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}