using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDacLuuTru
{
    public int Id { get; set; }

    public int? IddangKyDoDac { get; set; }

    public string? Kho { get; set; }

    public string? Gia { get; set; }

    public string? Ngan { get; set; }

    public string? SoHsluu { get; set; }

    public virtual DangKyDoDac? IddangKyDoDacNavigation { get; set; }
}
