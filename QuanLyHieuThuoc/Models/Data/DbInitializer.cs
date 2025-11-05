using Microsoft.EntityFrameworkCore;
using QuanLyHieuThuoc.Models.Entity;
using System.Linq;

namespace QuanLyHieuThuoc.Models.Data
{
    public class DbInitializer
    {
        public static void Initializer(IServiceProvider serviceProvider)
        {
            var contextOptions = serviceProvider.GetRequiredService<DbContextOptions<MyDbContext>>();
            using (var context = new MyDbContext(contextOptions))
            {
                // Lệnh tạo database (nếu chưa có)
                context.Database.EnsureCreated();

                // Dùng Any() để kiểm tra nếu database không trống
                if (context.NhanViens.Any())
                {
                    return;
                }

                // === 1. TẠO CÁC DANH MỤC BAN ĐẦU (Master Data) ===

                var LoaiThuocs = new LoaiThuoc[]
                {
                    new LoaiThuoc { TenLoaiThuoc = "Thuốc kháng sinh" },
                    new LoaiThuoc { TenLoaiThuoc = "Thuốc giảm đau" },
                    new LoaiThuoc { TenLoaiThuoc = "Vitamin và khoáng chất" },
                    new LoaiThuoc { TenLoaiThuoc = "Sơ cứu & Băng gạc" }
                };
                context.LoaiThuocs.AddRange(LoaiThuocs);

                var nccUmbrella = new NhaCungCap { TenNhaCungCap = "Umbrella Corporation", DiaChi = "Racoon", SoDienThoai = "111" };
                var nccTricell = new NhaCungCap { TenNhaCungCap = "TriCell Inc", DiaChi = "Khu tự trị Kijuju", SoDienThoai = "222" };
                context.NhaCungCaps.AddRange(nccUmbrella, nccTricell);

                // --- NhanVien (Chỉ giữ 2 Role: Quản lý và Dược sĩ) ---

                var nvWesker = new NhanVien
                {
                    HoTen = "Albert Wesker",
                    ChucVu = "Quản lý", 
                    TenDangNhap = "wesker",
                    MatKhau = "Wesker@123", 
                    SoDienThoai = "0900000001"
                };
                var nvJill = new NhanVien
                {
                    HoTen = "Jill Valentine",
                    ChucVu = "Dược sĩ", 
                    TenDangNhap = "jill",
                    MatKhau = "Jill@123",
                    SoDienThoai = "0900000002"
                };
                var nvRebecca = new NhanVien
                {
                    HoTen = "Rebecca Chambers",
                    ChucVu = "Dược sĩ",
                    TenDangNhap = "rebecca",
                    MatKhau = "Rebecca@123",
                    SoDienThoai = "0900000003"
                };
                context.NhanViens.AddRange(nvWesker, nvJill, nvRebecca);
                context.SaveChanges();

                var khEthan = new KhachHang { TenKhachHang = "Ethan Winters", SoDienThoai = "900555666" };
                var khClaire = new KhachHang { TenKhachHang = "Claire Redfield", SoDienThoai = "900777888" };
                context.KhachHangs.AddRange(khEthan, khClaire);

                // LƯU CÁC ĐỐI TƯỢNG CHA (Master Data) CÙNG LÚC
                context.SaveChanges();

                // === 2. SỬ DỤNG ID CỦA ĐỐI TƯỢNG VỪA LƯU ===

                // Tìm lại LoaiThuocId (vì ID đã được gán sau SaveChanges)
                var idGiamDau = LoaiThuocs.Single(l => l.TenLoaiThuoc == "Thuốc giảm đau").LoaiThuocId;
                var idKhangSinh = LoaiThuocs.Single(l => l.TenLoaiThuoc == "Thuốc kháng sinh").LoaiThuocId;
                var idVitamin = LoaiThuocs.Single(l => l.TenLoaiThuoc == "Vitamin và khoáng chất").LoaiThuocId;
                var idSoCuu = LoaiThuocs.Single(l => l.TenLoaiThuoc == "Sơ cứu & Băng gạc").LoaiThuocId;

                var thuocParacetamol = new Thuoc
                {
                    TenThuoc = "Paracetamol 500mg",
                    MoTa = "Giảm đau, hạ sốt",
                    GiaBan = 15000,
                    SoLuongTon = 100,
                    QuyCachDongGoi = "Vỉ 10 viên",
                    LoaiThuocId = idGiamDau
                };
                var thuocFirstAid = new Thuoc
                {
                    TenThuoc = "First Aid Spray (Dạng xịt)",
                    MoTa = "Sát khuẩn",
                    GiaBan = 120000,
                    SoLuongTon = 75,
                    QuyCachDongGoi = "Chai 250ml",
                    LoaiThuocId = idSoCuu
                };
                context.Thuocs.AddRange(thuocParacetamol, thuocFirstAid);
                context.SaveChanges();


                var dsThuoc = new Thuoc[]
                {
                    new Thuoc { TenThuoc = "Green Herb", MoTa = "Giảm đau nhẹ, tăng sức bền", QuyCachDongGoi = "Lá khô", GiaBan = 50000, SoLuongTon = 200, LoaiThuocId = idGiamDau },
                    new Thuoc { TenThuoc = "Red Herb", MoTa = "Tăng cường hiệu quả dược tính", QuyCachDongGoi = "Lá khô", GiaBan = 75000, SoLuongTon = 50, LoaiThuocId = idVitamin },
                    new Thuoc { TenThuoc = "Mixed Herb (G+R)", MoTa = "Phục hồi sức khỏe toàn diện", QuyCachDongGoi = "Hộp viên nén", GiaBan = 150000, SoLuongTon = 30, LoaiThuocId = idVitamin },
                    new Thuoc { TenThuoc = "Ambrose Vaccine", MoTa = "Kháng khuẩn cấp tốc", QuyCachDongGoi = "Ống tiêm", GiaBan = 250000, SoLuongTon = 20, LoaiThuocId = idKhangSinh },
                    new Thuoc { TenThuoc = "Calcitrol (Kháng Virut)", MoTa = "Thuốc kháng virus mạnh", QuyCachDongGoi = "Vỉ 10 viên", GiaBan = 450000, SoLuongTon = 40, LoaiThuocId = idKhangSinh },
                    new Thuoc { TenThuoc = "Bandage (Quấn vết thương)", MoTa = "Băng bó vết thương hở", QuyCachDongGoi = "Cuộn lớn", GiaBan = 30000, SoLuongTon = 300, LoaiThuocId = idSoCuu },
                    new Thuoc { TenThuoc = "PainAway Extra", MoTa = "Giảm đau cơ và khớp", QuyCachDongGoi = "Lọ 50 viên", GiaBan = 45000, SoLuongTon = 120, LoaiThuocId = idGiamDau }
                };
                context.Thuocs.AddRange(dsThuoc);
                context.SaveChanges();

                // === 3. TẠO GIAO DỊCH MẪU ===

                // Phiếu Nhập
                var phieunhap = new PhieuNhap
                {
                    NgayNhap = DateTime.Now.AddDays(-1),
                    TongTienNhap = 120000 * 75,
                    NhanVienId = nvJill.NhanVienId, // Dùng ID (đã có)
                    NhaCungCapId = nccUmbrella.NhaCungCapId
                };
                context.PhieuNhaps.Add(phieunhap);
                context.SaveChanges(); // <-- Phải Save để PhieuNhap có ID

                // Chi Tiết Nhập
                var chiTietNhap = new ChiTietPhieuNhap
                {
                    SoLuongNhap = 75,
                    GiaNhap = 120000,
                    PhieuNhapId = phieunhap.PhieuNhapId,
                    ThuocId = thuocFirstAid.ThuocId
                };
                context.ChiTietPhieuNhaps.Add(chiTietNhap);

                // Hóa Đơn Bán
                var hoaDon = new HoaDon
                {
                    NgayBan = DateTime.Now,
                    TongTien = (15000 * 2) + 120000,
                    NhanVienId = nvJill.NhanVienId,
                    KhachHangId = khEthan.KhachHangId
                };
                context.HoaDons.Add(hoaDon);
                context.SaveChanges(); // <-- Phải Save để HoaDon có ID

                var chiTietBan1 = new ChiTietHoaDon { SoLuong = 2, DonGia = 15000, HoaDonId = hoaDon.HoaDonId, ThuocId = thuocParacetamol.ThuocId };
                context.ChiTietHoaDons.Add(chiTietBan1);

                // Cập nhật kho
                thuocParacetamol.SoLuongTon -= 2;
                context.Thuocs.Update(thuocParacetamol);

                context.SaveChanges(); // Lưu tất cả các thay đổi cuối cùng
            }
        }
    }
}

