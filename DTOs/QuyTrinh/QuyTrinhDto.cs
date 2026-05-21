namespace HeThongQuanLyVanPhong.DTOs.QuyTrinh
{
    public class QuyTrinhDto
    {
        public int Id { get; set; }
        public string? TenQuyTrinh { get; set; }
        public string? LoaiQuyTrinh { get; set; }
    }

    public class BuocQuyTrinhDto
    {
        public int Id { get; set; }
        public int IdQuyTrinhXuLy { get; set; }
        public string? TenBuocQt { get; set; }
        public string? Guid { get; set; }
    }

    public class NhayBuocDto
    {
        public int Id { get; set; }
        public int IdBuocQuyTrinh { get; set; }
        public string? TenBuocChuyen { get; set; }
        public int IdBuocTiepTheo { get; set; }
    }

    public class SaveQuyTrinhRequestDto
    {
        public int Id { get; set; }
        public string TenQuyTrinh { get; set; } = "";
        public string? LoaiQuyTrinh { get; set; }
    }
}