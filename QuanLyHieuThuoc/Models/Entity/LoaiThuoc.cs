using System.ComponentModel.DataAnnotations;
namespace QuanLyHieuThuoc.Models.Entity
{
    public class LoaiThuoc
    {
        [Key]
        public int LoaiThuocId { get; set; }

        [Required(ErrorMessage = "Phải nhập Tên Thuốc")]
        [StringLength(50)]
        public string TenLoaiThuoc { get; set; }

        //Mối quan hệ 1-N
        public virtual ICollection<Thuoc> Thuocs { get; set; }
        public LoaiThuoc()
        {
            Thuocs = new HashSet<Thuoc>();
        }



        public bool IsDeleted { get; set; } = false;
    }
}
