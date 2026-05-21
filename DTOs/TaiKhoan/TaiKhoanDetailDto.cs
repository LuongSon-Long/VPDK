namespace HeThongQuanLyVanPhong.DTOs.TaiKhoan
{
    public class TaiKhoanDetailDto
    {
        public int Id { get; set; }
        public string? TenTaiKhoan { get; set; }
        public string? HoVaTen { get; set; }
        public int? IdchucVu { get; set; }
        public string? TenChucVu { get; set; }
        public int? IddonViCongTac { get; set; }
        public string? TenDonViCongTac { get; set; }
        public List<int>? UserModules { get; set; }
    }
}