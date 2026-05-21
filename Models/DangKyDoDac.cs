using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class DangKyDoDac
{
    public int Id { get; set; }

    public int? IddonViCongTac { get; set; }

    public int? IdquyTrinh { get; set; }

    public int? IdbuocQuyTrinh { get; set; }

    public int? IdtaiKhoan { get; set; }

    public string? TrangThaiDo { get; set; }

    public string? SoHopDong { get; set; }

    public string? NgayHopDong { get; set; }

    public string? NguoiDangKy { get; set; }

    public string? Cccd { get; set; }

    public string? SoDienThoai { get; set; }

    public string? SeriGcn { get; set; }

    public string? MucDichDangKy { get; set; }

    public string? DiaChiThuaDat { get; set; }

    public int? Idxa { get; set; }

    public int? Idtinh { get; set; }

    public string? GhiChu { get; set; }

    public int? IdtaiKhoanDo { get; set; }

    public string? SoPhieuGiao { get; set; }

    public DateOnly? NgayGiao { get; set; }

    public DateOnly? NgayYeuCau { get; set; }

    public DateOnly? NgayDo { get; set; }

    public DateOnly? NgayTraKetQua { get; set; }

    public virtual ICollection<DangKyDoDacBanVeThanhToan> DangKyDoDacBanVeThanhToans { get; set; } = new List<DangKyDoDacBanVeThanhToan>();

    public virtual ICollection<DangKyDoDacBanVe> DangKyDoDacBanVes { get; set; } = new List<DangKyDoDacBanVe>();

    public virtual ICollection<DangKyDoDacLog> DangKyDoDacLogs { get; set; } = new List<DangKyDoDacLog>();

    public virtual ICollection<DangKyDoDacLuuTru> DangKyDoDacLuuTrus { get; set; } = new List<DangKyDoDacLuuTru>();

    public virtual ICollection<FileHoSoDoDac> FileHoSoDoDacs { get; set; } = new List<FileHoSoDoDac>();

    public virtual QuyTrinhXuLyBuocQuyTrinh? IdbuocQuyTrinhNavigation { get; set; }

    public virtual DonViCongTac? IddonViCongTacNavigation { get; set; }

    public virtual QuyTrinhXuLy? IdquyTrinhNavigation { get; set; }

    public virtual TaiKhoan? IdtaiKhoanDoNavigation { get; set; }

    public virtual TaiKhoan? IdtaiKhoanNavigation { get; set; }

    public virtual DonViHanhChinhTinh? IdtinhNavigation { get; set; }

    public virtual Dvhcxa? IdxaNavigation { get; set; }
}
