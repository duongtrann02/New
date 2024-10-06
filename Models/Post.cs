using System;
using System.ComponentModel.DataAnnotations;

namespace New_Project.Models
{
    public class Post
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Tác giả")]
        public string Author { get; set; }

        [Display(Name = "Ngày đăng")]
        public DateTime PostedDate { get; set; }
    }
}
