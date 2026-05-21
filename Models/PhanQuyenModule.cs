using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class PhanQuyenModule
{
    public int Id { get; set; }

    public int? IdtaiKhoan { get; set; }

    public int? Idmodule { get; set; }

    public virtual Module? IdmoduleNavigation { get; set; }

    public virtual TaiKhoan? IdtaiKhoanNavigation { get; set; }
}
