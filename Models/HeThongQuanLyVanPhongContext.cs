using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HeThongQuanLyVanPhong.Models;

public partial class HeThongQuanLyVanPhongContext : DbContext
{
    public HeThongQuanLyVanPhongContext()
    {
    }

    public HeThongQuanLyVanPhongContext(DbContextOptions<HeThongQuanLyVanPhongContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChucVu> ChucVus { get; set; }

    public virtual DbSet<DangKyDoDac> DangKyDoDacs { get; set; }

    public virtual DbSet<DangKyDoDacBanVe> DangKyDoDacBanVes { get; set; }

    public virtual DbSet<DangKyDoDacBanVeThanhToan> DangKyDoDacBanVeThanhToans { get; set; }

    public virtual DbSet<DangKyDoDacDonGium> DangKyDoDacDonGia { get; set; }

    public virtual DbSet<DangKyDoDacLog> DangKyDoDacLogs { get; set; }

    public virtual DbSet<DangKyDoDacLuuTru> DangKyDoDacLuuTrus { get; set; }

    public virtual DbSet<DangKyDoDacSoManhTd> DangKyDoDacSoManhTds { get; set; }

    public virtual DbSet<DonViCongTac> DonViCongTacs { get; set; }

    public virtual DbSet<DonViHanhChinhTinh> DonViHanhChinhTinhs { get; set; }

    public virtual DbSet<Dvhcxa> Dvhcxas { get; set; }

    public virtual DbSet<FileHoSoDoDac> FileHoSoDoDacs { get; set; }

    public virtual DbSet<LoaiBanVe> LoaiBanVes { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<PhanQuyenModule> PhanQuyenModules { get; set; }

    public virtual DbSet<QuyTrinhXuLy> QuyTrinhXuLies { get; set; }

    public virtual DbSet<QuyTrinhXuLyBuocQuyTrinh> QuyTrinhXuLyBuocQuyTrinhs { get; set; }

    public virtual DbSet<QuyTrinhXuLyNhayBuoc> QuyTrinhXuLyNhayBuocs { get; set; }

    public virtual DbSet<SoHopDongDoDac> SoHopDongDoDacs { get; set; }

    public virtual DbSet<SystemLog> SystemLogs { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TrangThaiBanVe> TrangThaiBanVes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChucVu__3214EC27C756C76D");

            entity.ToTable("ChucVu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenChucVu).HasMaxLength(255);
        });

        modelBuilder.Entity<DangKyDoDac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC27F95B78D1");

            entity.ToTable("DangKyDoDac");

            entity.HasIndex(e => e.IdbuocQuyTrinh, "IX_DangKyDoDac_IDBuocQuyTrinh");

            entity.HasIndex(e => e.IddonViCongTac, "IX_DangKyDoDac_IDDonViCongTac");

            entity.HasIndex(e => e.IdquyTrinh, "IX_DangKyDoDac_IDQuyTrinh");

            entity.HasIndex(e => e.IdtaiKhoan, "IX_DangKyDoDac_IDTaiKhoan");

            entity.HasIndex(e => e.IdtaiKhoanDo, "IX_DangKyDoDac_IDTaiKhoanDo");

            entity.HasIndex(e => e.Idtinh, "IX_DangKyDoDac_IDTinh");

            entity.HasIndex(e => e.Idxa, "IX_DangKyDoDac_IDXa");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.IdbuocQuyTrinh).HasColumnName("IDBuocQuyTrinh");
            entity.Property(e => e.IddonViCongTac).HasColumnName("IDDonViCongTac");
            entity.Property(e => e.IdquyTrinh).HasColumnName("IDQuyTrinh");
            entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");
            entity.Property(e => e.IdtaiKhoanDo).HasColumnName("IDTaiKhoanDo");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.Idxa).HasColumnName("IDXa");
            entity.Property(e => e.NgayHopDong).HasMaxLength(50);
            entity.Property(e => e.NguoiDangKy).HasMaxLength(255);
            entity.Property(e => e.SeriGCN)
                .HasMaxLength(255)
                .HasColumnName("SeriGCN");
            entity.Property(e => e.SoDienThoai).HasMaxLength(50);
            entity.Property(e => e.SoHopDong).HasMaxLength(255);
            entity.Property(e => e.SoPhieuGiao).HasMaxLength(255);
            entity.Property(e => e.TrangThaiDo).HasMaxLength(255);

            entity.HasOne(d => d.IdbuocQuyTrinhNavigation).WithMany(p => p.DangKyDoDacs)
                .HasForeignKey(d => d.IdbuocQuyTrinh)
                .HasConstraintName("FK__DangKyDoD__IDBuo__5BE2A6F2");

            entity.HasOne(d => d.IddonViCongTacNavigation).WithMany(p => p.DangKyDoDacs)
                .HasForeignKey(d => d.IddonViCongTac)
                .HasConstraintName("FK__DangKyDoD__IDDon__59FA5E80");

            entity.HasOne(d => d.IdquyTrinhNavigation).WithMany(p => p.DangKyDoDacs)
                .HasForeignKey(d => d.IdquyTrinh)
                .HasConstraintName("FK__DangKyDoD__IDQuy__5AEE82B9");

            entity.HasOne(d => d.IdtaiKhoanNavigation).WithMany(p => p.DangKyDoDacIdtaiKhoanNavigations)
                .HasForeignKey(d => d.IdtaiKhoan)
                .HasConstraintName("FK__DangKyDoD__IDTai__5CD6CB2B");

            entity.HasOne(d => d.IdtaiKhoanDoNavigation).WithMany(p => p.DangKyDoDacIdtaiKhoanDoNavigations)
                .HasForeignKey(d => d.IdtaiKhoanDo)
                .HasConstraintName("FK__DangKyDoD__IDTai__5FB337D6");

            entity.HasOne(d => d.IdtinhNavigation).WithMany(p => p.DangKyDoDacs)
                .HasForeignKey(d => d.Idtinh)
                .HasConstraintName("FK__DangKyDoD__IDTin__5EBF139D");

            entity.HasOne(d => d.IdxaNavigation).WithMany(p => p.DangKyDoDacs)
                .HasForeignKey(d => d.Idxa)
                .HasConstraintName("FK__DangKyDoDa__IDXa__5DCAEF64");
        });

        modelBuilder.Entity<DangKyDoDacBanVe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC277B58956D");

            entity.ToTable("DangKyDoDac_BanVe");

            entity.HasIndex(e => e.IddangKyDoDac, "IX_DangKyDoDac_BanVe_IDDangKyDoDac");

            entity.HasIndex(e => e.IdnguoiDo, "IX_DangKyDoDac_BanVe_IDNguoiDo");

            entity.HasIndex(e => e.IdnguoiKy, "IX_DangKyDoDac_BanVe_IDNguoiKy");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DiaChiThuaDat).HasMaxLength(250);
            entity.Property(e => e.DienTich).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.IddangKyDoDac).HasColumnName("IDDangKyDoDac");
            entity.Property(e => e.IdnguoiDo).HasColumnName("IDNguoiDo");
            entity.Property(e => e.IdnguoiKy).HasColumnName("IDNguoiKy");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.Idxa).HasColumnName("IDXa");
            entity.Property(e => e.LoaiBanVe).HasMaxLength(250);
            entity.Property(e => e.NgayVb).HasColumnName("NgayVB");
            entity.Property(e => e.SoHieuThua).HasMaxLength(150);
            entity.Property(e => e.SoVb)
                .HasMaxLength(20)
                .HasColumnName("SoVB");
            entity.Property(e => e.TenCsd)
                .HasMaxLength(250)
                .HasColumnName("TenCSD");
            entity.Property(e => e.ToBd)
                .HasMaxLength(250)
                .HasColumnName("ToBD");
            entity.Property(e => e.TrangThai).HasMaxLength(250);

            entity.HasOne(d => d.IddangKyDoDacNavigation).WithMany(p => p.DangKyDoDacBanVes)
                .HasForeignKey(d => d.IddangKyDoDac)
                .HasConstraintName("FK__DangKyDoD__IDDan__628FA481");

            entity.HasOne(d => d.IdnguoiDoNavigation).WithMany(p => p.DangKyDoDacBanVeIdnguoiDoNavigations)
                .HasForeignKey(d => d.IdnguoiDo)
                .HasConstraintName("FK__DangKyDoD__IDNgu__656C112C");

            entity.HasOne(d => d.IdnguoiKyNavigation).WithMany(p => p.DangKyDoDacBanVeIdnguoiKyNavigations)
                .HasForeignKey(d => d.IdnguoiKy)
                .HasConstraintName("FK__DangKyDoD__IDNgu__66603565");

            entity.HasOne(d => d.IdtinhNavigation).WithMany(p => p.DangKyDoDacBanVes)
                .HasForeignKey(d => d.Idtinh)
                .HasConstraintName("FK__DangKyDoD__IDTin__6477ECF3");

            entity.HasOne(d => d.IdxaNavigation).WithMany(p => p.DangKyDoDacBanVes)
                .HasForeignKey(d => d.Idxa)
                .HasConstraintName("FK__DangKyDoDa__IDXa__6383C8BA");
        });

        modelBuilder.Entity<DangKyDoDacBanVeThanhToan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC27F508CC01");

            entity.ToTable("DangKyDoDac_BanVe_ThanhToan");

            entity.HasIndex(e => e.IdbanVe, "IX_DangKyDoDac_BanVe_ThanhToan_IDBanVe");

            entity.HasIndex(e => e.IddangKyDoDac, "IX_DangKyDoDac_BanVe_ThanhToan_IDDangKyDoDac");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdbanVe).HasColumnName("IDBanVe");
            entity.Property(e => e.IddangKyDoDac).HasColumnName("IDDangKyDoDac");
            entity.Property(e => e.IddonGia).HasColumnName("IDDonGia");
            entity.Property(e => e.IddonViCongTac).HasColumnName("IDDonViCongTac");
            entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");
            entity.Property(e => e.KyHieuHoaDon).HasMaxLength(255);
            entity.Property(e => e.SoHoaDon).HasMaxLength(255);

            entity.HasOne(d => d.IdbanVeNavigation).WithMany(p => p.DangKyDoDacBanVeThanhToans)
                .HasForeignKey(d => d.IdbanVe)
                .HasConstraintName("FK__DangKyDoD__IDBan__7D439ABD");

            entity.HasOne(d => d.IddangKyDoDacNavigation).WithMany(p => p.DangKyDoDacBanVeThanhToans)
                .HasForeignKey(d => d.IddangKyDoDac)
                .HasConstraintName("FK__DangKyDoD__IDDan__7E37BEF6");

            entity.HasOne(d => d.IddonGiaNavigation).WithMany(p => p.DangKyDoDacBanVeThanhToans)
                .HasForeignKey(d => d.IddonGia)
                .HasConstraintName("FK__DangKyDoD__IDDon__01142BA1");

            entity.HasOne(d => d.IddonViCongTacNavigation).WithMany(p => p.DangKyDoDacBanVeThanhToans)
                .HasForeignKey(d => d.IddonViCongTac)
                .HasConstraintName("FK__DangKyDoD__IDDon__7F2BE32F");

            entity.HasOne(d => d.IdtaiKhoanNavigation).WithMany(p => p.DangKyDoDacBanVeThanhToans)
                .HasForeignKey(d => d.IdtaiKhoan)
                .HasConstraintName("FK__DangKyDoD__IDTai__00200768");
        });

        modelBuilder.Entity<DangKyDoDacDonGium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC27AC5D2A2E");

            entity.ToTable("DangKyDoDac_DonGia");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DinhMucTinh).HasMaxLength(255);
            entity.Property(e => e.LoaiBanVe).HasMaxLength(255);
            entity.Property(e => e.LoaiKhuVuc).HasMaxLength(255);
            entity.Property(e => e.VanBanQuyDinhGia).HasMaxLength(255);
        });

        modelBuilder.Entity<DangKyDoDacLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC27A35FE07E");

            entity.ToTable("DangKyDoDac_log");

            entity.HasIndex(e => e.IddangKyDoDac, "IX_DangKyDoDac_log_IDDangKyDoDac");

            entity.HasIndex(e => e.UserId, "IX_DangKyDoDac_log_user_id");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.IddangKyDoDac).HasColumnName("IDDangKyDoDac");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.IddangKyDoDacNavigation).WithMany(p => p.DangKyDoDacLogs)
                .HasForeignKey(d => d.IddangKyDoDac)
                .HasConstraintName("FK__DangKyDoD__IDDan__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.DangKyDoDacLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DangKyDoD__user___693CA210");
        });

        modelBuilder.Entity<DangKyDoDacLuuTru>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC27FC74E21D");

            entity.ToTable("DangKyDoDac_LuuTru");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Gia).HasMaxLength(255);
            entity.Property(e => e.IddangKyDoDac).HasColumnName("IDDangKyDoDac");
            entity.Property(e => e.Kho).HasMaxLength(255);
            entity.Property(e => e.Ngan).HasMaxLength(255);
            entity.Property(e => e.SoHsluu)
                .HasMaxLength(255)
                .HasColumnName("SoHSLuu");

            entity.HasOne(d => d.IddangKyDoDacNavigation).WithMany(p => p.DangKyDoDacLuuTrus)
                .HasForeignKey(d => d.IddangKyDoDac)
                .HasConstraintName("FK__DangKyDoD__IDDan__73BA3083");
        });

        modelBuilder.Entity<DangKyDoDacSoManhTd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyDo__3214EC276752593A");

            entity.ToTable("DangKyDoDac_SoManhTD");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IddonViCongTac).HasColumnName("IDDonViCongTac");
            entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");
            entity.Property(e => e.MaXa).HasMaxLength(50);
            entity.Property(e => e.NgayLay).HasColumnType("datetime");
            entity.Property(e => e.ShbanVe).HasColumnName("SHBanVe");

            entity.HasOne(d => d.IddonViCongTacNavigation).WithMany(p => p.DangKyDoDacSoManhTds)
                .HasForeignKey(d => d.IddonViCongTac)
                .HasConstraintName("FK__DangKyDoD__IDDon__778AC167");

            entity.HasOne(d => d.IdtaiKhoanNavigation).WithMany(p => p.DangKyDoDacSoManhTds)
                .HasForeignKey(d => d.IdtaiKhoan)
                .HasConstraintName("FK__DangKyDoD__IDTai__76969D2E");
        });

        modelBuilder.Entity<DonViCongTac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonViCon__3214EC27059CA018");

            entity.ToTable("DonViCongTac");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IddonViHanhChinhTinh).HasColumnName("IDDonViHanhChinhTinh");
            entity.Property(e => e.TenDonVi).HasMaxLength(255);

            entity.HasOne(d => d.IddonViHanhChinhTinhNavigation).WithMany(p => p.DonViCongTacs)
                .HasForeignKey(d => d.IddonViHanhChinhTinh)
                .HasConstraintName("FK__DonViCong__IDDon__3B75D760");
        });

        modelBuilder.Entity<DonViHanhChinhTinh>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonViHan__3214EC274381A97B");

            entity.ToTable("DonViHanhChinhTinh");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaTinh).HasMaxLength(50);
            entity.Property(e => e.TenTinh).HasMaxLength(255);
        });

        modelBuilder.Entity<Dvhcxa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DVHCxa__3214EC276B2B5886");

            entity.ToTable("DVHCxa");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.MaXa).HasMaxLength(50);
            entity.Property(e => e.TenXa).HasMaxLength(255);

            entity.HasOne(d => d.IdtinhNavigation).WithMany(p => p.Dvhcxas)
                .HasForeignKey(d => d.Idtinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DVHCxa__IDTinh__571DF1D5");
        });

        modelBuilder.Entity<FileHoSoDoDac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FileHoSo__3214EC27283AFA89");

            entity.ToTable("FileHoSoDoDac");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IddangKyDoDac).HasColumnName("IDDangKyDoDac");
            entity.Property(e => e.NoiDungSoBo).HasMaxLength(250);
            entity.Property(e => e.TenFileLuu).HasMaxLength(150);

            entity.HasOne(d => d.IddangKyDoDacNavigation).WithMany(p => p.FileHoSoDoDacs)
                .HasForeignKey(d => d.IddangKyDoDac)
                .HasConstraintName("FK__FileHoSoD__IDDan__70DDC3D8");
        });

        modelBuilder.Entity<LoaiBanVe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiBanV__3214EC270310555B");

            entity.ToTable("LoaiBanVe");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenLoaiBanVe).HasMaxLength(100);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Module__3214EC27F2668066");

            entity.ToTable("Module");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenModule).HasMaxLength(255);
        });

        modelBuilder.Entity<PhanQuyenModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhanQuye__3214EC270E85CE83");

            entity.ToTable("PhanQuyenModule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idmodule).HasColumnName("IDModule");
            entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");

            entity.HasOne(d => d.IdmoduleNavigation).WithMany(p => p.PhanQuyenModules)
                .HasForeignKey(d => d.Idmodule)
                .HasConstraintName("FK__PhanQuyen__IDMod__44FF419A");

            entity.HasOne(d => d.IdtaiKhoanNavigation).WithMany(p => p.PhanQuyenModules)
                .HasForeignKey(d => d.IdtaiKhoan)
                .HasConstraintName("FK__PhanQuyen__IDTai__440B1D61");
        });

        modelBuilder.Entity<QuyTrinhXuLy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuyTrinh__3214EC27C3B50040");

            entity.ToTable("QuyTrinhXuLy");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LoaiQuyTrinh).HasMaxLength(255);
            entity.Property(e => e.TenQuyTrinh).HasMaxLength(255);
        });

        modelBuilder.Entity<QuyTrinhXuLyBuocQuyTrinh>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuyTrinh__3214EC274EEC901D");

            entity.ToTable("QuyTrinhXuLy_BuocQuyTrinh");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Guid).HasMaxLength(255);
            entity.Property(e => e.IdquyTrinhXuLy).HasColumnName("IDQuyTrinhXuLy");
            entity.Property(e => e.TenBuocQt)
                .HasMaxLength(255)
                .HasColumnName("TenBuocQT");

            entity.HasOne(d => d.IdquyTrinhXuLyNavigation).WithMany(p => p.QuyTrinhXuLyBuocQuyTrinhs)
                .HasForeignKey(d => d.IdquyTrinhXuLy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuyTrinhX__IDQuy__4CA06362");
        });

        modelBuilder.Entity<QuyTrinhXuLyNhayBuoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuyTrinh__3214EC27FDFF148A");

            entity.ToTable("QuyTrinhXuLy_NhayBuoc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdbuocQuyTrinh).HasColumnName("IDBuocQuyTrinh");
            entity.Property(e => e.IdbuocTiepTheo).HasColumnName("IDBuocTiepTheo");
            entity.Property(e => e.TenBuocChuyen).HasMaxLength(255);

            entity.HasOne(d => d.IdbuocQuyTrinhNavigation).WithMany(p => p.QuyTrinhXuLyNhayBuocIdbuocQuyTrinhNavigations)
                .HasForeignKey(d => d.IdbuocQuyTrinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuyTrinhX__IDBuo__4F7CD00D");

            entity.HasOne(d => d.IdbuocTiepTheoNavigation).WithMany(p => p.QuyTrinhXuLyNhayBuocIdbuocTiepTheoNavigations)
                .HasForeignKey(d => d.IdbuocTiepTheo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuyTrinhX__IDBuo__5070F446");
        });

        modelBuilder.Entity<SoHopDongDoDac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SoHopDon__3214EC275BF71F07");

            entity.ToTable("SoHopDong_DoDac");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IddonViCongTac).HasColumnName("IDDonViCongTac");
            entity.Property(e => e.IdtaiKhoan).HasColumnName("IDTaiKhoan");
            entity.Property(e => e.ThoiGian).HasColumnType("datetime");

            entity.HasOne(d => d.IddonViCongTacNavigation).WithMany(p => p.SoHopDongDoDacs)
                .HasForeignKey(d => d.IddonViCongTac)
                .HasConstraintName("FK__SoHopDong__IDDon__6E01572D");

            entity.HasOne(d => d.IdtaiKhoanNavigation).WithMany(p => p.SoHopDongDoDacs)
                .HasForeignKey(d => d.IdtaiKhoan)
                .HasConstraintName("FK__SoHopDong__IDTai__6D0D32F4");
        });

        modelBuilder.Entity<SystemLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemLo__3214EC27ACA34811");

            entity.ToTable("SystemLog");

            entity.HasIndex(e => e.Timestamp, "IX_SystemLog_timestamp");

            entity.HasIndex(e => e.UserId, "IX_SystemLog_user_id");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.SystemLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SystemLog__user___47DBAE45");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaiKhoan__3214EC278F729199");
            entity.ToTable("TaiKhoan");
            entity.HasIndex(e => e.IdchucVu, "IX_TaiKhoan_IDChucVu");
            entity.HasIndex(e => e.IddonViCongTac, "IX_TaiKhoan_IDDonViCongTac");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HoVaTen).HasMaxLength(255);
            entity.Property(e => e.IdchucVu).HasColumnName("IDChucVu");
            entity.Property(e => e.IddonViCongTac).HasColumnName("IDDonViCongTac");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.TenTaiKhoan).HasMaxLength(255);
            entity.HasOne(d => d.IdchucVuNavigation).WithMany(p => p.TaiKhoans)

                .HasForeignKey(d => d.IdchucVu)

                .HasConstraintName("FK__TaiKhoan__IDChuc__3E52440B");
            entity.HasOne(d => d.IddonViCongTacNavigation).WithMany(p => p.TaiKhoans)

                .HasForeignKey(d => d.IddonViCongTac)

                .HasConstraintName("FK__TaiKhoan__IDDonV__3F466844");

        });

        modelBuilder.Entity<TrangThaiBanVe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrangTha__3214EC271628A4C4");

            entity.ToTable("TrangThaiBanVe");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TrangThai).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
