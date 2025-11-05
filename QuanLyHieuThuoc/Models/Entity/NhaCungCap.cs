using System.ComponentModel.DataAnnotations;

namespace QuanLyHieuThuoc.Models.Entity
{
    public class NhaCungCap
    {
        [Key]
        public int NhaCungCapId { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc")]
        [StringLength(200)]
        public string TenNhaCungCap { get; set; }

        [StringLength(250)]
        public string? DiaChi { get; set; } 

        [StringLength(15)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? SoDienThoai { get; set; } // Cho phép null

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; } 

        // --- Mối quan hệ 1-N
        // Một nhà cung cấp có thể cung cấp NHIỀU phiếu nhập kho
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }

        // Constructor
        public NhaCungCap()
        {
            PhieuNhaps = new HashSet<PhieuNhap>();
        }

        public bool IsDeleted { get; set; } = false;
    }
}