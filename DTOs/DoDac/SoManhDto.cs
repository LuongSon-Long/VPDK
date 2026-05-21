namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class CreateSoManhDto
    {
        public string? MaXa { get; set; }
        public int Nam { get; set; }
    }

    public class SoManhResponseDto
    {
        public int Id { get; set; }
        public int? SHBanVe { get; set; }
        public int? Nam { get; set; }
        public string? MaXa { get; set; }
        public int? IDTaiKhoan { get; set; }
        public int? IDDonViCongTac { get; set; }
        public DateTime? NgayLay { get; set; }
    }

    public class LichSuCapSoDto
    {
        public int? SHBanVe { get; set; }
        public string? NgayLay { get; set; }
        public string? TenCanBo { get; set; }
        public string? TenDonVi { get; set; }
    }

    public class SoHieuMaxDto
    {
        public int IDXa { get; set; }
        public string? TenXa { get; set; }
        public string? MaXa { get; set; }
        public int SoHieuMax { get; set; }
    }
}