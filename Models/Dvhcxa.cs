using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class Dvhcxa
{
    public int Id { get; set; }

    public int Idtinh { get; set; }

    public string? MaXa { get; set; }

    public string? TenXa { get; set; }

    public virtual ICollection<DangKyDoDacBanVe> DangKyDoDacBanVes { get; set; } = new List<DangKyDoDacBanVe>();

    public virtual ICollection<DangKyDoDac> DangKyDoDacs { get; set; } = new List<DangKyDoDac>();

    public virtual DonViHanhChinhTinh IdtinhNavigation { get; set; } = null!;
}
