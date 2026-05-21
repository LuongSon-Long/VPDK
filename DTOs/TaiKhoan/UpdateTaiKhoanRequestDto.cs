namespace HeThongQuanLyVanPhong.DTOs.TaiKhoan
{
    public class UpdateTaiKhoanRequestDto
    {
        public string? HoVaTen { get; set; }
        public int? IdchucVu { get; set; }
        public int? IddonViCongTac { get; set; }
        public List<int>? SelectedModules { get; set; }
    }
}