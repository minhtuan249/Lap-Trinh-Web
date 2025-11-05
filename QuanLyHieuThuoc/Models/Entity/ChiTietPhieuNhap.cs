using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyHieuThuoc.Models.Entity
{
    public class ChiTietPhieuNhap
    {
        [Key]
        public int ChiTietPhieuNhapId { get; set; }

        // --- Dữ liệu giao dịch (Nhập kho) ---
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn 0")]
        public int SoLuongNhap { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá nhập không thể âm")]
        public decimal GiaNhap { get; set; } // Giá vốn của lô hàng này

        // --- Mối quan hệ Nhiều-Một với PhieuNhap ---
        [Required]
        public int PhieuNhapId { get; set; } // Khóa ngoại

        [ForeignKey("PhieuNhapId")]
        public virtual PhieuNhap PhieuNhap { get; set; } // Thuộc tính điều hướng

        // --- Mối quan hệ Nhiều-Một với Thuoc ---
        [Required]
        public int ThuocId { get; set; } // Khóa ngoại

        [ForeignKey("ThuocId")]
        public virtual Thuoc Thuoc { get; set; } // Thuộc tính điều hướng



    }
}