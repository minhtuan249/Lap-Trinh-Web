using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using QuanLyHieuThuoc.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

// === KHỐI SERVICE ===
var connectionString = builder.Configuration.GetConnectionString("MyDb");

// 1. Đăng ký DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Đăng ký Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddControllersWithViews();
// === KẾT THÚC KHỐI SERVICE ===






builder.Services.AddAuthorization(options =>
{
    // Plc 1: Cần quyền "Quản lý" (Chức năng Thêm/Xóa)
    options.AddPolicy("YeuCauQuanLy", policy =>
        policy.RequireRole("Quản lý"));

    // Plc 2: Chỉ cần là người dùng đã đăng nhập (Chức năng Xem/Sửa)
    options.AddPolicy("YeuCauDangNhap", policy =>
        policy.RequireAuthenticatedUser());
});








var app = builder.Build();

// === KHỐI INITIALIZER (BƯỚC NẠP DỮ LIỆU) ===
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        DbInitializer.Initializer(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the DB.");
    }
}
// === KẾT THÚC KHỐI INITIALIZER ===

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=Index}/{id?}");


app.Run();
