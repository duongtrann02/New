using System.ComponentModel.DataAnnotations;

namespace New_Project.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Nhớ tôi")]
        public bool RememberMe { get; set; }
    }
}
