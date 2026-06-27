using System.Text.Json.Serialization;

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
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("iddangKyDoDac")]
        public int? IDDangKyDoDac { get; set; }

        [JsonPropertyName("nguoiDangKy")]
        public string? NguoiDangKy { get; set; }

        [JsonPropertyName("kho")]
        public string? Kho { get; set; }

        [JsonPropertyName("gia")]
        public string? Gia { get; set; }

        [JsonPropertyName("ngan")]
        public string? Ngan { get; set; }

        [JsonPropertyName("soHsluu")]
        public string? SoHSLuu { get; set; }
    }
}