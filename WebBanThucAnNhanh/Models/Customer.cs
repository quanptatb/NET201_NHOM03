using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThucAnNhanh.Models
{
    // Table("TenBang") là tùy chọn, nếu không có EF sẽ lấy tên theo DbSet
    [Table("Customers")] 
    public class Customer
    {
        [Key] // Khóa chính (Primary Key)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng (Identity)
        public int IdCustomer { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)] // Giới hạn độ dài nvarchar(100)
        public string FullName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }
    }
}