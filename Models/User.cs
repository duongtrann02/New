using System.ComponentModel.DataAnnotations;

namespace New_Project.Models
{
    public class User
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "URL Hình Ảnh Cá Nhân")]
        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        [RegularExpression(@"^https?:\/\/.*\.(jpg|jpeg|png|gif|bmp|svg)$", 
            ErrorMessage = "URL phải trỏ đến một hình ảnh hợp lệ (jpg, jpeg, png, gif, bmp, svg)")]
        public string ProfilePictureUrl { get; set; }
    }
}
