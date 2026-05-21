using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class QuyTrinhXuLy
{
    public int Id { get; set; }

    public string? TenQuyTrinh { get; set; }

    public string? LoaiQuyTrinh { get; set; }

    public virtual ICollection<DangKyDoDac> DangKyDoDacs { get; set; } = new List<DangKyDoDac>();

    public virtual ICollection<QuyTrinhXuLyBuocQuyTrinh> QuyTrinhXuLyBuocQuyTrinhs { get; set; } = new List<QuyTrinhXuLyBuocQuyTrinh>();
}
