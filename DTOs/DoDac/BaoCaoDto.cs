namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class ThongKeTrangThaiDto
    {
        public string? TenTrangThai { get; set; }
        public int SoLuong { get; set; }
        public int ConHan { get; set; }
        public int QuaHan { get; set; }
        public string? MauSac { get; set; }
    }

    public class ThongKeCanBoDto
    {
        public int IDTaiKhoanDo { get; set; }
        public string? TenCanBo { get; set; }
        public int Tong { get; set; }
        public int DangXLDungHan { get; set; }
        public int DangXLQuaHan { get; set; }
        public int DaXLDungHan { get; set; }
        public int DaXLQuaHan { get; set; }
        public int DaTraKetQua { get; set; }
    }

    public class ThongKeBanVeDto
    {
        public string? TenLoai { get; set; }
        public int TongSo { get; set; }
        public List<ChiTietTrangThaiBanVeDto>? ChiTietTrangThai { get; set; }
    }

    public class ChiTietTrangThaiBanVeDto
    {
        public string? TenTrangThai { get; set; }
        public int SoLuong { get; set; }
    }

    public class ThongKeQuyTrinhDto
    {
        public string? TenQuyTrinh { get; set; }
        public int Tong { get; set; }
        public int DangXLDungHan { get; set; }
        public int DangXLQuaHan { get; set; }
        public int DaXLDungHan { get; set; }
        public int DaXLQuaHan { get; set; }
    }
    

    public class BaoCaoRequestDto
    {
        public int? IdTinh { get; set; }
        public int? IdDonVi { get; set; }
        public string? TuNgay { get; set; }  // dd/MM/yyyy
        public string? DenNgay { get; set; } // dd/MM/yyyy
        public string? TenChuSD { get; set; }
        public string? TuKhoa { get; set; }
    }
}