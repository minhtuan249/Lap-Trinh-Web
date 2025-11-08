using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Data;
using QuanLyHieuThuoc.Models.Entity;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyHieuThuoc.Controllers
{
    [Authorize(Policy = "YeuCauNhanVien")] 
    public class HoaDonsController : Controller
    {
        private readonly MyDbContext _context;

        public HoaDonsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: HoaDons (Xem danh sách - Read)
        public async Task<IActionResult> Index()
        {
            // Liên kết với Khách hàng và Nhân viên để hiển thị tên
            var myDbContext = _context.HoaDons.Include(h => h.KhachHang).Include(h => h.NhanVien);
            return View(await myDbContext.ToListAsync());
        }

        // GET: HoaDons/Details/5 (Xem Chi tiết Hóa đơn)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                // Include ChiTietHoaDon và Thuoc để hiển thị danh sách sản phẩm
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.Thuoc)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);

            if (hoaDon == null) return NotFound();

            return View(hoaDon);
        }


        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.HoaDonId == id);
        }
    }
}