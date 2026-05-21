using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Services
{
    public class BanVeService
    {
        private readonly BanVeRepository _banVeRepo;
        private readonly SystemLogRepository _logRepo;

        public BanVeService(BanVeRepository banVeRepo, SystemLogRepository logRepo)
        {
            _banVeRepo = banVeRepo;
            _logRepo = logRepo;
        }

        private DateOnly? ParseToDateOnly(string? dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return null;
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result))
                return DateOnly.FromDateTime(result);
            return null;
        }

        private string? FormatDateOnly(DateOnly? date)
        {
            return date?.ToString("dd/MM/yyyy");
        }

        public async Task<List<BanVeDto>> GetByHoSoIdAsync(int idDangKyDoDac)
        {
            var list = await _banVeRepo.GetByHoSoIdAsync(idDangKyDoDac);
            return list.Select(x => new BanVeDto
            {
                Id = x.Id,
                IDDangKyDoDac = x.IddangKyDoDac,
                TrangThai = x.TrangThai,
                TenCSD = x.TenCsd,
                DiaChiThuaDat = x.DiaChiThuaDat,
                IDXa = x.Idxa,
                IDTinh = x.Idtinh,
                NamDoDac = x.NamDoDac,
                SoHieuBanVe = x.SoHieuBanVe,
                LoaiBanVe = x.LoaiBanVe,
                ToBD = x.ToBd,
                SoHieuThua = x.SoHieuThua,
                DienTich = x.DienTich,
                NgayLap = x.NgayLap.HasValue ? new DateTime(x.NgayLap.Value.Year, x.NgayLap.Value.Month, x.NgayLap.Value.Day) : (DateTime?)null,
                IDNguoiDo = x.IdnguoiDo,
                SoVB = x.SoVb,
                NgayVB = x.NgayVb.HasValue ? new DateTime(x.NgayVb.Value.Year, x.NgayVb.Value.Month, x.NgayVb.Value.Day) : (DateTime?)null,
                NgayTrinhKy = x.NgayTrinhKy.HasValue ? new DateTime(x.NgayTrinhKy.Value.Year, x.NgayTrinhKy.Value.Month, x.NgayTrinhKy.Value.Day) : (DateTime?)null,
                IDNguoiKy = x.IdnguoiKy,
                NgayKy = x.NgayKy.HasValue ? new DateTime(x.NgayKy.Value.Year, x.NgayKy.Value.Month, x.NgayKy.Value.Day) : (DateTime?)null,
                GhiChu = x.GhiChu
            }).ToList();
        }

        public async Task<BanVeDto?> GetByIdAsync(int id)
        {
            var x = await _banVeRepo.GetByIdAsync(id);
            if (x == null) return null;

            return new BanVeDto
            {
                Id = x.Id,
                IDDangKyDoDac = x.IddangKyDoDac,
                TrangThai = x.TrangThai,
                TenCSD = x.TenCsd,
                DiaChiThuaDat = x.DiaChiThuaDat,
                IDXa = x.Idxa,
                IDTinh = x.Idtinh,
                NamDoDac = x.NamDoDac,
                SoHieuBanVe = x.SoHieuBanVe,
                LoaiBanVe = x.LoaiBanVe,
                ToBD = x.ToBd,
                SoHieuThua = x.SoHieuThua,
                DienTich = x.DienTich,
                NgayLap = x.NgayLap.HasValue ? new DateTime(x.NgayLap.Value.Year, x.NgayLap.Value.Month, x.NgayLap.Value.Day) : (DateTime?)null,
                IDNguoiDo = x.IdnguoiDo,
                SoVB = x.SoVb,
                NgayVB = x.NgayVb.HasValue ? new DateTime(x.NgayVb.Value.Year, x.NgayVb.Value.Month, x.NgayVb.Value.Day) : (DateTime?)null,
                NgayTrinhKy = x.NgayTrinhKy.HasValue ? new DateTime(x.NgayTrinhKy.Value.Year, x.NgayTrinhKy.Value.Month, x.NgayTrinhKy.Value.Day) : (DateTime?)null,
                IDNguoiKy = x.IdnguoiKy,
                NgayKy = x.NgayKy.HasValue ? new DateTime(x.NgayKy.Value.Year, x.NgayKy.Value.Month, x.NgayKy.Value.Day) : (DateTime?)null,
                GhiChu = x.GhiChu
            };
        }

        private string XacDinhTrangThai(SaveBanVeDto dto)
        {
            DateOnly? ngayLap = ParseToDateOnly(dto.NgayLapStr);
            DateOnly? ngayVB = ParseToDateOnly(dto.NgayVBStr);
            DateOnly? ngayTrinhKy = ParseToDateOnly(dto.NgayTrinhKyStr);
            DateOnly? ngayKy = ParseToDateOnly(dto.NgayKyStr);

            if (ngayKy != null) return "Đã duyệt bản vẽ";
            if (ngayTrinhKy != null) return "Đang trình ký";
            if (ngayVB != null) return "Đang xin ý kiến cơ quan liên quan";
            if (ngayLap != null) return "Đã lập bản vẽ";
            return "Chưa lập bản vẽ";
        }

        public async Task<(bool success, string message, int? id)> SaveBanVeAsync(SaveBanVeDto dto, int currentUserId)
        {
            string trangThai = XacDinhTrangThai(dto);

            if (dto.Id == 0)
            {
                var banVe = new DangKyDoDacBanVe
                {
                    IddangKyDoDac = dto.IDDangKyDoDac,
                    TenCsd = dto.TenCSD,
                    DiaChiThuaDat = dto.DiaChiThuaDat,
                    Idxa = dto.IDXa,
                    Idtinh = dto.IDTinh,
                    NamDoDac = dto.NamDoDac,
                    SoHieuBanVe = dto.SoHieuBanVe,
                    LoaiBanVe = dto.LoaiBanVe,
                    ToBd = dto.ToBD,
                    SoHieuThua = dto.SoHieuThua,
                    DienTich = dto.DienTich,
                    NgayLap = ParseToDateOnly(dto.NgayLapStr),
                    IdnguoiDo = dto.IDNguoiDo,
                    SoVb = dto.SoVB,
                    NgayVb = ParseToDateOnly(dto.NgayVBStr),
                    NgayTrinhKy = ParseToDateOnly(dto.NgayTrinhKyStr),
                    IdnguoiKy = dto.IDNguoiKy,
                    NgayKy = ParseToDateOnly(dto.NgayKyStr),
                    GhiChu = dto.GhiChu,
                    TrangThai = trangThai
                };

                await _banVeRepo.AddAsync(banVe);
                await _banVeRepo.SaveChangesAsync();
                await _logRepo.AddLogAsync(currentUserId, "Thêm bản vẽ", $"Thêm bản vẽ cho hồ sơ {dto.IDDangKyDoDac}");
                return (true, "Thêm thành công", banVe.Id);
            }
            else
            {
                var existItem = await _banVeRepo.GetByIdAsync(dto.Id);
                if (existItem == null)
                    return (false, "Không tìm thấy bản vẽ", null);

                existItem.TenCsd = dto.TenCSD;
                existItem.DiaChiThuaDat = dto.DiaChiThuaDat;
                existItem.Idxa = dto.IDXa;
                existItem.Idtinh = dto.IDTinh;
                existItem.NamDoDac = dto.NamDoDac;
                existItem.SoHieuBanVe = dto.SoHieuBanVe;
                existItem.LoaiBanVe = dto.LoaiBanVe;
                existItem.ToBd = dto.ToBD;
                existItem.SoHieuThua = dto.SoHieuThua;
                existItem.DienTich = dto.DienTich;
                existItem.NgayLap = ParseToDateOnly(dto.NgayLapStr);
                existItem.IdnguoiDo = dto.IDNguoiDo;
                existItem.SoVb = dto.SoVB;
                existItem.NgayVb = ParseToDateOnly(dto.NgayVBStr);
                existItem.NgayTrinhKy = ParseToDateOnly(dto.NgayTrinhKyStr);
                existItem.IdnguoiKy = dto.IDNguoiKy;
                existItem.NgayKy = ParseToDateOnly(dto.NgayKyStr);
                existItem.GhiChu = dto.GhiChu;
                existItem.TrangThai = trangThai;

                _banVeRepo.Update(existItem);
                await _banVeRepo.SaveChangesAsync();
                await _logRepo.AddLogAsync(currentUserId, "Sửa bản vẽ", $"Sửa bản vẽ ID {dto.Id}");
                return (true, "Cập nhật thành công", dto.Id);
            }
        }

        public async Task<(bool success, string message)> DeleteBanVeAsync(int id, int currentUserId)
        {
            var item = await _banVeRepo.GetByIdAsync(id);
            if (item == null)
                return (false, "Không tìm thấy bản vẽ");

            int idHoSo = item.IddangKyDoDac ?? 0;
            _banVeRepo.Delete(item);
            await _banVeRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Xóa bản vẽ", $"Xóa bản vẽ ID {id} của hồ sơ {idHoSo}");
            return (true, "Xóa thành công");
        }
    }
}