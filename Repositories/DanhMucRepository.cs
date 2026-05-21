using HeThongQuanLyVanPhong.Models;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Repositories
{
    public class DanhMucRepository
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public DanhMucRepository(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        // ChucVu
        public async Task<List<ChucVu>> GetChucVusAsync() => await _context.ChucVus.OrderBy(x => x.Id).ToListAsync();
        public async Task<ChucVu?> GetChucVuByIdAsync(int id) => await _context.ChucVus.FindAsync(id);
        public async Task AddChucVuAsync(ChucVu entity) => await _context.ChucVus.AddAsync(entity);
        public void UpdateChucVu(ChucVu entity) => _context.ChucVus.Update(entity);
        public void DeleteChucVu(ChucVu entity) => _context.ChucVus.Remove(entity);

        // Tinh
        public async Task<List<DonViHanhChinhTinh>> GetTinhsAsync() => await _context.DonViHanhChinhTinhs.OrderBy(x => x.TenTinh).ToListAsync();
        public async Task<DonViHanhChinhTinh?> GetTinhByIdAsync(int id) => await _context.DonViHanhChinhTinhs.FindAsync(id);
        public async Task AddTinhAsync(DonViHanhChinhTinh entity) => await _context.DonViHanhChinhTinhs.AddAsync(entity);
        public void UpdateTinh(DonViHanhChinhTinh entity) => _context.DonViHanhChinhTinhs.Update(entity);
        public void DeleteTinh(DonViHanhChinhTinh entity) => _context.DonViHanhChinhTinhs.Remove(entity);

        // Xa
        public async Task<List<Dvhcxa>> GetXasAsync() => await _context.Dvhcxas.Include(x => x.IdtinhNavigation).OrderBy(x => x.TenXa).ToListAsync();
        public async Task<Dvhcxa?> GetXaByIdAsync(int id) => await _context.Dvhcxas.FindAsync(id);
        public async Task AddXaAsync(Dvhcxa entity) => await _context.Dvhcxas.AddAsync(entity);
        public void UpdateXa(Dvhcxa entity) => _context.Dvhcxas.Update(entity);
        public void DeleteXa(Dvhcxa entity) => _context.Dvhcxas.Remove(entity);

        // DonViCongTac
        public async Task<List<DonViCongTac>> GetDonViCongTacsAsync() => await _context.DonViCongTacs.Include(x => x.IddonViHanhChinhTinhNavigation).OrderBy(x => x.TenDonVi).ToListAsync();
        public async Task<DonViCongTac?> GetDonViCongTacByIdAsync(int id) => await _context.DonViCongTacs.FindAsync(id);
        public async Task AddDonViCongTacAsync(DonViCongTac entity) => await _context.DonViCongTacs.AddAsync(entity);
        public void UpdateDonViCongTac(DonViCongTac entity) => _context.DonViCongTacs.Update(entity);
        public void DeleteDonViCongTac(DonViCongTac entity) => _context.DonViCongTacs.Remove(entity);

        // LoaiBanVe
        public async Task<List<LoaiBanVe>> GetLoaiBanVesAsync() => await _context.LoaiBanVes.OrderBy(x => x.Id).ToListAsync();
        public async Task<LoaiBanVe?> GetLoaiBanVeByIdAsync(int id) => await _context.LoaiBanVes.FindAsync(id);
        public async Task AddLoaiBanVeAsync(LoaiBanVe entity) => await _context.LoaiBanVes.AddAsync(entity);
        public void UpdateLoaiBanVe(LoaiBanVe entity) => _context.LoaiBanVes.Update(entity);
        public void DeleteLoaiBanVe(LoaiBanVe entity) => _context.LoaiBanVes.Remove(entity);

        // TrangThaiBanVe
        public async Task<List<TrangThaiBanVe>> GetTrangThaisAsync() => await _context.TrangThaiBanVes.OrderBy(x => x.Id).ToListAsync();
        public async Task<TrangThaiBanVe?> GetTrangThaiByIdAsync(int id) => await _context.TrangThaiBanVes.FindAsync(id);
        public async Task AddTrangThaiAsync(TrangThaiBanVe entity) => await _context.TrangThaiBanVes.AddAsync(entity);
        public void UpdateTrangThai(TrangThaiBanVe entity) => _context.TrangThaiBanVes.Update(entity);
        public void DeleteTrangThai(TrangThaiBanVe entity) => _context.TrangThaiBanVes.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}