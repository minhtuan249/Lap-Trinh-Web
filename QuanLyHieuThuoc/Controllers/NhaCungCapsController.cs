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
    public class NhaCungCapsController : Controller
    {
        private readonly MyDbContext _context;

        public NhaCungCapsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: NhaCungCaps (Allowed for all staff: Quản lý & Dược sĩ)
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Index()
        {
            // Lọc ra các mục chưa bị Xóa Mềm
            return View(await _context.NhaCungCaps.Where(n => n.IsDeleted == false).ToListAsync());
        }

        // GET: NhaCungCaps/Details/5 (Allowed for all staff)
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps
                // Chỉ lấy mục chưa bị Xóa Mềm
                .FirstOrDefaultAsync(m => m.NhaCungCapId == id && m.IsDeleted == false);

            if (nhaCungCap == null) return NotFound();
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Create (Allowed for all staff: Quản lý & Dược sĩ)
        [Authorize(Policy = "YeuCauNhanVien")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaCungCaps/Create (Allowed for all staff)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauNhanVien")]
        public async Task<IActionResult> Create([Bind("NhaCungCapId,TenNhaCungCap,DiaChi,SoDienThoai,Email,IsDeleted")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                nhaCungCap.IsDeleted = false; // Mặc định là chưa xóa khi tạo mới
                _context.Add(nhaCungCap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Edit/5 (Restricted to Admin Only)
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            // Kiểm tra: Tồn tại VÀ chưa bị xóa
            if (nhaCungCap == null || nhaCungCap.IsDeleted == true) return NotFound();

            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Edit/5 (Restricted to Admin Only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Edit(int id, [Bind("NhaCungCapId,TenNhaCungCap,DiaChi,SoDienThoai,Email,IsDeleted")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.NhaCungCapId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaCungCap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaCungCapExists(nhaCungCap.NhaCungCapId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Delete/5 (Restricted to Admin Only)
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps
                // Lọc ra các mục đã bị Xóa Mềm
                .FirstOrDefaultAsync(m => m.NhaCungCapId == id && m.IsDeleted == false);

            if (nhaCungCap == null) return NotFound();
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Delete/5 (Soft Delete logic, Restricted to Admin Only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "YeuCauQuanLy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap != null)
            {
                nhaCungCap.IsDeleted = true; // Changed to Soft Delete
                _context.Update(nhaCungCap); // Update the entity state
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaCungCapExists(int id)
        {
            return _context.NhaCungCaps.Any(e => e.NhaCungCapId == id);
        }
    }
}