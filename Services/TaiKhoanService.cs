using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.TaiKhoan;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Services
{
    public class TaiKhoanService
    {
        private readonly TaiKhoanRepository _taiKhoanRepo;
        private readonly SystemLogRepository _logRepo;
        private readonly HeThongQuanLyVanPhongContext _context;

        public TaiKhoanService(TaiKhoanRepository taiKhoanRepo, SystemLogRepository logRepo, HeThongQuanLyVanPhongContext context)
        {
            _taiKhoanRepo = taiKhoanRepo;
            _logRepo = logRepo;
            _context = context;
        }

        public async Task<List<TaiKhoanListDto>> GetAllAsync(string? searchTerm = null)
        {
            var query = _taiKhoanRepo.GetQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.TenTaiKhoan.Contains(searchTerm) ||
                                         (t.HoVaTen != null && t.HoVaTen.Contains(searchTerm)));
            }

            var list = await query
                .Select(t => new TaiKhoanListDto
                {
                    Id = t.Id,
                    TenTaiKhoan = t.TenTaiKhoan,
                    HoVaTen = t.HoVaTen,
                    TenChucVu = t.IdchucVuNavigation != null ? t.IdchucVuNavigation.TenChucVu : "",
                    TenDonViCongTac = t.IddonViCongTacNavigation != null ? t.IddonViCongTacNavigation.TenDonVi : "",
                    IdTinh = t.Idtinh
                })
                .OrderByDescending(t => t.Id)
                .ToListAsync();

            return list;
        }

        public async Task<TaiKhoanDetailDto?> GetDetailByIdAsync(int id)
        {
            var taiKhoan = await _taiKhoanRepo.GetWithDetailsAsync(id);
            if (taiKhoan == null) return null;

            var moduleIds = await _taiKhoanRepo.GetModuleIdsByUserIdAsync(id);

            return new TaiKhoanDetailDto
            {
                Id = taiKhoan.Id,
                TenTaiKhoan = taiKhoan.TenTaiKhoan,
                HoVaTen = taiKhoan.HoVaTen,
                IdchucVu = taiKhoan.IdchucVu,
                TenChucVu = taiKhoan.IdchucVuNavigation?.TenChucVu,
                IddonViCongTac = taiKhoan.IddonViCongTac,
                TenDonViCongTac = taiKhoan.IddonViCongTacNavigation?.TenDonVi,
                IdTinh = taiKhoan.Idtinh,
                UserModules = moduleIds
            };
        }

        public async Task<List<int>> GetUserModulesAsync(int userId)
        {
            return await _taiKhoanRepo.GetModuleIdsByUserIdAsync(userId);
        }

        public async Task<bool> CreateAsync(TaiKhoan model, List<int> selectedModules, int currentUserId)
        {
            model.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau ?? "123456");
            model.Id = 0;

            var created = await _taiKhoanRepo.AddAsync(model);
            await _taiKhoanRepo.UpdateUserModulesAsync(created.Id, selectedModules);
            await _logRepo.AddLogAsync(currentUserId, "Thêm mới", $"Tạo tài khoản: {model.TenTaiKhoan}");

            return true;
        }

        public async Task<bool> UpdateAsync(TaiKhoan model, List<int> selectedModules, int currentUserId)
        {
            var existing = await _taiKhoanRepo.GetByIdAsync(model.Id);
            if (existing == null) return false;

            existing.HoVaTen = model.HoVaTen;
            existing.IdchucVu = model.IdchucVu;
            existing.IddonViCongTac = model.IddonViCongTac;
            existing.Idtinh = model.Idtinh;

            await _taiKhoanRepo.UpdateAsync(existing);
            await _taiKhoanRepo.UpdateUserModulesAsync(model.Id, selectedModules);
            await _logRepo.AddLogAsync(currentUserId, "Cập nhật", $"Sửa tài khoản: {existing.TenTaiKhoan}");

            return true;
        }

        public async Task<bool> ResetPasswordAsync(int id, int currentUserId)
        {
            var user = await _taiKhoanRepo.GetByIdAsync(id);
            if (user == null) return false;
            if (user.TenTaiKhoan == "admin") return false;

            user.MatKhau = BCrypt.Net.BCrypt.HashPassword("VPDK@123");
            await _taiKhoanRepo.UpdateAsync(user);
            await _logRepo.AddLogAsync(currentUserId, "Reset Pass", $"Tài khoản: {user.TenTaiKhoan}");

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int currentUserId)
        {
            var user = await _taiKhoanRepo.GetByIdAsync(id);
            if (user == null) return false;
            if (user.TenTaiKhoan == "admin") return false;

            await _taiKhoanRepo.DeleteAsync(user);
            await _logRepo.AddLogAsync(currentUserId, "Xóa", $"Tài khoản: {user.TenTaiKhoan}");

            return true;
        }

        public async Task<List<object>> GetMyModulesAsync(int userId)
        {
            var modules = await _context.PhanQuyenModules
                .Where(pq => pq.IdtaiKhoan == userId)
                .Select(pq => pq.IdmoduleNavigation)
                .Where(m => m != null)
                .Select(m => new { m.Id, m.TenModule })
                .ToListAsync();
            return modules.Cast<object>().ToList();
        }

        public async Task<List<ModuleDashboardDto>> GetUserDashboardModulesAsync(int userId)
        {
            // 1. Kiểm tra xem tài khoản đăng nhập hiện tại có tên đăng nhập là "admin" hay không
            var taiKhoan = await _context.TaiKhoans.FindAsync(userId);
            bool isAdminAccount = taiKhoan?.TenTaiKhoan?.Trim().ToLower() == "admin";

            // 2. Truy vấn lấy các module của tài khoản từ bảng phân quyền như bình thường
            var modules = await _context.PhanQuyenModules
                .Where(pq => pq.IdtaiKhoan == userId)
                .Select(pq => pq.IdmoduleNavigation)
                .Where(m => m != null)
                .ToListAsync();

            // 3. ĐIỀU KIỆN ĐẶC CÁCH: Nếu là tài khoản admin gốc và trong danh sách chưa có module Quản trị hệ thống (Id = 5)
            if (isAdminAccount && !modules.Any(m => m.Id == 5))
            {
                // Tự động lấy Module Quản trị hệ thống từ bảng gốc để nạp vào danh sách hiển thị
                var systemAdminModule = await _context.Modules.FindAsync(5);
                if (systemAdminModule != null)
                {
                    modules.Add(systemAdminModule);
                }
            }

            // 4. Ánh xạ dữ liệu sang DTO để trả ra cho Controller hiển thị lên View Index
            return modules.Select(m => new ModuleDashboardDto
            {
                Id = m!.Id,
                TenModule = m.TenModule ?? "Không tên",
                Url = GetUrlForModule(m.Id),
                IconClass = GetIconForModule(m.Id)
            }).ToList();
        }
        // --- Hàm phụ trợ gán Url theo cấu trúc thư mục của bạn ---
        private string GetUrlForModule(int id)
        {
            return id switch
            {
                1 => "/DoDac/Index",
                2 => "/ThuTucHanhChinh/Index",
                3 => "/VanThu/Index",
                4 => "/QuanLyPhoiGCN/Index",
                5 => "/Admin/Index",
                _ => "/Home/Index"
            };
        }

        // --- Hàm phụ trợ gán Icon (dùng Bootstrap Icons) ---
        private string GetIconForModule(int id)
        {
            return id switch
            {
                1 => "bi-compass",
                2 => "bi-file-earmark-text",
                3 => "bi-envelope-paper",
                4 => "bi-award",
                5 => "bi-gear",
                _ => "bi-box"
            };
        }

        public async Task<(bool success, string message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _taiKhoanRepo.GetByIdAsync(userId);
            if (user == null)
                return (false, "Không tìm thấy tài khoản");

            // Kiểm tra mật khẩu cũ - xử lý cả 2 trường hợp
            bool isValid = false;
            try
            {
                // Thử BCrypt trước (tài khoản đã được tạo/reset sau khi có BCrypt)
                isValid = BCrypt.Net.BCrypt.Verify(oldPassword, user.MatKhau);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Mật khẩu đang lưu plaintext → so sánh thẳng
                isValid = (oldPassword == user.MatKhau);
            }

            if (!isValid)
                return (false, "Mật khẩu hiện tại không đúng");

            // Lưu mật khẩu mới luôn dùng BCrypt
            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _taiKhoanRepo.UpdateAsync(user);
            await _logRepo.AddLogAsync(userId, "Đổi mật khẩu", $"Tài khoản: {user.TenTaiKhoan}");

            return (true, "Đổi mật khẩu thành công");
        }
    }
}