using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class FileHoSoDoDac
{
    public int Id { get; set; }

    public int? IddangKyDoDac { get; set; }

    public string? TenFileLuu { get; set; }

    public string? NoiDungSoBo { get; set; }

    public virtual DangKyDoDac? IddangKyDoDacNavigation { get; set; }
}
