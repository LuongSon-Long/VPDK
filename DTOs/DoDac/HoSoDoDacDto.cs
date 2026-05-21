namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class HoSoDoDacDto
    {
        public int Id { get; set; }
        public int? IDDonViCongTac { get; set; }
        public int? IDQuyTrinh { get; set; }
        public int? IDBuocQuyTrinh { get; set; }
        public int? IDTaiKhoan { get; set; }
        public string? TrangThaiDo { get; set; }
        public string? SoHopDong { get; set; }
        public string? NgayHopDong { get; set; }
        public string? NguoiDangKy { get; set; }
        public string? CCCD { get; set; }
        public string? SoDienThoai { get; set; }
        public string? SeriGCN { get; set; }
        public string? MucDichDangKy { get; set; }
        public string? DiaChiThuaDat { get; set; }
        public int? IDXa { get; set; }
        public int? IDTinh { get; set; }
        public string? GhiChu { get; set; }
        public int? IDTaiKhoanDo { get; set; }
        public string? SoPhieuGiao { get; set; }
        public DateOnly? NgayGiao { get; set; }
        public DateOnly? NgayYeuCau { get; set; }
        public DateOnly? NgayDo { get; set; }
        public DateOnly? NgayTraKetQua { get; set; }
        public string? TenDonViCongTac { get; set; }
        public string? TenXa { get; set; }
        public string? TenTinh { get; set; }
        public string? TenNguoiXuLy { get; set; }
    }

    public class CreateHoSoDoDacDto
    {
        public int? IDDonViCongTac { get; set; }
        public string? SoHopDong { get; set; }
        public string? NgayHopDong { get; set; }
        public string? NguoiDangKy { get; set; }
        public string? CCCD { get; set; }
        public string? SoDienThoai { get; set; }
        public string? MucDichDangKy { get; set; }
        public string? DiaChiThuaDat { get; set; }
        public int? IDXa { get; set; }
        public int? IDTinh { get; set; }
        public string? GhiChu { get; set; }
        public bool? ChkOtherUnit { get; set; }
        public int? IDQuyTrinh { get; set; }
    }

    public class UpdateHoSoDoDacDto
    {
        public int Id { get; set; }
        public string? SoPhieuGiao { get; set; }
        public string? NguoiDangKy { get; set; }
        public string? CCCD { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChiThuaDat { get; set; }
        public string? SeriGCN { get; set; }
        public string? MucDichDangKy { get; set; }
        public string? NgayDoStr { get; set; }
        public string? NgayGiaoStr { get; set; }
        public string? NgayYeuCauStr { get; set; }
        public string? GhiChu { get; set; }
        public int? IDTaiKhoanDo { get; set; }
    }

    public class ChuyenBuocDto
    {
        public int HoSoId { get; set; }
        public int IdBuocTiepTheo { get; set; }
        public int IdTaiKhoanNhan { get; set; }
        public string? GhiChu { get; set; }
    }
}