using System.ComponentModel.DataAnnotations;

namespace QuanLyHieuThuoc.Models.Entity
{
    public class NhanVien
    {
        [Key]
        public int NhanVienId { get; set; }

        [Required(ErrorMessage = "Tên nhân viên là bắt buộc")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Chức vụ là bắt buộc")]
        [StringLength(50)]
        public string ChucVu { get; set; }

        [StringLength(15)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? SoDienThoai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(100)]
        public string MatKhau { get; set; }

        // --- Mối quan hệ Một-Nhiều ---
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }

        // Constructor
        public NhanVien()
        {
            HoaDons = new HashSet<HoaDon>();
            PhieuNhaps = new HashSet<PhieuNhap>();
        }

        public bool IsDeleted { get; set; } = false;
    }
}