using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Data;
using QuanLyHieuThuoc.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuanLyHieuThuoc.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;

        public AccountController(MyDbContext context)
        {
            _context = context; // Dùng Dependency Injection để lấy DbContext
        }

        // Model để nhận dữ liệu từ Form Login
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Tên đăng nhập là bắt buộc nha bé ơi")]
            public string TenDangNhap { get; set; }

            [Required(ErrorMessage = "Mật khẩu là bắt buộc nha bé ơi")]
            [DataType(DataType.Password)]
            public string MatKhau { get; set; }
        }

        // --- B1: HIỂN THỊ FORM LOGIN (GET) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }

        // --- B2: XỬ LÝ LOGIN (POST) ---
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // So sánh với database
                var user = await _context.NhanViens
                    .FirstOrDefaultAsync(u => u.TenDangNhap == model.TenDangNhap);

                // Kiểm tra User có tồn tại và mật khẩu có khớp không
                if (user != null && user.MatKhau == model.MatKhau)
                {
                    // TẠO "THẺ TÊN" (Claims)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.TenDangNhap),
                        new Claim("HoTen", user.HoTen),
                        new Claim(ClaimTypes.Role, user.ChucVu) // So sánh chức vụ
                    };

                   //Tạo bản chứng nhận
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // ĐĂNG NHẬP 
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Thuocs"); // Đăng nhập thành công -> về trang chủ
                }

                // Nếu sai mật khẩu hoặc user
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return View(model); // Trả về form Login và báo lỗi
        }

        // --- BƯỚC 3: XỬ LÝ LOGOUT (Đăng xuất) ---
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Shop"); // Đăng xuất xong -> về trang chủ
        }

        
    }
}