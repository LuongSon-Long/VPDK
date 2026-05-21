using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class QuyTrinhRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public QuyTrinhRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        // QuyTrinhXuLy - dùng QuyTrinhXuLies (có 'es')
        public async Task<List<QuyTrinhXuLy>> GetAllQuyTrinhAsync() => await _context.QuyTrinhXuLies.ToListAsync();
        public async Task<QuyTrinhXuLy?> GetQuyTrinhByIdAsync(int id) => await _context.QuyTrinhXuLies.FindAsync(id);
        public async Task AddQuyTrinhAsync(QuyTrinhXuLy entity) => await _context.QuyTrinhXuLies.AddAsync(entity);
        public void UpdateQuyTrinh(QuyTrinhXuLy entity) => _context.QuyTrinhXuLies.Update(entity);
        public void DeleteQuyTrinh(QuyTrinhXuLy entity) => _context.QuyTrinhXuLies.Remove(entity);

        // BuocQuyTrinh - dùng QuyTrinhXuLyBuocQuyTrinhs
        public async Task<List<QuyTrinhXuLyBuocQuyTrinh>> GetBuocByQuyTrinhIdAsync(int idQuyTrinh)
            => await _context.QuyTrinhXuLyBuocQuyTrinhs.Where(b => b.IdquyTrinhXuLy == idQuyTrinh).ToListAsync();

        public async Task<QuyTrinhXuLyBuocQuyTrinh?> GetBuocByIdAsync(int id)
            => await _context.QuyTrinhXuLyBuocQuyTrinhs.FindAsync(id);

        public async Task AddBuocAsync(QuyTrinhXuLyBuocQuyTrinh entity)
            => await _context.QuyTrinhXuLyBuocQuyTrinhs.AddAsync(entity);

        public void DeleteBuoc(QuyTrinhXuLyBuocQuyTrinh entity)
            => _context.QuyTrinhXuLyBuocQuyTrinhs.Remove(entity);

        public async Task<List<int>> GetBuocIdsByQuyTrinhIdAsync(int idQuyTrinh)
            => await _context.QuyTrinhXuLyBuocQuyTrinhs
                .Where(b => b.IdquyTrinhXuLy == idQuyTrinh)
                .Select(b => b.Id)
                .ToListAsync();

        // NhayBuoc - dùng QuyTrinhXuLyNhayBuocs
        public async Task<List<QuyTrinhXuLyNhayBuoc>> GetNhayBuocByBuocIdsAsync(List<int> buocIds)
            => await _context.QuyTrinhXuLyNhayBuocs
                .Where(n => buocIds.Contains(n.IdbuocQuyTrinh))
                .ToListAsync();

        public async Task<QuyTrinhXuLyNhayBuoc?> GetNhayBuocByIdAsync(int id)
            => await _context.QuyTrinhXuLyNhayBuocs.FindAsync(id);

        public async Task AddNhayBuocAsync(QuyTrinhXuLyNhayBuoc entity)
            => await _context.QuyTrinhXuLyNhayBuocs.AddAsync(entity);

        public void DeleteNhayBuoc(QuyTrinhXuLyNhayBuoc entity)
            => _context.QuyTrinhXuLyNhayBuocs.Remove(entity);

        public void DeleteNhayBuocRange(IEnumerable<QuyTrinhXuLyNhayBuoc> entities)
            => _context.QuyTrinhXuLyNhayBuocs.RemoveRange(entities);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}