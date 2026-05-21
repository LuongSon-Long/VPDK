using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDacSoManhTd
{
    public int Id { get; set; }

    public int? ShbanVe { get; set; }

    public int? Nam { get; set; }

    public string? MaXa { get; set; }

    public int? IdtaiKhoan { get; set; }

    public int? IddonViCongTac { get; set; }

    public DateTime? NgayLay { get; set; }

    public virtual DonViCongTac? IddonViCongTacNavigation { get; set; }

    public virtual TaiKhoan? IdtaiKhoanNavigation { get; set; }
}
