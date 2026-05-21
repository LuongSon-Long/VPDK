using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class ChucVu
{
    public int Id { get; set; }

    public string? TenChucVu { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
