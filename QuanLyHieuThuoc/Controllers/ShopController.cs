using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Data;
using System.Linq;

namespace QuanLyHieuThuoc.Controllers
{
    public class ShopController : Controller
    {
        private readonly MyDbContext _context;

        public ShopController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var thuocsQuery = _context.Thuocs
                .Include(t => t.LoaiThuoc)
                .Where(t => t.IsDeleted == false);

            // === LOGIC TÌM KIẾM ===
            if (!String.IsNullOrEmpty(searchString))
            {
                // Dùng Contains để tìm kiếm (không phân biệt chữ hoa, chữ thường)
                thuocsQuery = thuocsQuery.Where(s => s.TenThuoc.Contains(searchString));

                // Lưu lại chuỗi tìm kiếm để hiển thị lại trên ô input
                ViewData["CurrentSearchString"] = searchString;
            }

            return View(await thuocsQuery.ToListAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            var thuoc = await _context.Thuocs
                .Include(t => t.LoaiThuoc)
                .FirstOrDefaultAsync(t => t.ThuocId == id);

            if (thuoc == null)
                return NotFound();

            return View(thuoc);
        }

    }
}