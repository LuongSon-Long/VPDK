namespace HeThongQuanLyVanPhong.DTOs.DoDac
{
    public class FileQuetDto
    {
        public int Id { get; set; }
        public int? IDDangKyDoDac { get; set; }
        public string? TenFileLuu { get; set; }
        public string? NoiDungSoBo { get; set; }
    }

    public class UploadFileQuetDto
    {
        public int IdHoSo { get; set; }
        public string? NoiDung { get; set; }
        public IFormFile? File { get; set; }
    }
}