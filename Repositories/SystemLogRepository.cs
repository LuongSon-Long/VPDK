using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class SystemLogRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public SystemLogRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(int? userId, string action, string details)
        {
            var log = new SystemLog
            {
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            };

            await _context.SystemLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SystemLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _context.SystemLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<List<SystemLog>> GetAllLogsAsync(int page = 1, int pageSize = 50)
        {
            return await _context.SystemLogs
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}