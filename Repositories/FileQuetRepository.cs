using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class FileQuetRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public FileQuetRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<List<FileHoSoDoDac>> GetFilesByHoSoIdAsync(int idHoSo)
        {
            return await _context.FileHoSoDoDacs
                .Where(x => x.IddangKyDoDac == idHoSo)
                .ToListAsync();
        }

        public async Task<FileHoSoDoDac?> GetFileByIdAsync(int id)
        {
            return await _context.FileHoSoDoDacs
                .Include(x => x.IddangKyDoDacNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddFileAsync(FileHoSoDoDac entity)
        {
            await _context.FileHoSoDoDacs.AddAsync(entity);
        }

        public void DeleteFile(FileHoSoDoDac entity)
        {
            _context.FileHoSoDoDacs.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}