using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Entity;
namespace QuanLyHieuThuoc.Models.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Thuoc> Thuocs {get; set;}
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<LoaiThuoc> LoaiThuocs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }



    }
}
