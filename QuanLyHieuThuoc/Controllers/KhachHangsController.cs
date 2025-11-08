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
    public class KhachHangsController : Controller
    {
        private readonly MyDbContext _context;

        public KhachHangsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: KhachHangs (Allowed for all staff: Quản lý & Dược sĩ)
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Index()
        {
            // Lọc ra các mục đã bị Xóa Mềm
            return View(await _context.KhachHangs.Where(k => k.IsDeleted == false).ToListAsync());
        }

        // GET: KhachHangs/Details/5 (Allowed for all staff)
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.KhachHangId == id && m.IsDeleted == false);

            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        // GET: KhachHangs/Create (Allowed for all staff: Quản lý & Dược sĩ)
        [Authorize(Policy = "YeuCauNhanVien")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhachHangs/Create (Allowed for all staff)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Create([Bind("KhachHangId,TenKhachHang,SoDienThoai,DiaChi,IsDeleted")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                khachHang.IsDeleted = false; // Mặc định là chưa xóa
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5 (CHỈ Quản lý)
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs.FindAsync(id);
            // Kiểm tra: Tồn tại VÀ chưa bị xóa
            if (khachHang == null || khachHang.IsDeleted == true) return NotFound();

            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5 (CHỈ Quản lý)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Edit(int id, [Bind("KhachHangId,TenKhachHang,SoDienThoai,DiaChi,IsDeleted")] KhachHang khachHang)
        {
            if (id != khachHang.KhachHangId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.KhachHangId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5 (CHỈ Quản lý)
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.KhachHangId == id && m.IsDeleted == false);

            if (khachHang == null) return NotFound();
            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5 (Xóa Mềm - CHỈ Quản lý)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                khachHang.IsDeleted = true; // Xóa mềm
                _context.Update(khachHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.KhachHangId == id);
        }
    }
}