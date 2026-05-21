using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class SoHopDongDoDac
{
    public int Id { get; set; }

    public int? SoLonNhat { get; set; }

    public int? Nam { get; set; }

    public int? IdtaiKhoan { get; set; }

    public DateTime? ThoiGian { get; set; }

    public int? IddonViCongTac { get; set; }

    public virtual DonViCongTac? IddonViCongTacNavigation { get; set; }

    public virtual TaiKhoan? IdtaiKhoanNavigation { get; set; }
}
