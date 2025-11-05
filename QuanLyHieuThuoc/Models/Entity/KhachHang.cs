using System.ComponentModel.DataAnnotations;
namespace QuanLyHieuThuoc.Models.Entity
{
    public class KhachHang
    {
        [Key]
        public int KhachHangId { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải nhập Tên Khách Hàng")]
        [StringLength(50)]
        public string TenKhachHang { get; set; }

        [StringLength(11)]
        [RegularExpression(@"^0[3|5|7|8|9]{1}[0-9]{8}$", ErrorMessage = "Số điện thoại không hợp lệ, vui lòng nhập lại")]
        public string? SoDienThoai { get; set; }

        [StringLength(250)]
        public string? DiaChi { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }

        public KhachHang()
        {
            HoaDons = new HashSet<HoaDon>();
        }


        public bool IsDeleted { get; set; } = false;
    }
}