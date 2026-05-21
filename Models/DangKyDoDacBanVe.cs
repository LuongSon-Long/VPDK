using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDacBanVe
{
    public int Id { get; set; }

    public int? IddangKyDoDac { get; set; }

    public string? TrangThai { get; set; }

    public string? TenCsd { get; set; }

    public string? DiaChiThuaDat { get; set; }

    public int? Idxa { get; set; }

    public int? Idtinh { get; set; }

    public int? NamDoDac { get; set; }

    public int? SoHieuBanVe { get; set; }

    public string? LoaiBanVe { get; set; }

    public string? ToBd { get; set; }

    public string? SoHieuThua { get; set; }

    public string? DienTich { get; set; }

    public DateOnly? NgayLap { get; set; }

    public int? IdnguoiDo { get; set; }

    public string? SoVb { get; set; }

    public DateOnly? NgayVb { get; set; }

    public DateOnly? NgayTrinhKy { get; set; }

    public int? IdnguoiKy { get; set; }

    public DateOnly? NgayKy { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<DangKyDoDacBanVeThanhToan> DangKyDoDacBanVeThanhToans { get; set; } = new List<DangKyDoDacBanVeThanhToan>();

    public virtual DangKyDoDac? IddangKyDoDacNavigation { get; set; }

    public virtual TaiKhoan? IdnguoiDoNavigation { get; set; }

    public virtual TaiKhoan? IdnguoiKyNavigation { get; set; }

    public virtual DonViHanhChinhTinh? IdtinhNavigation { get; set; }

    public virtual Dvhcxa? IdxaNavigation { get; set; }
}
