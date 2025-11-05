using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyHieuThuoc.Models.Entity
{
    public class ChiTietHoaDon
    {
        [Key]
        public int ChiTietHoaDonId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; } //Số lượng bán

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không thể âm")]
        public decimal DonGia { get; set; } 

        // --- Mối quan hệ N-1
        // Một hóa đơn có thể có nhiều chi tiết
        [Required]
        public int HoaDonId { get; set; } // Khóa ngoại

        [ForeignKey("HoaDonId")]
        public virtual HoaDon HoaDon { get; set; } 


        // Một loại thuốc có thể xuất hiện trong nhiều chi tiết
        [Required]
        public int ThuocId { get; set; } // Khóa ngoại

        [ForeignKey("ThuocId")]
        public virtual Thuoc Thuoc { get; set; } // Thuộc tính điều hướng

      
    }
}