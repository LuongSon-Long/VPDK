using HeThongQuanLyVanPhong.DTOs.TaiKhoan;
namespace HeThongQuanLyVanPhong.DTOs.Auth
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public TaiKhoanDto? User { get; set; }
    }
}