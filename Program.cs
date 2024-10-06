// Program.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using New_Project.Services;

var builder = WebApplication.CreateBuilder(args);

// Thêm các dịch vụ vào DI Container
builder.Services.AddControllersWithViews();

// Đăng ký FirebaseService với DI Container thông qua interface
builder.Services.AddSingleton<IFirebaseService, FirebaseService>();

// Thêm dịch vụ Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        // Cấu hình thêm nếu cần
    });

    builder.Services.AddDistributedMemoryCache(); // Để lưu trữ tạm thời
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Cần thiết cho session hoạt động
});

var app = builder.Build();

// Cấu hình pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Sử dụng Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
