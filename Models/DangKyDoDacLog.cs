using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDacLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? IddangKyDoDac { get; set; }

    public string? Details { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual DangKyDoDac? IddangKyDoDacNavigation { get; set; }

    public virtual TaiKhoan? User { get; set; }
}
