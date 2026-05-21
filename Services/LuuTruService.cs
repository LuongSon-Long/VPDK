using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.DTOs.DoDac;

namespace HeThongQuanLyVanPhong.Services
{
    public class LuuTruService
    {
        private readonly LuuTruRepository _luuTruRepo;
        private readonly SystemLogRepository _logRepo;

        public LuuTruService(LuuTruRepository luuTruRepo, SystemLogRepository logRepo)
        {
            _luuTruRepo = luuTruRepo;
            _logRepo = logRepo;
        }

        public async Task<List<string>> GetKhoListAsync()
        {
            return await _luuTruRepo.GetDistinctKhoAsync();
        }

        public async Task<List<string>> GetGiaByKhoAsync(string kho)
        {
            return await _luuTruRepo.GetGiaByKhoAsync(kho);
        }

        public async Task<int> GetNextSoHSLuuAsync(string kho, string gia, string ngan)
        {
            var lastRecord = await _luuTruRepo.GetLastRecordByLocationAsync(kho, gia, ngan);
            int nextNumber = 1;
            if (lastRecord != null && int.TryParse(lastRecord.SoHsluu, out int lastNum))
            {
                nextNumber = lastNum + 1;
            }
            return nextNumber;
        }

        public async Task<bool> SaveLuuTruAsync(LuuTruDto request, int currentUserId)
        {
            var luuTru = await _luuTruRepo.GetByHoSoIdAsync(request.IDDangKyDoDac);

            if (luuTru == null)
            {
                luuTru = new DangKyDoDacLuuTru
                {
                    IddangKyDoDac = request.IDDangKyDoDac,
                    Kho = request.Kho,
                    Gia = request.Gia,
                    Ngan = request.Ngan,
                    SoHsluu = request.SoHSLuu
                };
                await _luuTruRepo.AddAsync(luuTru);
            }
            else
            {
                luuTru.Kho = request.Kho;
                luuTru.Gia = request.Gia;
                luuTru.Ngan = request.Ngan;
                luuTru.SoHsluu = request.SoHSLuu;
                _luuTruRepo.Update(luuTru);
            }

            await _luuTruRepo.SaveChangesAsync();
            await _logRepo.AddLogAsync(currentUserId, "Lưu trữ", $"Lưu hồ sơ {request.IDDangKyDoDac} vào kho {request.Kho}");
            return true;
        }

        public async Task<LuuTruResponseDto?> GetLuuTruByHoSoIdAsync(int idDangKyDoDac)
        {
            var luuTru = await _luuTruRepo.GetByHoSoIdAsync(idDangKyDoDac);
            if (luuTru == null) return null;

            return new LuuTruResponseDto
            {
                Id = luuTru.Id,
                IDDangKyDoDac = luuTru.IddangKyDoDac,
                Kho = luuTru.Kho,
                Gia = luuTru.Gia,
                Ngan = luuTru.Ngan,
                SoHSLuu = luuTru.SoHsluu
            };
        }
    }
}