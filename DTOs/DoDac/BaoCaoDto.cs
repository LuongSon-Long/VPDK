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
        public int DaXong { get; set; }
        public int DangLam { get; set; }
        public int QuaHan { get; set; }
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

    public class BaoCaoRequestDto
    {
        public int? IdTinh { get; set; }
        public int? IdDonVi { get; set; }
        public string? TuNgay { get; set; }  // dd/MM/yyyy
        public string? DenNgay { get; set; } // dd/MM/yyyy
    }
}