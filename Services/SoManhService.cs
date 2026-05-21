using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Services
{
    public class SoManhService
    {
        private readonly SoManhRepository _soManhRepo;
        private readonly SystemLogRepository _logRepo;
        private readonly HeThongQuanLyVanPhongContext _context;
        private static readonly object _lockObj = new object();

        public SoManhService(SoManhRepository soManhRepo, SystemLogRepository logRepo, HeThongQuanLyVanPhongContext context)
        {
            _soManhRepo = soManhRepo;
            _logRepo = logRepo;
            _context = context;
        }

        public async Task<object> CreateSoManhAsync(CreateSoManhDto request, int currentUserId, int donViId)
        {
            if (string.IsNullOrEmpty(request.MaXa))
                return new { success = false, message = "Mã xã không được để trống" };

            if (request.Nam <= 0 || request.Nam > DateTime.Now.Year + 1)
                return new { success = false, message = "Năm không hợp lệ" };

            var xaExists = await _context.Dvhcxas.AnyAsync(x => x.MaXa == request.MaXa);
            if (!xaExists)
                return new { success = false, message = $"Mã xã '{request.MaXa}' không tồn tại" };

            // ── FIX: luôn lấy IDDonViCongTac từ DB, không tin session ──
            var user = await _context.TaiKhoans.FindAsync(currentUserId);
            int? idDonViThucTe = user?.IddonViCongTac;  // nullable, đúng với FK

            lock (_lockObj)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    var maxSoHieu = _context.DangKyDoDacSoManhTds
                        .Where(x => x.MaXa == request.MaXa && x.Nam == request.Nam)
                        .Max(x => (int?)x.ShbanVe) ?? 0;

                    int soMoi = maxSoHieu + 1;

                    var record = new DangKyDoDacSoManhTd
                    {
                        MaXa = request.MaXa,
                        Nam = request.Nam,
                        ShbanVe = soMoi,
                        IdtaiKhoan = currentUserId,
                        IddonViCongTac = idDonViThucTe,  // dùng giá trị từ DB, có thể null
                        NgayLay = DateTime.Now
                    };

                    _context.DangKyDoDacSoManhTds.Add(record);
                    _context.SaveChanges();
                    transaction.Commit();

                    _ = Task.Run(async () =>
                        await _logRepo.AddLogAsync(currentUserId, "Cấp số mãnh",
                            $"Cấp số {soMoi} cho xã {request.MaXa} năm {request.Nam}")
                    );

                    return new { success = true, soMoi = soMoi };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new { success = false, message = $"Lỗi hệ thống: {ex.Message}" };
                }
            }
        }

        public async Task<List<LichSuCapSoDto>> GetLichSuCapSoAsync(string maXa, int nam)
        {
            if (string.IsNullOrEmpty(maXa))
                return new List<LichSuCapSoDto>();

            var list = await _soManhRepo.GetLichSuByMaXaAndNamAsync(maXa, nam);

            return list.Select(x => new LichSuCapSoDto
            {
                SHBanVe = x.ShbanVe,
                NgayLay = x.NgayLay.HasValue ? x.NgayLay.Value.ToString("dd/MM/yyyy HH:mm") : "",
                TenCanBo = _context.TaiKhoans.Where(u => u.Id == x.IdtaiKhoan).Select(u => u.HoVaTen).FirstOrDefault() ?? "N/A",
                TenDonVi = _context.DonViCongTacs.Where(d => d.Id == x.IddonViCongTac).Select(d => d.TenDonVi).FirstOrDefault() ?? "N/A"
            }).ToList();
        }

        public async Task<List<SoHieuMaxDto>> GetSoHieuMaxByTinhAndNamAsync(int idTinh, int nam)
        {
            if (idTinh <= 0)
                return new List<SoHieuMaxDto>();

            return await _soManhRepo.GetSoHieuMaxByTinhAndNamAsync(idTinh, nam);
        }
    }
}