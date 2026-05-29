using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.DTOs.DoDac;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.Globalization;

namespace HeThongQuanLyVanPhong.Services
{
    public class XuatFileService
    {
        private readonly HeThongQuanLyVanPhongContext _context;

        public XuatFileService(HeThongQuanLyVanPhongContext context)
        {
            _context = context;
        }

        // ==================== XUẤT EXCEL CHI TIẾT HỒ SƠ ====================
        public async Task<byte[]> XuatExcelTheoListIdsAsync(List<int> ids)
        {
            // 1. Query lấy dữ liệu bản vẽ dựa trên list ID truyền vào
            // Sử dụng join giống hệt hàm XuatExcelThanhToanAsync để có đầy đủ thông tin
            var query = from bv in _context.DangKyDoDacBanVes
                        join dk in _context.DangKyDoDacs on bv.IddangKyDoDac equals dk.Id
                        join tt in _context.DangKyDoDacBanVeThanhToans on bv.Id equals tt.IdbanVe into ttGroup
                        from tt in ttGroup.DefaultIfEmpty()
                        join dg in _context.DangKyDoDacDonGia on tt.IddonGia equals dg.Id into dgGroup
                        from dg in dgGroup.DefaultIfEmpty()
                        join xa in _context.Dvhcxas on bv.Idxa equals xa.Id into xaGroup
                        from xa in xaGroup.DefaultIfEmpty()
                        join tk in _context.TaiKhoans on bv.IdnguoiDo equals tk.Id into tkGroup
                        from tk in tkGroup.DefaultIfEmpty()
                        join tkKy in _context.TaiKhoans on bv.IdnguoiKy equals tkKy.Id into kyGroup
                        from tkKy in kyGroup.DefaultIfEmpty()
                        join tkDo in _context.TaiKhoans on dk.IdtaiKhoanDo equals tkDo.Id into tkDoGroup
                        from tkDo in tkDoGroup.DefaultIfEmpty()
                        where ids.Contains(bv.Id)
                        select new { bv, tt, dg, xa, tk, tkKy, dk, tkDo };

            var data = await query.ToListAsync();

            // 2. Vẽ file Excel (Giữ nguyên cấu trúc định dạng của bạn)
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachBanVe");
                string[] headers = {
            "Tên chủ sử dụng", "Địa chỉ thửa đất", "Xã/Phường","Đơn vị đo đạc", "Năm đo", "Số hiệu BV",
            "Loại bản vẽ", "Số tờ", "Số thửa", "Diện tích", "Ngày lập", "Người đo, kiểm tra","Người duyệt",
            "Ký hiệu HĐ", "Số hóa đơn", "Ngày hóa đơn", "Số tiền", "Chi phí lao động", "Tiền làm tròn", "Văn bản quy định giá",
            "Người, đơn vị đăng ký", "Số hợp đồng", "Ngày hợp đồng", "Số điện thoại", "Số Phiếu Giao", "Ngày Giao", "Ngày Yêu Cầu", "Ngày Đo", "Người được giao xử lý"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                // Highlight cột thông tin đăng ký (giống bản cũ)
                worksheet.Range(1, 21, 1, 29).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Range(1, 21, 1, 29).Style.Font.Bold = true;

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.bv.TenCsd;
                    worksheet.Cell(row, 2).Value = item.bv.DiaChiThuaDat;
                    worksheet.Cell(row, 3).Value = item.xa?.TenXa;
                    worksheet.Cell(row, 4).Value = item.bv.DonViDoDac;
                    worksheet.Cell(row, 5).Value = item.bv.NamDoDac;
                    worksheet.Cell(row, 6).Value = item.bv.SoHieuBanVe;
                    worksheet.Cell(row, 7).Value = item.bv.LoaiBanVe;
                    worksheet.Cell(row, 8).Value = item.bv.ToBd;
                    worksheet.Cell(row, 9).Value = item.bv.SoHieuThua;
                    worksheet.Cell(row, 10).Value = item.bv.DienTich;
                    worksheet.Cell(row, 11).Value = item.bv.NgayLap?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 12).Value = item.tk?.HoVaTen;
                    worksheet.Cell(row, 13).Value = item.tkKy?.HoVaTen;
                    worksheet.Cell(row, 14).Value = item.tt?.KyHieuHoaDon;
                    worksheet.Cell(row, 15).Value = item.tt?.SoHoaDon;
                    worksheet.Cell(row, 16).Value = item.tt?.NgayHoaDon?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 17).Value = item.dg?.SoTien ?? 0;
                    worksheet.Cell(row, 18).Value = item.dg?.ChiPhiLaoDong ?? 0;
                    worksheet.Cell(row, 19).Value = item.dg?.TienLamTron ?? 0;
                    worksheet.Cell(row, 20).Value = item.dg?.VanBanQuyDinhGia;
                    worksheet.Cell(row, 21).Value = item.dk.NguoiDangKy;
                    worksheet.Cell(row, 22).Value = item.dk.SoHopDong;
                    worksheet.Cell(row, 23).Value = item.dk.NgayHopDong;
                    worksheet.Cell(row, 24).Value = item.dk.SoDienThoai;
                    worksheet.Cell(row, 25).Value = item.dk.SoPhieuGiao;
                    worksheet.Cell(row, 26).Value = item.dk.NgayGiao?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 27).Value = item.dk.NgayYeuCau?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 28).Value = item.dk.NgayDo?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 29).Value = item.tkDo?.HoVaTen;

                    worksheet.Range(row, 17, row, 19).Style.NumberFormat.Format = "#,##0";
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> XuatExcelHoSoTheoIdsAsync(List<int> ids)
        {
            var query = from dk in _context.DangKyDoDacs
                        join xa in _context.Dvhcxas on dk.Idxa equals xa.Id into xaGroup
                        from xa in xaGroup.DefaultIfEmpty()
                        where ids.Contains(dk.Id)
                        select new { dk, TenXa = xa.TenXa }; // Chọn cả hồ sơ và tên xã

            var data = await query.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachHoSo");
                // Thêm cột "Xã/Phường" vào header
                string[] headers = { "STT", "Số hợp đồng", "Ngày hợp đồng", "Người đăng ký", "Địa chỉ", "Xã/Phường", "Số điện thoại" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = row - 1;
                    worksheet.Cell(row, 2).Value = item.dk.SoHopDong;
                    worksheet.Cell(row, 3).Value = item.dk.NgayHopDong;
                    worksheet.Cell(row, 4).Value = item.dk.NguoiDangKy;                    
                    worksheet.Cell(row, 6).Value = item.dk.DiaChiThuaDat;
                    worksheet.Cell(row, 5).Value = item.TenXa ?? ""; // Tên xã lấy từ kết quả join
                    worksheet.Cell(row, 7).Value = item.dk.SoDienThoai;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> XuatExcelChiTietAsync(XuatExcelRequestDto request)
        {
            var query = _context.DangKyDoDacs
                .Include(x => x.IdxaNavigation)
                .Include(x => x.IdtaiKhoanDoNavigation)
                .AsQueryable();

            if (request.IdNhanVien.HasValue && request.IdNhanVien > 0)
                query = query.Where(x => x.IdtaiKhoanDo == request.IdNhanVien);
            else
            {
                if (request.IdDonVi.HasValue)
                    query = query.Where(x => x.IddonViCongTac == request.IdDonVi);
                if (!string.IsNullOrEmpty(request.TrangThai))
                    query = query.Where(x => x.TrangThaiDo == request.TrangThai);
            }

            var dataRaw = await query.ToListAsync();
            DateTime dTu = DateTime.MinValue;
            if (!string.IsNullOrEmpty(request.TuNgay))
            {
                dTu = DateTime.ParseExact(request.TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
            }

            DateTime dDen = DateTime.MaxValue;
            if (!string.IsNullOrEmpty(request.DenNgay))
            {
                dDen = DateTime.ParseExact(request.DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
            }
            DateTime ngayHienTai = DateTime.Now.Date;

            var list = dataRaw.Where(x =>
            {
                if (DateTime.TryParseExact(x.NgayHopDong, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime d))
                    return d.Date >= dTu && d.Date <= dDen;
                return false;
            }).OrderByDescending(x => x.Id).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachHoSo");

                worksheet.Cell(1, 1).Value = "DANH SÁCH CHI TIẾT HỒ SƠ";
                worksheet.Range(1, 1, 1, 11).Merge().Style.Font.SetBold().Font.SetFontSize(14);
                worksheet.Cell(2, 1).Value = $"Thời gian: {request.TuNgay} đến {request.DenNgay}";
                worksheet.Range(2, 1, 2, 11).Merge();

                string[] headers = { "STT", "Số Hợp Đồng", "Chủ Sử Dụng", "Ngày đăng ký", "Địa chỉ thửa đất", "ĐVHC xã/phường", "Người giải quyết", "Ngày Yêu Cầu", "Ngày đo", "Ngày Trả kết quả", "Tình Trạng Hạn" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(4, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4e73df");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Font.Bold = true;
                }

                int row = 5;
                foreach (var item in list)
                {
                    DateTime nyc = item.NgayYeuCau.HasValue ? item.NgayYeuCau.Value.ToDateTime(TimeOnly.MinValue) : ngayHienTai;
                    DateTime ntkq = item.NgayTraKetQua.HasValue ? item.NgayTraKetQua.Value.ToDateTime(TimeOnly.MinValue) : ngayHienTai;
                    bool laQuaHan = ntkq.Date > nyc.Date;

                    worksheet.Cell(row, 1).Value = row - 4;
                    worksheet.Cell(row, 2).Value = item.SoHopDong;
                    worksheet.Cell(row, 3).Value = item.NguoiDangKy;
                    worksheet.Cell(row, 4).Value = item.NgayHopDong;
                    worksheet.Cell(row, 5).Value = item.DiaChiThuaDat;
                    worksheet.Cell(row, 6).Value = item.IdxaNavigation?.TenXa ?? "";
                    worksheet.Cell(row, 7).Value = item.IdtaiKhoanDoNavigation?.HoVaTen ?? "Chưa phân công";
                    worksheet.Cell(row, 8).Value = item.NgayYeuCau?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 9).Value = item.NgayDo?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 10).Value = item.NgayTraKetQua?.ToString("dd/MM/yyyy");

                    var cellTinhTrang = worksheet.Cell(row, 11);
                    cellTinhTrang.Value = laQuaHan ? "Quá hạn" : "Trong hạn";
                    if (laQuaHan) cellTinhTrang.Style.Font.FontColor = XLColor.Red;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        // ==================== XUẤT EXCEL BÁO CÁO THANH TOÁN ====================
        public async Task<byte[]> XuatExcelThanhToanAsync(XuatExcelThanhToanRequestDto request)
        {
            var query = from tt in _context.DangKyDoDacBanVeThanhToans
                        join bv in _context.DangKyDoDacBanVes on tt.IdbanVe equals bv.Id
                        join dk in _context.DangKyDoDacs on bv.IddangKyDoDac equals dk.Id
                        join dg in _context.DangKyDoDacDonGia on tt.IddonGia equals dg.Id into dgGroup
                        from dg in dgGroup.DefaultIfEmpty()
                        join xa in _context.Dvhcxas on bv.Idxa equals xa.Id into xaGroup
                        from xa in xaGroup.DefaultIfEmpty()
                        join tk in _context.TaiKhoans on bv.IdnguoiDo equals tk.Id into tkGroup
                        from tk in tkGroup.DefaultIfEmpty()
                        join tkKy in _context.TaiKhoans on bv.IdnguoiKy equals tkKy.Id into kyGroup
                        from tkKy in kyGroup.DefaultIfEmpty()
                        join tkDo in _context.TaiKhoans on dk.IdtaiKhoanDo equals tkDo.Id into tkDoGroup
                        from tkDo in tkDoGroup.DefaultIfEmpty()
                        where tt.NgayHoaDon >= DateOnly.FromDateTime(request.TuNgay) && tt.NgayHoaDon <= DateOnly.FromDateTime(request.DenNgay)
                        select new { bv, tt, dg, xa, tk, tkKy, dk, tkDo };

            if (request.DaThanhToan == "true") query = query.Where(x => x.tt.DaThanhToan == true);
            else if (request.DaThanhToan == "false") query = query.Where(x => x.tt.DaThanhToan == false);
            if (request.IdTinh.HasValue && request.IdTinh > 0) query = query.Where(x => x.bv.Idtinh == request.IdTinh);
            if (request.IdDonVi.HasValue && request.IdDonVi > 0) query = query.Where(x => x.tt.IddonViCongTac == request.IdDonVi);

            var data = await query.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoChiTiet");
                string[] headers = {
                    "Tên chủ sử dụng", "Địa chỉ thửa đất", "Xã/Phường","Đơn vị đo đạc", "Năm đo", "Số hiệu BV",
                    "Loại bản vẽ", "Số tờ", "Số thửa", "Diện tích", "Ngày lập", "Người đo, kiểm tra","Người duyệt",
                    "Ký hiệu HĐ", "Số hóa đơn", "Ngày hóa đơn", "Số tiền", "Chi phí lao động", "Tiền làm tròn", "Văn bản quy định giá",
                    "Người, đơn vị đăng ký", "Số hợp đồng", "Ngày hợp đồng", "Số điện thoại", "Số Phiếu Giao", "Ngày Giao", "Ngày Yêu Cầu", "Ngày Đo", "Người được giao xử lý"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                }
                worksheet.Range(1, 21, 1, 29).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Range(1, 21, 1, 29).Style.Font.Bold = true;
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.bv.TenCsd;
                    worksheet.Cell(row, 2).Value = item.bv.DiaChiThuaDat;
                    worksheet.Cell(row, 3).Value = item.xa?.TenXa;
                    worksheet.Cell(row, 4).Value = item.bv.DonViDoDac;
                    worksheet.Cell(row, 5).Value = item.bv.NamDoDac;
                    worksheet.Cell(row, 6).Value = item.bv.SoHieuBanVe;
                    worksheet.Cell(row, 7).Value = item.bv.LoaiBanVe;
                    worksheet.Cell(row, 8).Value = item.bv.ToBd;
                    worksheet.Cell(row, 9).Value = item.bv.SoHieuThua;
                    worksheet.Cell(row, 10).Value = item.bv.DienTich;
                    worksheet.Cell(row, 11).Value = item.bv.NgayLap?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 12).Value = item.tk?.HoVaTen;
                    worksheet.Cell(row, 13).Value = item.tkKy?.HoVaTen;
                    worksheet.Cell(row, 14).Value = item.tt.KyHieuHoaDon;
                    worksheet.Cell(row, 15).Value = item.tt.SoHoaDon;
                    worksheet.Cell(row, 16).Value = item.tt.NgayHoaDon?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 17).Value = item.dg?.SoTien ?? 0;
                    worksheet.Cell(row, 18).Value = item.dg?.ChiPhiLaoDong ?? 0;
                    worksheet.Cell(row, 19).Value = item.dg?.TienLamTron ?? 0;
                    worksheet.Cell(row, 20).Value = item.dg?.VanBanQuyDinhGia;
                    worksheet.Cell(row, 21).Value = item.dk.NguoiDangKy;
                    worksheet.Cell(row, 22).Value = item.dk.SoHopDong;
                    worksheet.Cell(row, 23).Value = item.dk.NgayHopDong;
                    worksheet.Cell(row, 24).Value = item.dk.SoDienThoai;
                    worksheet.Cell(row, 25).Value = item.dk.SoPhieuGiao;
                    worksheet.Cell(row, 26).Value = item.dk.NgayGiao?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 27).Value = item.dk.NgayYeuCau?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 28).Value = item.dk.NgayDo?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 29).Value = item.tkDo?.HoVaTen;
                    worksheet.Range(row, 17, row, 19).Style.NumberFormat.Format = "#,##0";
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        // ==================== HÀM ĐỌC SỐ THÀNH CHỮ ====================
        private string DocSoThanhChu(decimal amount)
        {
            if (amount == 0) return "Không đồng";
            if (amount < 0) return "Số tiền âm";

            string[] unitNumbers = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

            string sNumber = ((long)Math.Round(amount, 0)).ToString();
            string res = "";
            int i, j, unit, chuc, tram;

            i = sNumber.Length;
            if (i == 0) res = unitNumbers[0] + res;
            else
            {
                j = 0;
                while (i > 0)
                {
                    unit = i >= 1 ? int.Parse(sNumber.Substring(i - 1, 1)) : -1;
                    i--;
                    chuc = i >= 1 ? int.Parse(sNumber.Substring(i - 1, 1)) : -1;
                    i--;
                    tram = i >= 1 ? int.Parse(sNumber.Substring(i - 1, 1)) : -1;
                    i--;
                    if ((unit > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        res = placeValues[j] + res;
                    j++;
                    if (j > 3) j = 1;
                    if ((unit == 1) && (chuc > 1)) res = " mốt" + res;
                    else if ((unit == 5) && (chuc > 0)) res = " lăm" + res;
                    else if (unit > 0) res = " " + unitNumbers[unit] + res;

                    if (chuc < 0) break;
                    if ((chuc == 0) && (unit > 0)) res = " linh" + res;
                    if (chuc == 1) res = " mười" + res;
                    if (chuc > 1) res = " " + unitNumbers[chuc] + " mươi" + res;

                    if (tram < 0) break;
                    if ((tram > 0) || (chuc > 0) || (unit > 0)) res = " " + unitNumbers[tram] + " trăm" + res;
                }
            }
            res = res.Trim();
            if (string.IsNullOrEmpty(res)) return "Không đồng";
            return res.Substring(0, 1).ToUpper() + res.Substring(1) + " Việt Nam đồng";
        }

        // ==================== XUẤT WORD HỢP ĐỒNG THANH LÝ ====================
        /* public async Task<byte[]> XuatWordThanhLyAsync(XuatWordThanhLyRequestDto request, IFormFile templateFile)
         {
             var hoSo = await _context.DangKyDoDacs
                 .Include(x => x.IdxaNavigation)
                 .Include(x => x.IdtinhNavigation)
                 .Include(x => x.IddonViCongTacNavigation)
                 .Include(x => x.IdtaiKhoanNavigation)
                 .FirstOrDefaultAsync(x => x.Id == request.IdDangKyDoDac);

             if (hoSo == null) throw new Exception("Không tìm thấy hồ sơ");

             // Xử lý ngày tháng
             DateTime ngayLapVBNhan;
             if (!DateTime.TryParseExact(request.NgayLapStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayLapVBNhan))
                 ngayLapVBNhan = DateTime.Now;

             string sNgay = ngayLapVBNhan.Day.ToString("00");
             string sThang = ngayLapVBNhan.Month.ToString("00");
             string sNam = ngayLapVBNhan.Year.ToString();
             decimal tienConLai = request.TongTien - request.TienUng;

             var replacements = new Dictionary<string, string>
             {
                 ["[SoHopDong]"] = hoSo.SoHopDong ?? "",
                 ["[NguoiDangKy]"] = hoSo.NguoiDangKy ?? "",
                 ["[DiaChiThuaDat]"] = hoSo.DiaChiThuaDat ?? "",
                 ["[TenXa]"] = hoSo.IdxaNavigation?.TenXa ?? "",
                 ["[TenTinh]"] = hoSo.IdtinhNavigation?.TenTinh ?? "",
                 ["[TenDonVi]"] = hoSo.IddonViCongTacNavigation?.TenDonVi ?? "",
                 ["[CanBoXuLy]"] = hoSo.IdtaiKhoanNavigation?.HoVaTen ?? "",
                 ["[CCCD]"] = hoSo.Cccd ?? "",
                 ["[SoDienThoai]"] = hoSo.SoDienThoai ?? "",
                 ["[MucDichDangKy]"] = hoSo.MucDichDangKy ?? "",
                 ["[NgayHopDong]"] = hoSo.NgayHopDong ?? "",
                 ["[NgayLapVB]"] = $"{sNgay}/{sThang}/{sNam}",
                 ["[NgayVB]"] = sNgay,
                 ["[ThangVB]"] = sThang,
                 ["[NamVB]"] = sNam,
                 ["[TienThanhLy]"] = request.TongTien.ToString("N0"),
                 ["[TienThanhLyBangChu]"] = DocSoThanhChu(request.TongTien),
                 ["[TienUng]"] = request.TienUng.ToString("N0"),
                 ["[TienUngBangChu]"] = DocSoThanhChu(request.TienUng),
                 ["[TienConLai]"] = tienConLai.ToString("N0"),
                 ["[TienConLaiBangChu]"] = DocSoThanhChu(tienConLai)
             };

             // Hàm đệ quy thay thế trong tất cả các phần tử (body, header, footer, textbox)
             void ReplaceInElement(OpenXmlElement element)
             {
                 if (element is Text text && text.Text != null)
                 {
                     string newText = text.Text;
                     foreach (var kvp in replacements)
                     {
                         if (newText.Contains(kvp.Key))
                             newText = newText.Replace(kvp.Key, kvp.Value);
                     }
                     if (newText != text.Text)
                         text.Text = newText;
                 }
                 foreach (var child in element.Elements())
                     ReplaceInElement(child);
             }

             using (var memoryStream = new MemoryStream())
             {
                 await templateFile.CopyToAsync(memoryStream);
                 memoryStream.Position = 0;

                 using (var wordDocument = WordprocessingDocument.Open(memoryStream, true))
                 {
                     var mainPart = wordDocument.MainDocumentPart;
                     if (mainPart == null) throw new Exception("Không thể mở document");

                     // Thay thế trong body
                     ReplaceInElement(mainPart.Document);

                     // Thay thế trong header
                     foreach (var headerPart in mainPart.HeaderParts)
                         if (headerPart.Header != null)
                             ReplaceInElement(headerPart.Header);

                     // Thay thế trong footer
                     foreach (var footerPart in mainPart.FooterParts)
                         if (footerPart.Footer != null)
                             ReplaceInElement(footerPart.Footer);

                     wordDocument.Save();
                 }
                 return memoryStream.ToArray();
             }
         }*/

        public async Task<byte[]> XuatWordThanhLyAsync(XuatWordThanhLyRequestDto request, IFormFile templateFile)
        {
            var hoSo = await _context.DangKyDoDacs
                .Include(x => x.IdxaNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IddonViCongTacNavigation)
                .Include(x => x.IdtaiKhoanNavigation)
                .FirstOrDefaultAsync(x => x.Id == request.IdDangKyDoDac);

            if (hoSo == null) throw new Exception("Không tìm thấy hồ sơ");

            DateTime ngayLapVBNhan;
            if (!DateTime.TryParseExact(request.NgayLapStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayLapVBNhan))
                ngayLapVBNhan = DateTime.Now;

            string sNgay = ngayLapVBNhan.Day.ToString("00");
            string sThang = ngayLapVBNhan.Month.ToString("00");
            string sNam = ngayLapVBNhan.Year.ToString();
            decimal tienConLai = request.TongTien - request.TienUng;

            var replacements = new Dictionary<string, string>
            {
                ["[SoHopDong]"] = hoSo.SoHopDong ?? "",
                ["[NguoiDangKy]"] = hoSo.NguoiDangKy ?? "",
                ["[DiaChiThuaDat]"] = hoSo.DiaChiThuaDat ?? "",
                ["[TenXa]"] = hoSo.IdxaNavigation?.TenXa ?? "",
                ["[TenTinh]"] = hoSo.IdtinhNavigation?.TenTinh ?? "",
                ["[TenDonVi]"] = hoSo.IddonViCongTacNavigation?.TenDonVi ?? "",
                ["[CanBoXuLy]"] = hoSo.IdtaiKhoanNavigation?.HoVaTen ?? "",
                ["[CCCD]"] = hoSo.Cccd ?? "",
                ["[SoDienThoai]"] = hoSo.SoDienThoai ?? "",
                ["[MucDichDangKy]"] = hoSo.MucDichDangKy ?? "",
                ["[NgayHopDong]"] = hoSo.NgayHopDong ?? "",
                ["[NgayLapVB]"] = $"{sNgay}/{sThang}/{sNam}",
                ["[NgayVB]"] = sNgay,
                ["[ThangVB]"] = sThang,
                ["[NamVB]"] = sNam,
                ["[TienThanhLy]"] = request.TongTien.ToString("N0"),
                ["[TienThanhLyBangChu]"] = DocSoThanhChu(request.TongTien),
                ["[TienUng]"] = request.TienUng.ToString("N0"),
                ["[TienUngBangChu]"] = DocSoThanhChu(request.TienUng),
                ["[TienConLai]"] = tienConLai.ToString("N0"),
                ["[TienConLaiBangChu]"] = DocSoThanhChu(tienConLai)
            };

            using (var memoryStream = new MemoryStream())
            {
                await templateFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using (var wordDocument = WordprocessingDocument.Open(memoryStream, true))
                {
                    var mainPart = wordDocument.MainDocumentPart;
                    if (mainPart == null) throw new Exception("Không thể mở document");

                    ReplaceInElement(mainPart.Document, replacements);

                    foreach (var headerPart in mainPart.HeaderParts)
                        if (headerPart.Header != null)
                            ReplaceInElement(headerPart.Header, replacements);

                    foreach (var footerPart in mainPart.FooterParts)
                        if (footerPart.Footer != null)
                            ReplaceInElement(footerPart.Footer, replacements);

                    wordDocument.Save();
                }
                return memoryStream.ToArray();
            }
        }

        private void MergeRunsInParagraph(Paragraph paragraph)
        {
            var runs = paragraph.Elements<Run>().ToList();
            if (runs.Count <= 1) return;

            string fullText = string.Concat(runs.Select(r =>
                string.Concat(r.Elements<Text>().Select(t => t.Text))));

            var firstRun = runs[0];
            foreach (var run in runs.Skip(1))
                run.Remove();

            foreach (var t in firstRun.Elements<Text>().ToList())
                t.Remove();

            firstRun.AppendChild(new Text(fullText)
            {
                Space = SpaceProcessingModeValues.Preserve
            });
        }

        private void ReplaceInElement(OpenXmlElement element, Dictionary<string, string> replacements)
        {
            foreach (var paragraph in element.Descendants<Paragraph>())
            {
                MergeRunsInParagraph(paragraph);
            }
            foreach (var text in element.Descendants<Text>())
            {
                if (string.IsNullOrEmpty(text.Text)) continue;

                string newText = text.Text;
                foreach (var kvp in replacements)
                {
                    if (newText.Contains(kvp.Key))
                    {
                        newText = newText.Replace(kvp.Key, kvp.Value ?? "");
                    }
                }

                if (newText != text.Text)
                {
                    text.Text = newText;
                }
            }
        }
    }
}