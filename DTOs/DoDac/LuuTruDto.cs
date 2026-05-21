namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class LuuTruDto
    {
        public int IDDangKyDoDac { get; set; }
        public string? Kho { get; set; }
        public string? Gia { get; set; }
        public string? Ngan { get; set; }
        public string? SoHSLuu { get; set; }
    }

    public class LuuTruResponseDto
    {
        public int Id { get; set; }
        public int? IDDangKyDoDac { get; set; }
        public string? Kho { get; set; }
        public string? Gia { get; set; }
        public string? Ngan { get; set; }
        public string? SoHSLuu { get; set; }
    }
}