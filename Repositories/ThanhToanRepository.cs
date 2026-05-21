using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class ThanhToanRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public ThanhToanRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<DangKyDoDacBanVeThanhToan?> GetThanhToanByBanVeIdAsync(int idBanVe)
        {
            return await _context.DangKyDoDacBanVeThanhToans
                .FirstOrDefaultAsync(x => x.IdbanVe == idBanVe);
        }

        public async Task AddThanhToanAsync(DangKyDoDacBanVeThanhToan entity)
        {
            await _context.DangKyDoDacBanVeThanhToans.AddAsync(entity);
        }

        public void UpdateThanhToan(DangKyDoDacBanVeThanhToan entity)
        {
            _context.DangKyDoDacBanVeThanhToans.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}