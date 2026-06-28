using HeThongQuanLyVanPhong.Filters;
using HeThongQuanLyVanPhong.Models;
using HeThongQuanLyVanPhong.Repositories;
using HeThongQuanLyVanPhong.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HeThongQuanLyVanPhong
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC + API Controllers (filter session tập trung)
            builder.Services.AddControllersWithViews(options =>
                {
                    options.Filters.Add<RequireSessionFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS (vẫn giữ, nhưng không cần thiết nếu chỉ dùng nội bộ)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowDev", policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            // DbContext — connection string duy nhất từ appsettings.json
            builder.Services.AddDbContext<HeThongQuanLyVanPhongContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            builder.Services.AddScoped<TaiKhoanRepository>();
            builder.Services.AddScoped<ModuleRepository>();
            builder.Services.AddScoped<SystemLogRepository>();
            builder.Services.AddScoped<DanhMucRepository>();
            builder.Services.AddScoped<QuyTrinhRepository>();
            builder.Services.AddScoped<DropdownRepository>();
            builder.Services.AddScoped<LuuTruRepository>();
            builder.Services.AddScoped<SoManhRepository>();
            builder.Services.AddScoped<DonGiaRepository>();
            builder.Services.AddScoped<ThanhToanRepository>();
            builder.Services.AddScoped<FileQuetRepository>();
            builder.Services.AddScoped<BanVeRepository>();
            builder.Services.AddScoped<HoSoDoDacRepository>();

            // Services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TaiKhoanService>();
            builder.Services.AddScoped<DanhMucService>();
            builder.Services.AddScoped<QuyTrinhService>();
            builder.Services.AddScoped<DropdownService>();
            builder.Services.AddScoped<LuuTruService>();
            builder.Services.AddScoped<SoManhService>();
            builder.Services.AddScoped<DonGiaThanhToanService>();
            builder.Services.AddScoped<FileQuetService>();
            builder.Services.AddScoped<BanVeService>();
            builder.Services.AddScoped<XuatFileService>();
            builder.Services.AddScoped<HoSoDoDacService>();
            builder.Services.AddScoped<BaoCaoService>();
            //
            builder.Services.AddHttpContextAccessor();
            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();          // Cho phép truy cập wwwroot
            app.UseRouting();

            app.UseCors("AllowDev");
            app.UseSession();
            app.UseAuthorization();

            // Route cho API controllers (attribute routing)
            app.MapControllers();

            // Route mặc định cho MVC (trang chủ)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}