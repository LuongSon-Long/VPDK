namespace HeThongQuanLyVanPhong.DTOs.DanhMuc
{
    public class DanhMucRequestDto
    {
        public string Table { get; set; } = "";
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? ParentId { get; set; }
        public int? IdTinh { get; set; }
    }
}