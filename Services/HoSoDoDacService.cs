using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Services
{
    public class HoSoDoDacService
    {
        private readonly HoSoDoDacRepository _hoSoRepo;
        private readonly HeThongQuanLyVanPhongContext _context;

        public HoSoDoDacService(HoSoDoDacRepository hoSoRepo, HeThongQuanLyVanPhongContext context)
        {
            _hoSoRepo = hoSoRepo;
            _context = context;
        }

        private DateOnly? ParseDateOnly(string? dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return null;
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                return DateOnly.FromDateTime(dt);
            return null;
        }

        public async Task<List<HoSoDoDacDto>> GetByTaiKhoanAsync(int taiKhoanId, bool onlyChuaKetThuc = true)
        {
            var query = _context.DangKyDoDacs
                .Include(x => x.IddonViCongTacNavigation)
                .Include(x => x.IdxaNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdtaiKhoanDoNavigation)
                .Where(x => x.IdtaiKhoan == taiKhoanId);
            if (onlyChuaKetThuc)
                query = query.Where(x => x.TrangThaiDo != "KetThuc");
            var list = await query.OrderByDescending(x => x.Id).ToListAsync();
            return list.Select(x => new HoSoDoDacDto
            {
                Id = x.Id,
                IDDonViCongTac = x.IddonViCongTac,
                IDQuyTrinh = x.IdquyTrinh,
                IDBuocQuyTrinh = x.IdbuocQuyTrinh,
                IDTaiKhoan = x.IdtaiKhoan,
                TrangThaiDo = x.TrangThaiDo,
                SoHopDong = x.SoHopDong,
                NgayHopDong = x.NgayHopDong,
                NguoiDangKy = x.NguoiDangKy,
                CCCD = x.Cccd,
                SoDienThoai = x.SoDienThoai,
                SeriGCN = x.SeriGcn,
                MucDichDangKy = x.MucDichDangKy,
                DiaChiThuaDat = x.DiaChiThuaDat,
                IDXa = x.Idxa,
                IDTinh = x.Idtinh,
                GhiChu = x.GhiChu,
                IDTaiKhoanDo = x.IdtaiKhoanDo,
                SoPhieuGiao = x.SoPhieuGiao,
                NgayGiao = x.NgayGiao,
                NgayYeuCau = x.NgayYeuCau,
                NgayDo = x.NgayDo,
                NgayTraKetQua = x.NgayTraKetQua,
                TenDonViCongTac = x.IddonViCongTacNavigation?.TenDonVi,
                TenXa = x.IdxaNavigation?.TenXa,
                TenTinh = x.IdtinhNavigation?.TenTinh,
                TenNguoiXuLy = x.IdtaiKhoanDoNavigation?.HoVaTen
            }).ToList();
        }

        public async Task<HoSoDoDacDto?> GetByIdAsync(int id)
        {
            var x = await _hoSoRepo.GetByIdAsync(id);
            if (x == null) return null;
            return new HoSoDoDacDto
            {
                Id = x.Id,
                IDDonViCongTac = x.IddonViCongTac,
                IDQuyTrinh = x.IdquyTrinh,
                IDBuocQuyTrinh = x.IdbuocQuyTrinh,
                IDTaiKhoan = x.IdtaiKhoan,
                TrangThaiDo = x.TrangThaiDo,
                SoHopDong = x.SoHopDong,
                NgayHopDong = x.NgayHopDong,
                NguoiDangKy = x.NguoiDangKy,
                CCCD = x.Cccd,
                SoDienThoai = x.SoDienThoai,
                SeriGCN = x.SeriGcn,
                MucDichDangKy = x.MucDichDangKy,
                DiaChiThuaDat = x.DiaChiThuaDat,
                IDXa = x.Idxa,
                IDTinh = x.Idtinh,
                GhiChu = x.GhiChu,
                IDTaiKhoanDo = x.IdtaiKhoanDo,
                SoPhieuGiao = x.SoPhieuGiao,
                NgayGiao = x.NgayGiao,
                NgayYeuCau = x.NgayYeuCau,
                NgayDo = x.NgayDo,
                NgayTraKetQua = x.NgayTraKetQua,
                TenDonViCongTac = x.IddonViCongTacNavigation?.TenDonVi,
                TenXa = x.IdxaNavigation?.TenXa,
                TenTinh = x.IdtinhNavigation?.TenTinh,
                TenNguoiXuLy = x.IdtaiKhoanDoNavigation?.HoVaTen
            };
        }

        
        public async Task<string> GetNextSoHopDongAsync(int idDonVi, int currentUserId)
        {
            int currentYear = DateTime.Now.Year;
            var soHienTai = await _context.SoHopDongDoDacs
                .Where(x => x.Nam == currentYear && x.IddonViCongTac == idDonVi)
                .OrderByDescending(x => x.SoLonNhat)
                .FirstOrDefaultAsync();
            int soTiepTheo = (soHienTai?.SoLonNhat ?? 0) + 1;
            var newLog = new SoHopDongDoDac
            {
                SoLonNhat = soTiepTheo,
                Nam = currentYear,
                IdtaiKhoan = currentUserId,
                IddonViCongTac = idDonVi,
                ThoiGian = DateTime.Now
            };
            _context.SoHopDongDoDacs.Add(newLog);
            await _context.SaveChangesAsync();
            return $"{soTiepTheo:D3}/{currentYear}/HĐKT";
        }

        public async Task<int> CreateHoSoAsync(CreateHoSoDoDacDto dto, int currentUserId, int userDonViId)
        {
            // Xác định đơn vị
            int donViId;
            if (dto.ChkOtherUnit == true && dto.IDDonViCongTac.HasValue && dto.IDDonViCongTac.Value > 0)
                donViId = dto.IDDonViCongTac.Value;
            else if (userDonViId > 0)
                donViId = userDonViId;
            else
            {
                var firstDonVi = await _context.DonViCongTacs.FirstOrDefaultAsync();
                donViId = firstDonVi?.Id ?? 1;
            }

            var donViExists = await _context.DonViCongTacs.AnyAsync(x => x.Id == donViId);
            if (!donViExists)
                throw new Exception($"Đơn vị công tác ID {donViId} không tồn tại.");

            var buocTiepNhan = await _context.QuyTrinhXuLyBuocQuyTrinhs
    .Where(b => b.IdquyTrinhXuLy == dto.IDQuyTrinh)
    .OrderByDescending(b => b.Guid == "TiepNhan")
    .FirstOrDefaultAsync();

            var hoSo = new DangKyDoDac
            {
                IddonViCongTac = donViId,
                IdtaiKhoan = currentUserId,
                IdquyTrinh = dto.IDQuyTrinh,
                IdbuocQuyTrinh = buocTiepNhan?.Id,
                TrangThaiDo = "Tiếp nhận hồ sơ",
                SoHopDong = dto.SoHopDong,
                NgayHopDong = dto.NgayHopDong ?? DateTime.Now.ToString("dd/MM/yyyy"),
                NguoiDangKy = dto.NguoiDangKy,
                Cccd = dto.CCCD,
                SoDienThoai = dto.SoDienThoai,
                MucDichDangKy = dto.MucDichDangKy,
                DiaChiThuaDat = dto.DiaChiThuaDat,
                Idxa = dto.IDXa,
                Idtinh = dto.IDTinh,
                IdtaiKhoanDo = currentUserId,
                GhiChu = dto.GhiChu
            };
            await _hoSoRepo.AddAsync(hoSo);
            await _hoSoRepo.SaveChangesAsync();

            // Ghi log
            var log = new DangKyDoDacLog
            {
                UserId = currentUserId,
                IddangKyDoDac = hoSo.Id,
                Details = $"Tạo hồ sơ mới. Số HĐ: {hoSo.SoHopDong}",
                Timestamp = DateTime.Now
            };
            _context.DangKyDoDacLogs.Add(log);
            await _context.SaveChangesAsync();

            return hoSo.Id;
        }

        public async Task<bool> UpdateHoSoAsync(UpdateHoSoDoDacDto dto, int currentUserId)
        {
            var hoSo = await _hoSoRepo.GetByIdAsync(dto.Id);
            if (hoSo == null) return false;

            bool hasChange = false;
            if (hoSo.SoPhieuGiao != dto.SoPhieuGiao) { hoSo.SoPhieuGiao = dto.SoPhieuGiao; hasChange = true; }
            if (hoSo.NguoiDangKy != dto.NguoiDangKy) { hoSo.NguoiDangKy = dto.NguoiDangKy; hasChange = true; }
            if (hoSo.Cccd != dto.CCCD) { hoSo.Cccd = dto.CCCD; hasChange = true; }
            if (hoSo.SoDienThoai != dto.SoDienThoai) { hoSo.SoDienThoai = dto.SoDienThoai; hasChange = true; }
            if (hoSo.DiaChiThuaDat != dto.DiaChiThuaDat) { hoSo.DiaChiThuaDat = dto.DiaChiThuaDat; hasChange = true; }
            if (hoSo.SeriGcn != dto.SeriGCN) { hoSo.SeriGcn = dto.SeriGCN; hasChange = true; }
            if (hoSo.MucDichDangKy != dto.MucDichDangKy) { hoSo.MucDichDangKy = dto.MucDichDangKy; hasChange = true; }
            if (hoSo.GhiChu != dto.GhiChu) { hoSo.GhiChu = dto.GhiChu; hasChange = true; }
            if (hoSo.IdtaiKhoanDo != dto.IDTaiKhoanDo) { hoSo.IdtaiKhoanDo = dto.IDTaiKhoanDo; hasChange = true; }

            var ngayDo = ParseDateOnly(dto.NgayDoStr);
            if (hoSo.NgayDo != ngayDo) { hoSo.NgayDo = ngayDo; hasChange = true; }
            var ngayGiao = ParseDateOnly(dto.NgayGiaoStr);
            if (hoSo.NgayGiao != ngayGiao) { hoSo.NgayGiao = ngayGiao; hasChange = true; }
            var ngayYeuCau = ParseDateOnly(dto.NgayYeuCauStr);
            if (hoSo.NgayYeuCau != ngayYeuCau) { hoSo.NgayYeuCau = ngayYeuCau; hasChange = true; }

            if (hasChange)
            {
                _hoSoRepo.Update(hoSo);
                await _hoSoRepo.SaveChangesAsync();

                var log = new DangKyDoDacLog
                {
                    UserId = currentUserId,
                    IddangKyDoDac = hoSo.Id,
                    Details = "Cập nhật thông tin hồ sơ",
                    Timestamp = DateTime.Now
                };
                _context.DangKyDoDacLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> ChuyenBuocAsync(ChuyenBuocDto dto, int currentUserId)
        {
            var hoSo = await _hoSoRepo.GetByIdAsync(dto.HoSoId);
            if (hoSo == null) return false;
            var buocMoi = await _context.QuyTrinhXuLyBuocQuyTrinhs.FindAsync(dto.IdBuocTiepTheo);
            if (buocMoi == null) return false;

            hoSo.IdbuocQuyTrinh = dto.IdBuocTiepTheo;
            hoSo.IdtaiKhoan = dto.IdTaiKhoanNhan;
            hoSo.TrangThaiDo = buocMoi.TenBuocQt;
            hoSo.GhiChu = dto.GhiChu;
            _hoSoRepo.Update(hoSo);
            await _hoSoRepo.SaveChangesAsync();

            var log = new DangKyDoDacLog
            {
                UserId = currentUserId,
                IddangKyDoDac = hoSo.Id,
                Details = $"Chuyển hồ sơ đến bước [{buocMoi.TenBuocQt}] - Ghi chú: {dto.GhiChu}",
                Timestamp = DateTime.Now
            };
            _context.DangKyDoDacLogs.Add(log);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> KetThucHoSoAsync(int id, int currentUserId)
        {
            var hoSo = await _hoSoRepo.GetByIdAsync(id);
            if (hoSo == null) return false;
            if (hoSo.TrangThaiDo == "KetThuc") return true;

            hoSo.TrangThaiDo = "KetThuc";
            hoSo.NgayTraKetQua = DateOnly.FromDateTime(DateTime.Now);
            // KHÔNG set IdtaiKhoan = null để giữ liên kết
            _hoSoRepo.Update(hoSo);
            await _hoSoRepo.SaveChangesAsync();

            var log = new DangKyDoDacLog
            {
                UserId = currentUserId,
                IddangKyDoDac = hoSo.Id,
                Details = "Kết thúc hồ sơ, trả kết quả",
                Timestamp = DateTime.Now
            };
            _context.DangKyDoDacLogs.Add(log);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<DangKyDoDacLog>> GetLichSuAsync(int id)
        {
            return await _context.DangKyDoDacLogs
                .Include(x => x.User)
                .Where(x => x.IddangKyDoDac == id)
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();
        }
    }
}