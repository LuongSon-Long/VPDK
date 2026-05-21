using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class SystemLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Action { get; set; }

    public string? Details { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual TaiKhoan? User { get; set; }
}
