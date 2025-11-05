using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QuanLyHieuThuoc.Models.Entity
{
    public class HoaDon
    {
        [Key]
        public int HoaDonId { get; set; }

        [Required]
        public DateTime NgayBan { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "Decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền không thể âm")]
        public decimal TongTien { get; set; }

        [Required]
        public int NhanVienId { get; set; }
        [ForeignKey("NhanVienId")]
        public virtual NhanVien NhanVien { get; set; }

        public int? KhachHangId { get; set; }
        [ForeignKey("KhachHangId")]
        public virtual KhachHang? KhachHang { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }


    }
}