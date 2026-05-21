namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class VanBanQuyDinhGiaDto
    {
        public string? VanBan { get; set; }
    }

    public class LoaiBanVeDonGiaDto
    {
        public string? LoaiBanVe { get; set; }
    }

    public class LoaiKhuVucDonGiaDto
    {
        public string? LoaiKhuVuc { get; set; }
    }

    public class ChiTietDonGiaDto
    {
        public int Id { get; set; }
        public string? DinhMucTinh { get; set; }
        public double? SoTien { get; set; }
        public double? ChiPhiLaoDong { get; set; }
        public double? TienLamTron { get; set; }
    }

    public class ThanhToanBanVeDto
    {
        public int IdBanVe { get; set; }
        public int IdDonGia { get; set; }
        public bool DaThanhToan { get; set; }
        public string? KyHieuHoaDon { get; set; }
        public string? SoHoaDon { get; set; }
        public string? NgayHoaDon { get; set; }
    }

    public class ThanhToanResponseDto
    {
        public int IdDonGia { get; set; }
        public string? KyHieu { get; set; }
        public string? SoHD { get; set; }
        public string? NgayHD { get; set; }
        public bool DaThanhToan { get; set; }
        public string? VanBan { get; set; }
        public string? LoaiBanVe { get; set; }
        public string? LoaiKhuVuc { get; set; }
    }
}