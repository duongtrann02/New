using System.ComponentModel.DataAnnotations;

namespace New_Project.Models.ViewModels
{
    public class RegisterViewModel
    {
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
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        [Display(Name = "Hình Ảnh Cá Nhân URL")]
        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        [RegularExpression(@"^https?:\/\/.*\.(jpg|jpeg|png|gif|bmp|svg)$", 
            ErrorMessage = "URL phải trỏ đến một hình ảnh hợp lệ (jpg, jpeg, png, gif, bmp, svg)")]
        public string ProfilePictureUrl { get; set; }
    }
}
