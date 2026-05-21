using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Services
{
    public class FileQuetService
    {
        private readonly FileQuetRepository _fileQuetRepo;
        private readonly SystemLogRepository _logRepo;
        private readonly HeThongQuanLyVanPhongContext _context;
        private readonly string _storageRoot;

        public FileQuetService(
            FileQuetRepository fileQuetRepo,
            SystemLogRepository logRepo,
            HeThongQuanLyVanPhongContext context,
            IConfiguration configuration)
        {
            _fileQuetRepo = fileQuetRepo;
            _logRepo = logRepo;
            _context = context;
            _storageRoot = configuration["StorageRoot"] ?? @"D:\CSDLDoDac\HSQuet";
        }

        public async Task<List<FileQuetDto>> GetFilesByHoSoIdAsync(int idHoSo)
        {
            var files = await _fileQuetRepo.GetFilesByHoSoIdAsync(idHoSo);
            return files.Select(f => new FileQuetDto
            {
                Id = f.Id,
                IDDangKyDoDac = f.IddangKyDoDac,
                TenFileLuu = f.TenFileLuu,
                NoiDungSoBo = f.NoiDungSoBo
            }).ToList();
        }

        public async Task<FileQuetDto?> GetFileByIdAsync(int id)
        {
            var file = await _fileQuetRepo.GetFileByIdAsync(id);
            if (file == null) return null;

            return new FileQuetDto
            {
                Id = file.Id,
                IDDangKyDoDac = file.IddangKyDoDac,
                TenFileLuu = file.TenFileLuu,
                NoiDungSoBo = file.NoiDungSoBo
            };
        }

        public async Task<(bool success, string message)> UploadFileAsync(int idHoSo, string noiDung, IFormFile file, int currentUserId)
        {
            var hoSo = await _context.DangKyDoDacs.FindAsync(idHoSo);
            if (hoSo == null)
                return (false, "Hồ sơ không tồn tại.");

            if (hoSo.TrangThaiDo == "KetThuc" || hoSo.TrangThaiDo == "Kết thúc")
                return (false, "Hồ sơ đã kết thúc, không thể thêm file.");

            /*if (hoSo.IdtaiKhoan != currentUserId)
                return (false, "Bạn không có quyền thêm file vào hồ sơ của người khác.");*/

            if (file == null || file.Length == 0 || file.Length > 30 * 1024 * 1024)
                return (false, "File không hợp lệ hoặc > 30MB.");

            try
            {
                string folderPath = Path.Combine(_storageRoot, idHoSo.ToString());
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = Path.GetFileName(file.FileName);
                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newFile = new FileHoSoDoDac
                {
                    IddangKyDoDac = idHoSo,
                    TenFileLuu = fileName,
                    NoiDungSoBo = noiDung
                };

                await _fileQuetRepo.AddFileAsync(newFile);
                await _fileQuetRepo.SaveChangesAsync();

                await _logRepo.AddLogAsync(currentUserId, "Upload file", $"Tải lên: {fileName} cho hồ sơ {idHoSo}");
                return (true, "Upload thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteFileAsync(int id, int currentUserId)
        {
            var fileRecord = await _fileQuetRepo.GetFileByIdAsync(id);
            if (fileRecord == null || fileRecord.IddangKyDoDacNavigation == null)
                return (false, "Không tìm thấy dữ liệu.");

            var hoSo = fileRecord.IddangKyDoDacNavigation;
            /*if (hoSo.IdtaiKhoan != currentUserId)
                return (false, "Bạn không có quyền xóa file của người khác.");*/

            if (hoSo.TrangThaiDo == "KetThuc" || hoSo.TrangThaiDo == "Kết thúc")
                return (false, "Hồ sơ đã kết thúc, không thể xóa file.");

            try
            {
                string folderPath = Path.Combine(_storageRoot, hoSo.Id.ToString());
                string fullPath = Path.Combine(folderPath, fileRecord.TenFileLuu ?? "");

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                _fileQuetRepo.DeleteFile(fileRecord);
                await _fileQuetRepo.SaveChangesAsync();

                await _logRepo.AddLogAsync(currentUserId, "Xóa file", $"Xóa file: {fileRecord.TenFileLuu} của hồ sơ {hoSo.Id}");
                return (true, "Xóa thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        public async Task<(byte[]? fileBytes, string? contentType, string? fileName)> ViewFileAsync(int id)
        {
            var fileRecord = await _fileQuetRepo.GetFileByIdAsync(id);
            if (fileRecord == null || fileRecord.IddangKyDoDacNavigation == null)
                return (null, null, null);

            string folderPath = Path.Combine(_storageRoot, fileRecord.IddangKyDoDacNavigation.Id.ToString());
            string filePath = Path.Combine(folderPath, fileRecord.TenFileLuu ?? "");

            if (!System.IO.File.Exists(filePath))
                return (null, null, null);

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            string ext = Path.GetExtension(fileRecord.TenFileLuu).ToLower();
            string contentType = ext == ".pdf" ? "application/pdf" : "application/octet-stream";

            return (fileBytes, contentType, fileRecord.TenFileLuu);
        }
    }
}