using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class Module
{
    public int Id { get; set; }

    public string? TenModule { get; set; }

    public virtual ICollection<PhanQuyenModule> PhanQuyenModules { get; set; } = new List<PhanQuyenModule>();
}
