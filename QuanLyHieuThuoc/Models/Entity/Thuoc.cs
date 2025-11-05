using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QuanLyHieuThuoc.Models.Entity
{
    public class Thuoc
    {
        [Key]
        public int ThuocId { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập tên thuốc")]
        [StringLength(100)]
        public string TenThuoc { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Phải nhập Quy Cách Đóng Gói")]
        [StringLength(50)]
        public string QuyCachDongGoi { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập giá bán")]
        [Range(0, int.MaxValue, ErrorMessage = "Không thể bán giá âm")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaBan { get; set; }

        [Required(ErrorMessage = "Phải nhập số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số Lượng Tồn không thể âm")]
        public int SoLuongTon { get; set; }


        // 1.Mối quan hệ 1-N với LoaiThuoc
        [Required(ErrorMessage = "Vui long chon Loại Thuốc")]
        public int LoaiThuocId { get; set; }

        [ForeignKey("LoaiThuocId")]
        public virtual LoaiThuoc? LoaiThuoc { get; set; }

        //2. Mối quan hệ N-N: ChiTietHoaDon, ChiTietPhieuNhap
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

        public bool IsDeleted { get; set; } = false;

        //3. Constructor
        public Thuoc()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
            ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
        }
    }
}
