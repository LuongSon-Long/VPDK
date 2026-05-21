namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class XuatExcelRequestDto
    {
        public string? TrangThai { get; set; }
        public int? IdDonVi { get; set; }
        public string? TuNgay { get; set; }
        public string? DenNgay { get; set; }
        public int? IdNhanVien { get; set; }
    }

    public class XuatExcelThanhToanRequestDto
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string? DaThanhToan { get; set; }
        public int? IdTinh { get; set; }
        public int? IdDonVi { get; set; }
    }

    public class XuatWordThanhLyRequestDto
    {
        public int IdDangKyDoDac { get; set; }
        public string? NgayLapStr { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienUng { get; set; }
        public string? FileNameOut { get; set; }
    }
}