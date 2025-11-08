using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Data;
using QuanLyHieuThuoc.Models.Entity;

namespace QuanLyHieuThuoc.Controllers
{
    public class HoaDons1Controller : Controller
    {
        private readonly MyDbContext _context;

        public HoaDons1Controller(MyDbContext context)
        {
            _context = context;
        }

        // GET: HoaDons1
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.HoaDons.Include(h => h.KhachHang).Include(h => h.NhanVien);
            return View(await myDbContext.ToListAsync());
        }

        // GET: HoaDons1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: HoaDons1/Create
        public IActionResult Create()
        {
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang");
            ViewData["NhanVienId"] = new SelectList(_context.NhanViens, "NhanVienId", "ChucVu");
            return View();
        }

        // POST: HoaDons1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoaDonId,NgayBan,TongTien,NhanVienId,KhachHangId")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang", hoaDon.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(_context.NhanViens, "NhanVienId", "ChucVu", hoaDon.NhanVienId);
            return View(hoaDon);
        }

        // GET: HoaDons1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang", hoaDon.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(_context.NhanViens, "NhanVienId", "ChucVu", hoaDon.NhanVienId);
            return View(hoaDon);
        }

        // POST: HoaDons1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HoaDonId,NgayBan,TongTien,NhanVienId,KhachHangId")] HoaDon hoaDon)
        {
            if (id != hoaDon.HoaDonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.HoaDonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang", hoaDon.KhachHangId);
            ViewData["NhanVienId"] = new SelectList(_context.NhanViens, "NhanVienId", "ChucVu", hoaDon.NhanVienId);
            return View(hoaDon);
        }

        // GET: HoaDons1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: HoaDons1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.HoaDonId == id);
        }
    }
}
