using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DanhMuc;

namespace HeThongQuanLyVanPhong.Services
{
    public class DanhMucService
    {
        private readonly DanhMucRepository _danhMucRepo;
        private readonly SystemLogRepository _logRepo;

        public DanhMucService(DanhMucRepository danhMucRepo, SystemLogRepository logRepo)
        {
            _danhMucRepo = danhMucRepo;
            _logRepo = logRepo;
        }

        public async Task<object?> GetListAsync(string table)
        {
            switch (table)
            {
                case "ChucVu":
                    return await _danhMucRepo.GetChucVusAsync();
                case "Tinh":
                    return await _danhMucRepo.GetTinhsAsync();
                case "Xa":
                    var xas = await _danhMucRepo.GetXasAsync();
                    return xas.Select(x => new { x.Id, x.MaXa, x.TenXa, TenTinh = x.IdtinhNavigation?.TenTinh }).ToList();
                case "DonViCongTac":
                    var dvs = await _danhMucRepo.GetDonViCongTacsAsync();
                    return dvs.Select(x => new { x.Id, x.TenDonVi, TenTinh = x.IddonViHanhChinhTinhNavigation?.TenTinh }).ToList();
                case "LoaiBanVe":
                    return await _danhMucRepo.GetLoaiBanVesAsync();
                case "TrangThai":
                    return await _danhMucRepo.GetTrangThaisAsync();
                default:
                    return null;
            }
        }

        public async Task<bool> SaveAsync(DanhMucRequestDto request, int currentUserId)
        {
            switch (request.Table)
            {
                case "Tinh":
                    if (request.Id > 0)
                    {
                        var tinh = await _danhMucRepo.GetTinhByIdAsync(request.Id);
                        if (tinh == null) return false;
                        tinh.TenTinh = request.Name;
                        tinh.MaTinh = request.Code;
                        _danhMucRepo.UpdateTinh(tinh);
                    }
                    else
                    {
                        await _danhMucRepo.AddTinhAsync(new DonViHanhChinhTinh { TenTinh = request.Name, MaTinh = request.Code });
                    }
                    break;

                case "Xa":
                    if (request.Id > 0)
                    {
                        var xa = await _danhMucRepo.GetXaByIdAsync(request.Id);
                        if (xa == null) return false;
                        xa.TenXa = request.Name;
                        xa.MaXa = request.Code;
                        if (request.ParentId.HasValue) xa.Idtinh = request.ParentId.Value;
                        _danhMucRepo.UpdateXa(xa);
                    }
                    else
                    {
                        await _danhMucRepo.AddXaAsync(new Dvhcxa { TenXa = request.Name, MaXa = request.Code, Idtinh = request.ParentId ?? 0 });
                    }
                    break;

                case "ChucVu":
                    if (request.Id > 0)
                    {
                        var cv = await _danhMucRepo.GetChucVuByIdAsync(request.Id);
                        if (cv == null) return false;
                        cv.TenChucVu = request.Name;
                        _danhMucRepo.UpdateChucVu(cv);
                    }
                    else
                    {
                        await _danhMucRepo.AddChucVuAsync(new ChucVu { TenChucVu = request.Name });
                    }
                    break;

                case "DonViCongTac":
                    if (request.Id > 0)
                    {
                        var dv = await _danhMucRepo.GetDonViCongTacByIdAsync(request.Id);
                        if (dv == null) return false;
                        dv.TenDonVi = request.Name;
                        dv.IddonViHanhChinhTinh = request.IdTinh;
                        _danhMucRepo.UpdateDonViCongTac(dv);
                    }
                    else
                    {
                        await _danhMucRepo.AddDonViCongTacAsync(new DonViCongTac { TenDonVi = request.Name, IddonViHanhChinhTinh = request.IdTinh });
                    }
                    break;

                case "LoaiBanVe":
                    if (request.Id > 0)
                    {
                        var lbv = await _danhMucRepo.GetLoaiBanVeByIdAsync(request.Id);
                        if (lbv == null) return false;
                        lbv.TenLoaiBanVe = request.Name;
                        _danhMucRepo.UpdateLoaiBanVe(lbv);
                    }
                    else
                    {
                        await _danhMucRepo.AddLoaiBanVeAsync(new LoaiBanVe { TenLoaiBanVe = request.Name });
                    }
                    break;

                case "TrangThai":
                    if (request.Id > 0)
                    {
                        var tt = await _danhMucRepo.GetTrangThaiByIdAsync(request.Id);
                        if (tt == null) return false;
                        tt.TrangThai = request.Name;
                        _danhMucRepo.UpdateTrangThai(tt);
                    }
                    else
                    {
                        await _danhMucRepo.AddTrangThaiAsync(new TrangThaiBanVe { TrangThai = request.Name });
                    }
                    break;

                default:
                    return false;
            }

            await _danhMucRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Lưu danh mục", $"{request.Table}: {request.Name}");
            return true;
        }

        public async Task<bool> DeleteAsync(string table, int id, int currentUserId)
        {
            try
            {
                string? name = null;

                switch (table)
                {
                    case "ChucVu":
                        var cv = await _danhMucRepo.GetChucVuByIdAsync(id);
                        if (cv == null) return false;
                        name = cv.TenChucVu;
                        _danhMucRepo.DeleteChucVu(cv);
                        break;
                    case "Tinh":
                        var tinh = await _danhMucRepo.GetTinhByIdAsync(id);
                        if (tinh == null) return false;
                        name = tinh.TenTinh;
                        _danhMucRepo.DeleteTinh(tinh);
                        break;
                    case "Xa":
                        var xa = await _danhMucRepo.GetXaByIdAsync(id);
                        if (xa == null) return false;
                        name = xa.TenXa;
                        _danhMucRepo.DeleteXa(xa);
                        break;
                    case "DonViCongTac":
                        var dv = await _danhMucRepo.GetDonViCongTacByIdAsync(id);
                        if (dv == null) return false;
                        name = dv.TenDonVi;
                        _danhMucRepo.DeleteDonViCongTac(dv);
                        break;
                    case "LoaiBanVe":
                        var lbv = await _danhMucRepo.GetLoaiBanVeByIdAsync(id);
                        if (lbv == null) return false;
                        name = lbv.TenLoaiBanVe;
                        _danhMucRepo.DeleteLoaiBanVe(lbv);
                        break;
                    case "TrangThai":
                        var tt = await _danhMucRepo.GetTrangThaiByIdAsync(id);
                        if (tt == null) return false;
                        name = tt.TrangThai;
                        _danhMucRepo.DeleteTrangThai(tt);
                        break;
                    default:
                        return false;
                }

                await _danhMucRepo.SaveChangesAsync();
                await _logRepo.AddLogAsync(currentUserId, "Xóa danh mục", $"{table}: {name}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}