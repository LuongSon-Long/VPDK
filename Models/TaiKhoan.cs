using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class TaiKhoan
{
    public int Id { get; set; }

    public string TenTaiKhoan { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? HoVaTen { get; set; }

    public int? IdchucVu { get; set; }

    public int? IddonViCongTac { get; set; }

    public virtual ICollection<DangKyDoDacBanVe> DangKyDoDacBanVeIdnguoiDoNavigations { get; set; } = new List<DangKyDoDacBanVe>();

    public virtual ICollection<DangKyDoDacBanVe> DangKyDoDacBanVeIdnguoiKyNavigations { get; set; } = new List<DangKyDoDacBanVe>();

    public virtual ICollection<DangKyDoDacBanVeThanhToan> DangKyDoDacBanVeThanhToans { get; set; } = new List<DangKyDoDacBanVeThanhToan>();

    public virtual ICollection<DangKyDoDac> DangKyDoDacIdtaiKhoanDoNavigations { get; set; } = new List<DangKyDoDac>();

    public virtual ICollection<DangKyDoDac> DangKyDoDacIdtaiKhoanNavigations { get; set; } = new List<DangKyDoDac>();

    public virtual ICollection<DangKyDoDacLog> DangKyDoDacLogs { get; set; } = new List<DangKyDoDacLog>();

    public virtual ICollection<DangKyDoDacSoManhTd> DangKyDoDacSoManhTds { get; set; } = new List<DangKyDoDacSoManhTd>();

    public virtual ChucVu? IdchucVuNavigation { get; set; }

    public virtual DonViCongTac? IddonViCongTacNavigation { get; set; }

    public virtual ICollection<PhanQuyenModule> PhanQuyenModules { get; set; } = new List<PhanQuyenModule>();

    public virtual ICollection<SoHopDongDoDac> SoHopDongDoDacs { get; set; } = new List<SoHopDongDoDac>();

    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();
}
