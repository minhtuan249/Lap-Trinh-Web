using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Data;
using QuanLyHieuThuoc.Models.Entity;
using Microsoft.AspNetCore.Authorization; // <-- BẮT BUỘC

namespace QuanLyHieuThuoc.Controllers
{
    // KHÔNG ĐẶT [Authorize] ở đây. Chúng ta sẽ đặt ở từng hàm cụ thể.
    public class ThuocsController : Controller
    {
        private readonly MyDbContext _context;

        public ThuocsController(MyDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "YeuCauDangNhap")]
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Thuocs
                .Include(t => t.LoaiThuoc)
                .Where(t => t.IsDeleted == false);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Thuocs/Details/5 - Bất cứ ai đăng nhập cũng được xem
        [Authorize(Policy = "YeuCauDangNhap")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var thuoc = await _context.Thuocs
                .Include(t => t.LoaiThuoc)
                .FirstOrDefaultAsync(m => m.ThuocId == id);

            if (thuoc == null || thuoc.IsDeleted == true) return NotFound();
            return View(thuoc);
        }

        // GET: Thuocs/Create - CHỈ "Quản lý" mới được Thêm
        [Authorize(Policy = "YeuCauQuanLy")]
        public IActionResult Create()
        {
            ViewData["LoaiThuocId"] = new SelectList(_context.LoaiThuocs.Where(l => l.IsDeleted == false), "LoaiThuocId", "TenLoaiThuoc");
            return View();
        }

        // POST: Thuocs/Create - CHỈ "Quản lý" mới được Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Create([Bind("ThuocId,TenThuoc,MoTa,QuyCachDongGoi,GiaBan,SoLuongTon,LoaiThuocId")] Thuoc thuoc)
        {
            if (ModelState.IsValid)
            {
                thuoc.IsDeleted = false;
                _context.Add(thuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiThuocId"] = new SelectList(_context.LoaiThuocs.Where(l => l.IsDeleted == false), "LoaiThuocId", "TenLoaiThuoc", thuoc.LoaiThuocId);
            return View(thuoc);
        }

        // GET: Thuocs/Edit/5 - Bất cứ ai đăng nhập cũng được Sửa
        [Authorize(Policy = "YeuCauDangNhap")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null || thuoc.IsDeleted == true) return NotFound();

            ViewData["LoaiThuocId"] = new SelectList(_context.LoaiThuocs.Where(l => l.IsDeleted == false), "LoaiThuocId", "TenLoaiThuoc", thuoc.LoaiThuocId);
            return View(thuoc);
        }

        // POST: Thuocs/Edit/5 - Bất cứ ai đăng nhập cũng được Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauDangNhap")]
        public async Task<IActionResult> Edit(int id, [Bind("ThuocId,TenThuoc,MoTa,QuyCachDongGoi,GiaBan,SoLuongTon,LoaiThuocId,IsDeleted")] Thuoc thuoc)
        {
            if (id != thuoc.ThuocId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Thuocs.Any(e => e.ThuocId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiThuocId"] = new SelectList(_context.LoaiThuocs.Where(l => l.IsDeleted == false), "LoaiThuocId", "TenLoaiThuoc", thuoc.LoaiThuocId);
            return View(thuoc);
        }

        // GET: Thuocs/Delete/5 - CHỈ "Quản lý" mới được Xóa
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var thuoc = await _context.Thuocs
                .Include(t => t.LoaiThuoc)
                .Where(t => t.IsDeleted == false)
                .FirstOrDefaultAsync(m => m.ThuocId == id);

            if (thuoc == null) return NotFound();
            return View(thuoc);
        }

        // POST: Thuocs/Delete/5 - CHỈ "Quản lý" mới được Xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc != null)
            {
                thuoc.IsDeleted = true;
                _context.Update(thuoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuocExists(int id)
        {
            return _context.Thuocs.Any(e => e.ThuocId == id);
        }




        // GET: Thuocs/DeletedItems - CHỈ QUẢN LÝ MỚI ĐƯỢC VÀO
        [Authorize(Roles = "Quản lý")]
        public async Task<IActionResult> DeletedItems()
        {
            var myDbContext = _context.Thuocs
                .Include(t => t.LoaiThuoc)
                .Where(t => t.IsDeleted == true); // <-- CHỈ LẤY THUỐC ĐÃ BỊ XÓA
            return View(await myDbContext.ToListAsync());
        }

        // POST: Thuocs/Restore/5 - CHỈ QUẢN LÝ MỚI ĐƯỢC PHỤC HỒI
        [HttpPost]
        [Authorize(Roles = "Quản lý")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc != null)
            {
                thuoc.IsDeleted = false; // <-- ĐẶT LẠI THÀNH FALSE (PHỤC HỒI)
                _context.Update(thuoc);
            }

            await _context.SaveChangesAsync();
            // Quay lại trang Thùng Rác để xem
            return RedirectToAction(nameof(DeletedItems));
        }
    }
}