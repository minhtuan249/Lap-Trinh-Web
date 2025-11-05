using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyHieuThuoc.Models.Entity
{
    public class PhieuNhap
    {
        [Key]
        public int PhieuNhapId { get; set; }

        [Required]
        public DateTime NgayNhap { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền nhập không thể âm")]
        public decimal TongTienNhap { get; set; } // Sẽ được tính toán từ ChiTietPhieuNhap

        // --- Mối quan hệ 1-N với NhanVien ---
        // Một nhân viên có thể lập nhiều phiếu nhập
        [Required]
        public int NhanVienId { get; set; } // Khóa ngoại

        [ForeignKey("NhanVienId")]
        public virtual NhanVien NhanVien { get; set; } // Thuộc tính điều hướng

        // --- Mối quan hệ 1-N với NhaCungCap ---
        // Một nhà cung cấp có thể có nhiều phiếu nhập
        [Required]
        public int NhaCungCapId { get; set; } // Khóa ngoại

        [ForeignKey("NhaCungCapId")]
        public virtual NhaCungCap NhaCungCap { get; set; } // Thuộc tính điều hướng

        // --- Mối quan hệ 1-N với ChiTietPhieuNhap ---
        // Một phiếu nhập có NHIỀU dòng chi tiết
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

        // Constructor
        public PhieuNhap()
        {
            ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
            TongTienNhap = 0; 
        }



    }
}