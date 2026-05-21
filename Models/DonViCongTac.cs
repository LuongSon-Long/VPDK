using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DonViCongTac
{
    public int Id { get; set; }

    public string? TenDonVi { get; set; }

    public int? IddonViHanhChinhTinh { get; set; }

    public virtual ICollection<DangKyDoDacBanVeThanhToan> DangKyDoDacBanVeThanhToans { get; set; } = new List<DangKyDoDacBanVeThanhToan>();

    public virtual ICollection<DangKyDoDacSoManhTd> DangKyDoDacSoManhTds { get; set; } = new List<DangKyDoDacSoManhTd>();

    public virtual ICollection<DangKyDoDac> DangKyDoDacs { get; set; } = new List<DangKyDoDac>();

    public virtual DonViHanhChinhTinh? IddonViHanhChinhTinhNavigation { get; set; }

    public virtual ICollection<SoHopDongDoDac> SoHopDongDoDacs { get; set; } = new List<SoHopDongDoDac>();

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
