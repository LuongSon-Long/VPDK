using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.QuyTrinh;

namespace HeThongQuanLyVanPhong.Services
{
    public class QuyTrinhService
    {
        private readonly QuyTrinhRepository _quyTrinhRepo;
        private readonly SystemLogRepository _logRepo;

        public QuyTrinhService(QuyTrinhRepository quyTrinhRepo, SystemLogRepository logRepo)
        {
            _quyTrinhRepo = quyTrinhRepo;
            _logRepo = logRepo;
        }

        // Lấy danh sách quy trình
        public async Task<List<QuyTrinhDto>> GetAllQuyTrinhAsync()
        {
            var list = await _quyTrinhRepo.GetAllQuyTrinhAsync();
            return list.Select(q => new QuyTrinhDto
            {
                Id = q.Id,
                TenQuyTrinh = q.TenQuyTrinh,
                LoaiQuyTrinh = q.LoaiQuyTrinh
            }).ToList();
        }

        // Lấy chi tiết quy trình (kèm bước + nhảy bước)
        public async Task<object?> GetWorkflowDetailsAsync(int idQuyTrinh)
        {
            var buocs = await _quyTrinhRepo.GetBuocByQuyTrinhIdAsync(idQuyTrinh);
            if (buocs == null || !buocs.Any()) return null;

            var buocIds = buocs.Select(b => b.Id).ToList();
            var nhays = await _quyTrinhRepo.GetNhayBuocByBuocIdsAsync(buocIds);

            var steps = buocs.Select(b => new BuocQuyTrinhDto
            {
                Id = b.Id,
                IdQuyTrinhXuLy = b.IdquyTrinhXuLy,
                TenBuocQt = b.TenBuocQt,
                Guid = b.Guid
            }).ToList();

            var transitions = nhays.Select(n => new NhayBuocDto
            {
                Id = n.Id,
                IdBuocQuyTrinh = n.IdbuocQuyTrinh,
                TenBuocChuyen = n.TenBuocChuyen,
                IdBuocTiepTheo = n.IdbuocTiepTheo
            }).ToList();

            return new { steps, transitions };
        }

        // Lưu quy trình (thêm hoặc sửa)
        public async Task<(bool success, string message)> SaveQuyTrinhAsync(SaveQuyTrinhRequestDto request, int currentUserId)
        {
            if (string.IsNullOrEmpty(request.TenQuyTrinh))
                return (false, "Tên quy trình không được để trống");

            if (request.Id == 0)
            {
                await _quyTrinhRepo.AddQuyTrinhAsync(new QuyTrinhXuLy
                {
                    TenQuyTrinh = request.TenQuyTrinh,
                    LoaiQuyTrinh = request.LoaiQuyTrinh
                });
                await _logRepo.AddLogAsync(currentUserId, "Thêm mới", $"Tạo quy trình: {request.TenQuyTrinh}");
            }
            else
            {
                var qt = await _quyTrinhRepo.GetQuyTrinhByIdAsync(request.Id);
                if (qt == null) return (false, "Không tìm thấy quy trình để cập nhật");

                qt.TenQuyTrinh = request.TenQuyTrinh;
                qt.LoaiQuyTrinh = request.LoaiQuyTrinh;
                _quyTrinhRepo.UpdateQuyTrinh(qt);
                await _logRepo.AddLogAsync(currentUserId, "Cập nhật", $"Sửa quy trình ID: {request.Id}");
            }

            await _quyTrinhRepo.SaveChangesAsync();
            return (true, "");
        }

        // Lưu bước quy trình
        public async Task<bool> SaveBuocAsync(BuocQuyTrinhDto request, int currentUserId)
        {
            var buoc = new QuyTrinhXuLyBuocQuyTrinh
            {
                IdquyTrinhXuLy = request.IdQuyTrinhXuLy,
                TenBuocQt = request.TenBuocQt,
                Guid = request.Guid
            };
            await _quyTrinhRepo.AddBuocAsync(buoc);
            await _quyTrinhRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Thêm bước", $"Thêm bước: {request.TenBuocQt}");
            return true;
        }

        // Lưu nhảy bước
        public async Task<bool> SaveNhayBuocAsync(NhayBuocDto request, int currentUserId)
        {
            var nhay = new QuyTrinhXuLyNhayBuoc
            {
                IdbuocQuyTrinh = request.IdBuocQuyTrinh,
                TenBuocChuyen = request.TenBuocChuyen,
                IdbuocTiepTheo = request.IdBuocTiepTheo
            };
            await _quyTrinhRepo.AddNhayBuocAsync(nhay);
            await _quyTrinhRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Thêm nhảy bước", $"Thêm nhảy bước từ ID {request.IdBuocQuyTrinh} sang {request.IdBuocTiepTheo}");
            return true;
        }

        // Xóa bước quy trình
        public async Task<bool> DeleteBuocAsync(int id, int currentUserId)
        {
            // Xóa các nhảy bước liên quan
            var relatedTransitions = await _quyTrinhRepo.GetNhayBuocByBuocIdsAsync(new List<int> { id });
            _quyTrinhRepo.DeleteNhayBuocRange(relatedTransitions);

            // Xóa bước
            var step = await _quyTrinhRepo.GetBuocByIdAsync(id);
            if (step != null) _quyTrinhRepo.DeleteBuoc(step);

            await _quyTrinhRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Xóa bước", $"Xóa bước ID: {id}");
            return true;
        }

        // Xóa nhảy bước
        public async Task<bool> DeleteNhayBuocAsync(int id, int currentUserId)
        {
            var item = await _quyTrinhRepo.GetNhayBuocByIdAsync(id);
            if (item != null) _quyTrinhRepo.DeleteNhayBuoc(item);

            await _quyTrinhRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Xóa nhảy bước", $"Xóa nhảy bước ID: {id}");
            return true;
        }

        // Xóa cả quy trình
        public async Task<bool> DeleteQuyTrinhAsync(int id, int currentUserId)
        {
            var buocIds = await _quyTrinhRepo.GetBuocIdsByQuyTrinhIdAsync(id);

            // Xóa nhảy bước
            var transitions = await _quyTrinhRepo.GetNhayBuocByBuocIdsAsync(buocIds);
            _quyTrinhRepo.DeleteNhayBuocRange(transitions);

            // Xóa bước
            var steps = await _quyTrinhRepo.GetBuocByQuyTrinhIdAsync(id);
            foreach (var step in steps) _quyTrinhRepo.DeleteBuoc(step);

            // Xóa quy trình
            var qt = await _quyTrinhRepo.GetQuyTrinhByIdAsync(id);
            if (qt != null) _quyTrinhRepo.DeleteQuyTrinh(qt);

            await _quyTrinhRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Xóa quy trình", $"Xóa quy trình ID: {id}");
            return true;
        }
    }
}