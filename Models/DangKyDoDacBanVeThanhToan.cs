using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDacBanVeThanhToan
{
    public int Id { get; set; }

    public int? IdbanVe { get; set; }

    public int? IddangKyDoDac { get; set; }

    public int? IddonViCongTac { get; set; }

    public int? IdtaiKhoan { get; set; }

    public int? IddonGia { get; set; }

    public string? KyHieuHoaDon { get; set; }

    public string? SoHoaDon { get; set; }

    public DateOnly? NgayHoaDon { get; set; }

    public bool DaThanhToan { get; set; }

    public virtual DangKyDoDacBanVe? IdbanVeNavigation { get; set; }

    public virtual DangKyDoDac? IddangKyDoDacNavigation { get; set; }

    public virtual DangKyDoDacDonGium? IddonGiaNavigation { get; set; }

    public virtual DonViCongTac? IddonViCongTacNavigation { get; set; }

    public virtual TaiKhoan? IdtaiKhoanNavigation { get; set; }
}
