using System;
using System.Collections.Generic;

namespace HeThongQuanLyVanPhong.Models;

public partial class QuyTrinhXuLyNhayBuoc
{
    public int Id { get; set; }

    public int IdbuocQuyTrinh { get; set; }

    public string? TenBuocChuyen { get; set; }

    public int IdbuocTiepTheo { get; set; }

    public virtual QuyTrinhXuLyBuocQuyTrinh IdbuocQuyTrinhNavigation { get; set; } = null!;

    public virtual QuyTrinhXuLyBuocQuyTrinh IdbuocTiepTheoNavigation { get; set; } = null!;
}
