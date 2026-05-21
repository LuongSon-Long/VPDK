using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DonViHanhChinhTinh
{
    public int Id { get; set; }

    public string? MaTinh { get; set; }

    public string? TenTinh { get; set; }

    public virtual ICollection<DangKyDoDacBanVe> DangKyDoDacBanVes { get; set; } = new List<DangKyDoDacBanVe>();

    public virtual ICollection<DangKyDoDac> DangKyDoDacs { get; set; } = new List<DangKyDoDac>();

    public virtual ICollection<DonViCongTac> DonViCongTacs { get; set; } = new List<DonViCongTac>();

    public virtual ICollection<Dvhcxa> Dvhcxas { get; set; } = new List<Dvhcxa>();
}
