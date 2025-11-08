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
    public class NhanViensController : Controller
    {
        private readonly MyDbContext _context;

        public NhanViensController(MyDbContext context)
        {
            _context = context;
        }

        // GET: NhanViens (Allowed for all staff: Quản lý & Dược sĩ)
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Index()
        {
            // Lọc ra các mục đã bị Xóa Mềm
            return View(await _context.NhanViens.Where(n => n.IsDeleted == false).ToListAsync());
        }

        // GET: NhanViens/Details/5 (Allowed for all staff)
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nhanVien = await _context.NhanViens
                // Lọc ra các mục đã bị Xóa Mềm
                .FirstOrDefaultAsync(m => m.NhanVienId == id && m.IsDeleted == false);

            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        // GET: NhanViens/Create (Allowed for all staff: Quản lý & Dược sĩ)
        [Authorize(Policy = "YeuCauNhanVien")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhanViens/Create (Allowed for all staff)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Create([Bind("NhanVienId,HoTen,ChucVu,SoDienThoai,TenDangNhap,MatKhau,IsDeleted")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                nhanVien.IsDeleted = false; // Mặc định là chưa xóa
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5 (Restricted to Admin Only)
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nhanVien = await _context.NhanViens.FindAsync(id);
            // Kiểm tra: Tồn tại VÀ chưa bị xóa
            if (nhanVien == null || nhanVien.IsDeleted == true) return NotFound();

            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5 (Restricted to Admin Only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Edit(int id, [Bind("NhanVienId,HoTen,ChucVu,SoDienThoai,TenDangNhap,MatKhau,IsDeleted")] NhanVien nhanVien)
        {
            if (id != nhanVien.NhanVienId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.NhanVienId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5 (Restricted to Admin Only)
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var nhanVien = await _context.NhanViens
                // Lọc ra các mục đã bị Xóa Mềm
                .FirstOrDefaultAsync(m => m.NhanVienId == id && m.IsDeleted == false);

            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5 (Soft Delete logic, Restricted to Admin Only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                nhanVien.IsDeleted = true; // Changed to Soft Delete
                _context.Update(nhanVien); // Update the entity state
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.NhanVienId == id);
        }
    }
}