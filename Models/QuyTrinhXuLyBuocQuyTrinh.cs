using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class QuyTrinhXuLyBuocQuyTrinh
{
    public int Id { get; set; }

    public int IdquyTrinhXuLy { get; set; }

    public string? TenBuocQt { get; set; }

    public string? Guid { get; set; }

    public virtual ICollection<DangKyDoDac> DangKyDoDacs { get; set; } = new List<DangKyDoDac>();

    public virtual QuyTrinhXuLy IdquyTrinhXuLyNavigation { get; set; } = null!;

    public virtual ICollection<QuyTrinhXuLyNhayBuoc> QuyTrinhXuLyNhayBuocIdbuocQuyTrinhNavigations { get; set; } = new List<QuyTrinhXuLyNhayBuoc>();

    public virtual ICollection<QuyTrinhXuLyNhayBuoc> QuyTrinhXuLyNhayBuocIdbuocTiepTheoNavigations { get; set; } = new List<QuyTrinhXuLyNhayBuoc>();
}
