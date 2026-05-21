namespace HeThongQuanLyVanPhong.DTOs.TaiKhoan
{
    public class CreateTaiKhoanRequestDto
    {
        public string TenTaiKhoan { get; set; } = "";
        public string MatKhau { get; set; } = "";
        public string? HoVaTen { get; set; }
        public int? IdchucVu { get; set; }
        public int? IddonViCongTac { get; set; }
        public List<int>? SelectedModules { get; set; }
    }
}