using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDacDonGium
{
    public int Id { get; set; }

    public string? VanBanQuyDinhGia { get; set; }

    public string? LoaiBanVe { get; set; }

    public string? LoaiKhuVuc { get; set; }

    public string? DinhMucTinh { get; set; }

    public double? SoTien { get; set; }

    public double? ChiPhiLaoDong { get; set; }

    public double? TienLamTron { get; set; }

    public virtual ICollection<DangKyDoDacBanVeThanhToan> DangKyDoDacBanVeThanhToans { get; set; } = new List<DangKyDoDacBanVeThanhToan>();
}
