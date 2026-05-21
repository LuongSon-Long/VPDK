using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.Auth;
using HeThongQuanLyVanPhong.DTOs.TaiKhoan;

namespace HeThongQuanLyVanPhong.Services
{
    public class AuthService
    {
        private readonly TaiKhoanRepository _taiKhoanRepo;
        private readonly ModuleRepository _moduleRepo;
        private readonly SystemLogRepository _systemLogRepo;

        public AuthService(
            TaiKhoanRepository taiKhoanRepo,
            ModuleRepository moduleRepo,
            SystemLogRepository systemLogRepo)
        {
            _taiKhoanRepo = taiKhoanRepo;
            _moduleRepo = moduleRepo;
            _systemLogRepo = systemLogRepo;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.TenTaiKhoan) || string.IsNullOrEmpty(request.MatKhau))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Tên đăng nhập và mật khẩu không được để trống"
                };
            }

            // Tìm tài khoản
            var taiKhoan = await _taiKhoanRepo.GetByUsernameAsync(request.TenTaiKhoan);
            if (taiKhoan == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Tài khoản không tồn tại"
                };
            }

            // Kiểm tra mật khẩu - hỗ trợ cả plaintext (cũ) lẫn BCrypt (mới)
            bool isValidPassword = false;
            try
            {
                isValidPassword = BCrypt.Net.BCrypt.Verify(request.MatKhau, taiKhoan.MatKhau);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Mật khẩu cũ đang lưu plaintext
                isValidPassword = (taiKhoan.MatKhau == request.MatKhau);

                // Tự động nâng cấp lên BCrypt sau lần đăng nhập thành công
                if (isValidPassword)
                {
                    taiKhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(request.MatKhau);
                    await _taiKhoanRepo.UpdateAsync(taiKhoan);
                }
            }

            if (!isValidPassword)
            {
                await _systemLogRepo.AddLogAsync(taiKhoan.Id, "login_failed", "Sai mật khẩu");
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Mật khẩu không chính xác"
                };
            }

            // Lấy thông tin chi tiết
            var userWithDetails = await _taiKhoanRepo.GetWithDetailsAsync(taiKhoan.Id);
            var moduleIds = await _taiKhoanRepo.GetModuleIdsByUserIdAsync(taiKhoan.Id);
            var moduleNames = await _moduleRepo.GetModuleNamesByIdsAsync(moduleIds);

            // Ghi log thành công
            await _systemLogRepo.AddLogAsync(taiKhoan.Id, "login_success", "Đăng nhập thành công");

            return new LoginResponseDto
            {
                Success = true,
                Message = "Đăng nhập thành công",
                User = new TaiKhoanDto
                {
                    Id = taiKhoan.Id,
                    TenTaiKhoan = taiKhoan.TenTaiKhoan,
                    HoVaTen = taiKhoan.HoVaTen,
                    TenChucVu = userWithDetails?.IdchucVuNavigation?.TenChucVu,
                    TenDonViCongTac = userWithDetails?.IddonViCongTacNavigation?.TenDonVi,
                    IdDonViCongTac = userWithDetails?.IddonViCongTac,
                    Modules = moduleNames
                }
            };
        }

        public async Task LogoutAsync(int? userId)
        {
            if (userId.HasValue)
            {
                await _systemLogRepo.AddLogAsync(userId.Value, "logout", "Đăng xuất khỏi hệ thống");
            }
        }

        public async Task<TaiKhoanDto?> GetCurrentUserAsync(int userId)
        {
            var taiKhoan = await _taiKhoanRepo.GetWithDetailsAsync(userId);
            if (taiKhoan == null) return null;

            var moduleIds = await _taiKhoanRepo.GetModuleIdsByUserIdAsync(userId);
            var moduleNames = await _moduleRepo.GetModuleNamesByIdsAsync(moduleIds);

            return new TaiKhoanDto
            {
                Id = taiKhoan.Id,
                TenTaiKhoan = taiKhoan.TenTaiKhoan,
                HoVaTen = taiKhoan.HoVaTen,
                TenChucVu = taiKhoan.IdchucVuNavigation?.TenChucVu,
                TenDonViCongTac = taiKhoan.IddonViCongTacNavigation?.TenDonVi,
                Modules = moduleNames
            };
        }

        public async Task<bool> HasPermissionAsync(int userId, int moduleId)
        {
            var moduleIds = await _taiKhoanRepo.GetModuleIdsByUserIdAsync(userId);
            return moduleIds.Contains(moduleId);
        }
    }
}