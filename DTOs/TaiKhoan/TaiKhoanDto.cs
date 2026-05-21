namespace HeThongQuanLyVanPhong.DTOs.TaiKhoan
{
    public class TaiKhoanDto
    {
        public int Id { get; set; }
        public string? TenTaiKhoan { get; set; }
        public string? HoVaTen { get; set; }
        public string? TenChucVu { get; set; }
        public string? TenDonViCongTac { get; set; }
        public int? IdDonViCongTac { get; set; }
        public List<string>? Modules { get; set; }
    }
}