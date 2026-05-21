using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Services
{
    public class DonGiaThanhToanService
    {
        private readonly DonGiaRepository _donGiaRepo;
        private readonly ThanhToanRepository _thanhToanRepo;
        private readonly SystemLogRepository _logRepo;
        private readonly HeThongQuanLyVanPhongContext _context;

        public DonGiaThanhToanService(
            DonGiaRepository donGiaRepo,
            ThanhToanRepository thanhToanRepo,
            SystemLogRepository logRepo,
            HeThongQuanLyVanPhongContext context)
        {
            _donGiaRepo = donGiaRepo;
            _thanhToanRepo = thanhToanRepo;
            _logRepo = logRepo;
            _context = context;
        }

        public async Task<List<string>> GetVanBanQuyDinhGiaAsync()
        {
            return await _donGiaRepo.GetVanBanQuyDinhGiaAsync();
        }

        public async Task<List<string>> GetLoaiBanVeByVanBanAsync(string vanBan)
        {
            if (string.IsNullOrEmpty(vanBan))
                return new List<string>();
            return await _donGiaRepo.GetLoaiBanVeByVanBanAsync(vanBan);
        }

        public async Task<List<string>> GetLoaiKhuVucAsync(string vanBan, string loaiBanVe)
        {
            if (string.IsNullOrEmpty(vanBan) || string.IsNullOrEmpty(loaiBanVe))
                return new List<string>();
            return await _donGiaRepo.GetLoaiKhuVucByVanBanAndLoaiBanVeAsync(vanBan, loaiBanVe);
        }

        public async Task<List<ChiTietDonGiaDto>> GetChiTietDonGiaAsync(string vanBan, string loaiBanVe, string loaiKhuVuc)
        {
            if (string.IsNullOrEmpty(vanBan) || string.IsNullOrEmpty(loaiBanVe) || string.IsNullOrEmpty(loaiKhuVuc))
                return new List<ChiTietDonGiaDto>();

            var list = await _donGiaRepo.GetChiTietDonGiaAsync(vanBan, loaiBanVe, loaiKhuVuc);
            return list.Select(x => new ChiTietDonGiaDto
            {
                Id = x.Id,
                DinhMucTinh = x.DinhMucTinh,
                SoTien = x.SoTien,
                ChiPhiLaoDong = x.ChiPhiLaoDong,
                TienLamTron = x.TienLamTron
            }).ToList();
        }

        public async Task<bool> SaveThanhToanBanVeAsync(ThanhToanBanVeDto request, int currentUserId)
        {
            var user = await _context.TaiKhoans.FindAsync(currentUserId);
            int? donViId = user?.IddonViCongTac;

            var banVe = await _context.DangKyDoDacBanVes.FindAsync(request.IdBanVe);
            if (banVe == null)
                return false;

            var tt = await _thanhToanRepo.GetThanhToanByBanVeIdAsync(request.IdBanVe);

            if (tt == null)
            {
                tt = new DangKyDoDacBanVeThanhToan
                {
                    IdbanVe = request.IdBanVe,
                    IddangKyDoDac = banVe.IddangKyDoDac,
                    IddonGia = request.IdDonGia,
                    KyHieuHoaDon = request.KyHieuHoaDon,
                    SoHoaDon = request.SoHoaDon,
                    DaThanhToan = request.DaThanhToan,
                    IdtaiKhoan = currentUserId,
                    IddonViCongTac = donViId
                };

                if (DateTime.TryParse(request.NgayHoaDon, out DateTime parsedDate))
                    tt.NgayHoaDon = DateOnly.FromDateTime(parsedDate);

                await _thanhToanRepo.AddThanhToanAsync(tt);
            }
            else
            {
                tt.IddonGia = request.IdDonGia;
                tt.KyHieuHoaDon = request.KyHieuHoaDon;
                tt.SoHoaDon = request.SoHoaDon;
                tt.DaThanhToan = request.DaThanhToan;
                tt.IdtaiKhoan = currentUserId;
                tt.IddonViCongTac = donViId;

                if (DateTime.TryParse(request.NgayHoaDon, out DateTime parsedDate))
                    tt.NgayHoaDon = DateOnly.FromDateTime(parsedDate);

                _thanhToanRepo.UpdateThanhToan(tt);
            }

            await _thanhToanRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Thanh toán", $"Thanh toán bản vẽ ID {request.IdBanVe}");
            return true;
        }

        public async Task<ThanhToanResponseDto?> GetThanhToanByBanVeAsync(int idBanVe)
        {
            var tt = await _thanhToanRepo.GetThanhToanByBanVeIdAsync(idBanVe);
            if (tt == null) return null;

            var dg = await _context.DangKyDoDacDonGia.FindAsync(tt.IddonGia);

            return new ThanhToanResponseDto
            {
                IdDonGia = tt.IddonGia ?? 0,
                KyHieu = tt.KyHieuHoaDon,
                SoHD = tt.SoHoaDon,
                NgayHD = tt.NgayHoaDon.HasValue ? tt.NgayHoaDon.Value.ToString("yyyy-MM-dd") : "",
                DaThanhToan = tt.DaThanhToan,
                VanBan = dg?.VanBanQuyDinhGia,
                LoaiBanVe = dg?.LoaiBanVe,
                LoaiKhuVuc = dg?.LoaiKhuVuc
            };
        }
    }
}