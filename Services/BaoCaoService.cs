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

            // Thống kê cán bộ
            var thongKeCanBo = filteredData
                .Where(x => x.IdtaiKhoanDo != null)
                .GroupBy(x => new { x.IdtaiKhoanDo, x.IdtaiKhoanDoNavigation!.HoVaTen })
                .Select(g => new ThongKeCanBoDto
                {
                    IDTaiKhoanDo = g.Key.IdtaiKhoanDo ?? 0,
                    TenCanBo = g.Key.HoVaTen ?? "--Chưa xác định--",
                    Tong = g.Count(),
                    DaXong = g.Count(x => x.TrangThaiDo == "KetThuc"),
                    DangLam = g.Count(x => x.TrangThaiDo != "KetThuc"),
                    QuaHan = g.Count(x => x.TrangThaiDo != "KetThuc" && x.NgayTraKetQua.HasValue && x.NgayTraKetQua.Value.ToDateTime(TimeOnly.MinValue).Date < bayGio)
                }).OrderByDescending(x => x.Tong).ToList();

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

            return new
            {
                thongKeTrangThai,
                thongKeCanBo,
                thongKeBanVe
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
                .Include(x => x.IdtaiKhoanDoNavigation)   // người xử lý
                .Include(x => x.IddonViCongTacNavigation)  // đơn vị công tác
                .Include(x => x.IdxaNavigation)            // xã/phường
                .AsQueryable();

            if (idNhanVien.HasValue && idNhanVien > 0)
                query = query.Where(x => x.IdtaiKhoanDo == idNhanVien);
            else
            {
                if (request.IdDonVi.HasValue)
                    query = query.Where(x => x.IddonViCongTac == request.IdDonVi);
                if (!string.IsNullOrEmpty(trangThai))
                    query = query.Where(x => x.TrangThaiDo == trangThai);
            }

            var dataRaw = await query.ToListAsync();
            DateTime dTu = DateTime.ParseExact(request.TuNgay!, "dd/MM/yyyy", null).Date;
            DateTime dDen = DateTime.ParseExact(request.DenNgay!, "dd/MM/yyyy", null).Date;

            var list = dataRaw.Where(x =>
            {
                if (DateTime.TryParseExact(x.NgayHopDong, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime d))
                    return d.Date >= dTu && d.Date <= dDen;
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
                // ✅ Các field join
                TenNguoiXuLy = x.IdtaiKhoanDoNavigation?.HoVaTen,
                TenDonViCongTac = x.IddonViCongTacNavigation?.TenDonVi,
                TenXa = x.IdxaNavigation?.TenXa,
            }).ToList();
        }
    }
}