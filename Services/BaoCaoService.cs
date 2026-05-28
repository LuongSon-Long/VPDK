using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.DTOs.DoDac;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Services
{
    public class BaoCaoService
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public BaoCaoService(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        public async Task<object> GetThongKeAsync(BaoCaoRequestDto request)
        {
            var query = _context.DangKyDoDacs.AsQueryable();
            if (request.IdDonVi.HasValue)
                query = query.Where(x => x.IddonViCongTac == request.IdDonVi);
            if (request.IdTinh.HasValue)
                query = query.Where(x => x.Idtinh == request.IdTinh);

            var dataRaw = await query
                .Include(x => x.IdtaiKhoanDoNavigation)
                .Include(x => x.IdquyTrinhNavigation)
                .ToListAsync();

            DateTime dTu = DateTime.ParseExact(request.TuNgay!, "dd/MM/yyyy", null).Date;
            DateTime dDen = DateTime.ParseExact(request.DenNgay!, "dd/MM/yyyy", null).Date;
            DateTime bayGio = DateTime.Now.Date;

            var filteredData = dataRaw.Where(x =>
            {
                if (DateTime.TryParseExact(x.NgayHopDong, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime d))
                    return d.Date >= dTu && d.Date <= dDen;
                return false;
            }).ToList();

            // Thống kê trạng thái
            var thongKeTrangThai = filteredData
                .GroupBy(x => x.TrangThaiDo ?? "Chưa xác định")
                .Select((g, index) =>
                {
                    var items = g.ToList();
                    int quaHan = items.Count(x => (x.NgayTraKetQua ?? DateOnly.FromDateTime(bayGio)).ToDateTime(TimeOnly.MinValue).Date > (x.NgayYeuCau ?? DateOnly.FromDateTime(bayGio)).ToDateTime(TimeOnly.MinValue).Date);
                    return new ThongKeTrangThaiDto
                    {
                        TenTrangThai = g.Key,
                        SoLuong = items.Count,
                        ConHan = items.Count - quaHan,
                        QuaHan = quaHan,
                        MauSac = GetColor(index)
                    };
                }).ToList();

            DateOnly homNayDateOnly = DateOnly.FromDateTime(bayGio);
            // Thống kê cán bộ
            var thongKeCanBo = filteredData
                .GroupBy(x => x.IdtaiKhoanDo ?? 0)
                .Select(g => {
                    var firstWithNav = g.FirstOrDefault(x => x.IdtaiKhoanDoNavigation != null);
                    var items = g.ToList();

                    int dangXlDungHan = 0, dangXlQuaHan = 0, daXlDungHan = 0, daXlQuaHan = 0;

                    foreach (var item in items)
                    {
                        var han = item.NgayYeuCau ?? homNayDateOnly; // Nếu không có hạn, tạm coi là hạn hôm nay

                        if (item.NgayDo.HasValue)
                        {
                            // Đã xử lý (Đã có ngày đo)
                            if (item.NgayDo.Value <= han) daXlDungHan++;
                            else daXlQuaHan++;
                        }
                        else
                        {
                            // Đang xử lý (Chưa có ngày đo)
                            if (homNayDateOnly <= han) dangXlDungHan++;
                            else dangXlQuaHan++;
                        }
                    }

                    return new ThongKeCanBoDto
                    {
                        IDTaiKhoanDo = g.Key,
                        TenCanBo = firstWithNav?.IdtaiKhoanDoNavigation?.HoVaTen ?? "Chưa phân công",
                        Tong = items.Count,
                        DangXLDungHan = dangXlDungHan,
                        DangXLQuaHan = dangXlQuaHan,
                        DaXLDungHan = daXlDungHan,
                        DaXLQuaHan = daXlQuaHan,
                        DaTraKetQua = items.Count(x => x.NgayTraKetQua.HasValue)
                    };
                }).ToList();

            // Thống kê bản vẽ
            var listIdHoSo = filteredData.Select(x => x.Id).ToList();
            var dataBanVe = await _context.DangKyDoDacBanVes
                .Where(bv => bv.IddangKyDoDac != null && listIdHoSo.Contains(bv.IddangKyDoDac.Value))
                .ToListAsync();

            var thongKeBanVe = dataBanVe
                .GroupBy(x => x.LoaiBanVe ?? "Chưa xác định")
                .Select(g => new ThongKeBanVeDto
                {
                    TenLoai = g.Key,
                    TongSo = g.Count(),
                    ChiTietTrangThai = g.GroupBy(x => x.TrangThai ?? "Chưa xác định")
                                        .Select(st => new ChiTietTrangThaiBanVeDto
                                        {
                                            TenTrangThai = st.Key,
                                            SoLuong = st.Count()
                                        }).ToList()
                }).ToList();

            var thongKeQuyTrinh = filteredData
                // (Bỏ lệnh .Where(TrangThai != KetThuc) ở đây để tính tổng chính xác)
                .GroupBy(x => x.IdquyTrinhNavigation?.TenQuyTrinh ?? "Chưa phân loại quy trình")
                .Select(g =>
                {
                    var items = g.ToList();
                    int dangXlDungHan = 0, dangXlQuaHan = 0, daXlDungHan = 0, daXlQuaHan = 0;

                    foreach (var item in items)
                    {
                        var han = item.NgayYeuCau ?? homNayDateOnly;

                        if (item.NgayDo.HasValue)
                        {
                            if (item.NgayDo.Value <= han) daXlDungHan++;
                            else daXlQuaHan++;
                        }
                        else
                        {
                            if (homNayDateOnly <= han) dangXlDungHan++;
                            else dangXlQuaHan++;
                        }
                    }

                    return new ThongKeQuyTrinhDto
                    {
                        TenQuyTrinh = g.Key,
                        Tong = items.Count,
                        DangXLDungHan = dangXlDungHan,
                        DangXLQuaHan = dangXlQuaHan,
                        DaXLDungHan = daXlDungHan,
                        DaXLQuaHan = daXlQuaHan
                    };
                }).ToList();

            return new
            {
                thongKeTrangThai,
                thongKeCanBo,
                thongKeBanVe,
                thongKeQuyTrinh
            };
        }

        private string GetColor(int index)
        {
            var colors = new[] { "#4e73df", "#1cc88a", "#36b9cc", "#f6c23e", "#e74a3b", "#858796", "#5a5c69", "#6f42c1" };
            return colors[index % colors.Length];
        }

        public async Task<List<object>> GetDanhSachChiTietAsync(BaoCaoRequestDto request, string? trangThai, int? idNhanVien)
        {
            var query = _context.DangKyDoDacs
                .Include(x => x.IdtaiKhoanDoNavigation)     
                .Include(x => x.IdtaiKhoanNavigation)       
                .Include(x => x.IddonViCongTacNavigation)
                .Include(x => x.IdxaNavigation)
                .Include(x => x.DangKyDoDacBanVes)
                .AsQueryable();

            // 2. Lọc theo các điều kiện cơ bản
            if (idNhanVien.HasValue && idNhanVien > 0)
                query = query.Where(x => x.IdtaiKhoanDo == idNhanVien);
            else
            {
                if (request.IdDonVi.HasValue)
                    query = query.Where(x => x.IddonViCongTac == request.IdDonVi);
                if (!string.IsNullOrEmpty(trangThai))
                    query = query.Where(x => x.TrangThaiDo == trangThai);
            }

            // 3. Lọc theo Tên chủ sử dụng đất (Bản vẽ)
            if (!string.IsNullOrWhiteSpace(request.TenChuSD))
            {
                string tuKhoaChuSD = request.TenChuSD.Trim();
                // Dùng Any với điều kiện kiểm tra null an toàn
                query = query.Where(x => x.DangKyDoDacBanVes.Any(bv =>
                    bv.TenCsd != null && EF.Functions.Like(bv.TenCsd, "%" + tuKhoaChuSD + "%")));
            }

            // 4. Lấy dữ liệu về bộ nhớ
            var dataRaw = await query.ToListAsync();
            Console.WriteLine("Tổng hồ sơ tìm được: " + dataRaw.Count);
            foreach (var item in dataRaw)
            {
                Console.WriteLine($"HoSo ID: {item.Id}, SoHopDong: {item.SoHopDong}, So luong BanVe: {item.DangKyDoDacBanVes?.Count ?? 0}");
            }
            // 5. Xử lý lọc ngày tháng phía Client một cách an toàn
            DateTime dTu = DateTime.ParseExact(request.TuNgay!, "dd/MM/yyyy", null).Date;
            DateTime dDen = DateTime.ParseExact(request.DenNgay!, "dd/MM/yyyy", null).Date;

            var list = dataRaw.Where(x =>
            {
                // 1. LUÔN LUÔN ƯU TIÊN KẾT QUẢ TÌM KIẾM ĐẶC BIỆT
                if (!string.IsNullOrWhiteSpace(request.TenChuSD)) return true;

                // 2. NẾU KHÔNG CÓ TÌM KIẾM ĐẶC BIỆT, MỚI LỌC THEO NGÀY
                if (string.IsNullOrWhiteSpace(x.NgayHopDong)) return false;

                // Sử dụng DateTime.TryParse để tránh crash khi định dạng ngày khác nhau
                if (DateTime.TryParseExact(x.NgayHopDong, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime d))
                {
                    return d.Date >= dTu.Date && d.Date <= dDen.Date;
                }

                return false;
            }).OrderByDescending(x => x.Id).ToList();

            return list.Select(x => (object)new HoSoDoDacDto
            {
                Id = x.Id,
                IDDonViCongTac = x.IddonViCongTac,
                IDTaiKhoan = x.IdtaiKhoan,
                IDTaiKhoanDo = x.IdtaiKhoanDo,
                TrangThaiDo = x.TrangThaiDo,
                SoHopDong = x.SoHopDong,
                NgayHopDong = x.NgayHopDong,
                NguoiDangKy = x.NguoiDangKy,
                CCCD = x.Cccd,
                SoDienThoai = x.SoDienThoai,
                MucDichDangKy = x.MucDichDangKy,
                DiaChiThuaDat = x.DiaChiThuaDat,
                IDXa = x.Idxa,
                IDTinh = x.Idtinh,
                GhiChu = x.GhiChu,
                NgayGiao = x.NgayGiao,
                NgayYeuCau = x.NgayYeuCau,
                NgayDo = x.NgayDo,
                NgayTraKetQua = x.NgayTraKetQua,
                TenNguoiXuLy = x.IdtaiKhoanNavigation?.HoVaTen ?? "Chưa phân công",
                TenNguoiDo = x.IdtaiKhoanDoNavigation?.HoVaTen ?? "Chưa phân công",
                TenDonViCongTac = x.IddonViCongTacNavigation?.TenDonVi,
                TenXa = x.IdxaNavigation?.TenXa,
            }).ToList();
        }
    }
}