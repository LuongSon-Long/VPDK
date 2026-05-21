namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class BanVeDto
    {
        public int Id { get; set; }
        public int? IDDangKyDoDac { get; set; }
        public string? TrangThai { get; set; }
        public string? TenCSD { get; set; }
        public string? DiaChiThuaDat { get; set; }
        public int? IDXa { get; set; }
        public int? IDTinh { get; set; }
        public int? NamDoDac { get; set; }
        public int? SoHieuBanVe { get; set; }
        public string? LoaiBanVe { get; set; }
        public string? ToBD { get; set; }
        public string? SoHieuThua { get; set; }
        public string? DienTich { get; set; }
        public DateTime? NgayLap { get; set; }
        public int? IDNguoiDo { get; set; }
        public string? SoVB { get; set; }
        public DateTime? NgayVB { get; set; }
        public DateTime? NgayTrinhKy { get; set; }
        public int? IDNguoiKy { get; set; }
        public DateTime? NgayKy { get; set; }
        public string? GhiChu { get; set; }
    }

    public class SaveBanVeDto
    {
        public int Id { get; set; }
        public int? IDDangKyDoDac { get; set; }
        public string? TenCSD { get; set; }
        public string? DiaChiThuaDat { get; set; }
        public int? IDXa { get; set; }
        public int? IDTinh { get; set; }
        public int? NamDoDac { get; set; }
        public int? SoHieuBanVe { get; set; }
        public string? LoaiBanVe { get; set; }
        public string? ToBD { get; set; }
        public string? SoHieuThua { get; set; }
        public string? DienTich { get; set; }
        public string? NgayLapStr { get; set; }
        public int? IDNguoiDo { get; set; }
        public string? SoVB { get; set; }
        public string? NgayVBStr { get; set; }
        public string? NgayTrinhKyStr { get; set; }
        public int? IDNguoiKy { get; set; }
        public string? NgayKyStr { get; set; }
        public string? GhiChu { get; set; }
        public string? TrangThai { get; set; }
    }
}